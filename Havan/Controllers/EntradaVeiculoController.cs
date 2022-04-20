using Havan.Context;
using Havan.Models;
using Havan.Models.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Havan.Controllers
{
    public class EntradaVeiculoController : Controller
    {
        private readonly Contexto _contexto;

        public EntradaVeiculoController(Contexto contexto)
        {
            _contexto = contexto;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _contexto.EntradaVeiculo.Where(x=> x.Status == false).ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Index(string search)
        {
            ViewData["GetEntradasDetails"] = search;

            var querry = from x in _contexto.EntradaVeiculo select x;
            if (!String.IsNullOrEmpty(search))
            {
                querry = querry.Where(x => x.Placa.Contains(search) && x.Status == false || x.Condutor.Nome.Contains(search) && x.Status == false);
            }

            return View(await querry.AsNoTracking().Where(x=> x.Status == false).ToListAsync());
        }

        public async Task<IActionResult> Historico()
        {
            return View(await _contexto.EntradaVeiculo.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Historico(BuscaModel busca)
        {
            ViewData["GetEntradasDetails"] = busca.result;

            var querry = from x in _contexto.EntradaVeiculo.Include(x =>x.Condutor) select x;

            if (!String.IsNullOrEmpty(busca.Search))
            {
                querry = querry.Where(x => x.Placa.Contains(busca.Search) || x.Condutor.Nome.Contains(busca.Search));
            }

            if (busca.Data != null)
            {
                querry = querry.Where(x => x.DataEntrada.Day == busca.Data.Value.Day);
            }

            if (busca.Ativo != null)
            {
                querry = querry.Where(x => x.Status == true);
            }

            return View(await querry.AsNoTracking().ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EntradaVeiculo entradaVeiculo)
        {
            if (ModelState.IsValid)
            {
                _contexto.Add(entradaVeiculo);
                await _contexto.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(entradaVeiculo);
        }

        public async Task<IActionResult> Finalizar(int? id)
        {
            if (id == 0)
                return NotFound();

            var entrada = await _contexto.EntradaVeiculo.Include(x => x.Condutor).Where(x => x.Id == id).FirstOrDefaultAsync();

            if (entrada == null)
                return NotFound();

            var preco = await _contexto.Preco
                 .FromSqlRaw("SELECT * FROM Preco WHERE @DataEntrada"
                 + " BETWEEN DataInicial AND DataFinal",
                 new SqlParameter("DataEntrada", entrada.DataEntrada))
                 .FirstOrDefaultAsync();

            var horaLivre = await _contexto.HorarioLivre.ToListAsync();

            var permanencia = (DateTime.Now - entrada.DataEntrada);

            var i = entrada.DataEntrada;

            if (horaLivre != null)
            {
                foreach (var livre in horaLivre)
                {
                    while (i < DateTime.Now)
                    {
                        if (i.DayOfWeek.ToString() == livre.Dia)
                        {
                            if (i < DateTime.Now)
                            {
                                permanencia -= (livre.HoraFinal.TimeOfDay - livre.HoraInicial.TimeOfDay);
                            }
                            else if(i.TimeOfDay >= livre.HoraInicial.TimeOfDay && i.TimeOfDay <= livre.HoraFinal.TimeOfDay)
                            {
                                permanencia -= (livre.HoraFinal.TimeOfDay - livre.HoraInicial.TimeOfDay);
                            }
                        }
                        i = i.AddDays(1);
                    }
                }
            }
            else
            {
                var HoraInicial = new TimeSpan(11, 30, 00);
                var HoraFinal = new TimeSpan(13, 00, 00);
              
                while (i < DateTime.Now)
                {
                    if (i.DayOfWeek.ToString() == "Monday" || i.DayOfWeek.ToString() == "Wednesday" || i.DayOfWeek.ToString() == "Thursday")
                    {
                        if (i < DateTime.Now)
                        {
                            permanencia -= (HoraFinal - HoraInicial);
                        }
                        else if (i.TimeOfDay >= HoraInicial && i.TimeOfDay <= HoraFinal)
                        {
                            permanencia -= (HoraFinal - HoraInicial);
                        }
                    }
                    i = i.AddDays(1);
                }
                
            }

            var saida = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

            entrada.Total = await CalculaTotal(entrada.CondutorId, preco.PrecoHora, preco.PrecoHoraAdicional, permanencia, entrada.Condutor);

            entrada.DataSaida = Convert.ToDateTime(saida);
            if (permanencia < new TimeSpan(00,00,00))
                entrada.Permanencia = "0";
            else
                entrada.Permanencia = permanencia.ToString(@"dd\.hh\:mm\:ss");

            entrada.ValorHora = preco != null ? preco.PrecoHora : 1;
            entrada.ValorHoraAdicional = preco != null ? preco.PrecoHoraAdicional : 2;

            return View(entrada);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Finalizar(EntradaVeiculo entradaVeiculo)
        {
            if (ModelState.IsValid)
            {
                entradaVeiculo.Status = true;
                _contexto.Update(entradaVeiculo);
                await _contexto.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(entradaVeiculo);
        }

        public async Task<double> CalculaTotal(int? condutorId, int valorHora, int valorHoraAdicional, TimeSpan permanencia, Condutor? condutor)
        {
            var valorTotal = 0;
           
            var minutos = permanencia.TotalMinutes;

            if (minutos > 30)
                valorTotal += valorTotal + valorHora;
            else
                valorTotal += valorTotal + (valorHora / 2);

            while (minutos >= 70)
            {
                valorTotal = valorTotal + valorHoraAdicional;
                minutos = minutos - 70;
            }

            if (condutorId != null)
            {
                if (condutor.Desconto != null && condutor.Desconto > 0)
                {
                    valorTotal = valorTotal / 2;
                    condutor.Desconto--;
                }
                else if
                    (permanencia.TotalHours >= 10)
                    condutor.Desconto = 2;

                _contexto.Condutor.Update(condutor);
                await _contexto.SaveChangesAsync();
            }
               
            return valorTotal;
        }
    }
}
