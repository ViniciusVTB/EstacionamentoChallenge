using Havan.Context;
using Havan.Models;
using Havan.Models.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Havan.Controllers
{
    public class PrecoController : Controller
    {
        private readonly Contexto _contexto;

        public PrecoController(Contexto contexto)
        {
            _contexto = contexto;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _contexto.Preco.ToListAsync());
        }

        public IActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View(new Preco());
            else
                return View(_contexto.Preco.Find(id));
        }

        [HttpPost]
        public async Task<RetornoHelper> AddOrEdit(Preco preco)
        {
            var res = new RetornoHelper();

            if (ModelState.IsValid)
            {
                if (preco.Id == 0)
                {
                    var lst = await _contexto.Preco
                        .FromSqlRaw("SELECT * FROM Preco WHERE @DataInicial"
                        + " BETWEEN DataInicial and DataFinal and @DataFinal"
                        + " BETWEEN DataInicial and DataFinal",
                        new SqlParameter("DataInicial", preco.DataInicial),
                        new SqlParameter("DataFinal", preco.DataFinal))
                        .ToListAsync();

                    if (lst.Count() >= 1)
                    {
                        res.Mensagem = "Já existe um preço vigente para data selecionada";
                        res.Situacao = false;
                        return res;
                    }
                    else
                    {
                        res.Situacao = true;
                        _contexto.Add(preco);
                    }
                }
                else
                {
                    res.Situacao = true;
                    _contexto.Update(preco);
                }

                await _contexto.SaveChangesAsync();
                return res;
            }

            res.Situacao = false;
            return res;
        }

        public async Task<IActionResult> Delete(int? id)
        {
            var preco = await _contexto.Preco.FindAsync(id);
            _contexto.Preco.Remove(preco);
            await _contexto.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<JsonResult> teste(Preco a)
        {
            var item = new Preco();
            item.Id = 1; 
            item.PrecoHora = 1;
            item.PrecoHoraAdicional = 2;
            item.DataInicial = DateTime.Now;
            item.DataFinal = DateTime.Now.AddDays(1);

            var res = new JsonResult(item);

            return res;
        }

    }
}
