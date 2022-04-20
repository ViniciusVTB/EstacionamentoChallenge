#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Havan.Context;
using Havan.Models;
using Microsoft.Data.SqlClient;
using Havan.Models.Helper;

namespace Havan.Controllers
{
    public class CondutorController : Controller
    {
        private readonly Contexto _context;

        public CondutorController(Contexto context)
        {
            _context = context;
        }

        public IActionResult Create(int Id)
        {
            return View();
        }

        [HttpPost]
        public async Task<RetornoHelper> Create(CondutorHelper condutorHelper)
        {
            var res = new RetornoHelper();

            if (ModelState.IsValid)
            {
                var condutor = new Condutor();
                condutor.Nome = condutorHelper.Nome;
                condutor.Cpf = condutorHelper.Cpf;

                var entrada = await _context.EntradaVeiculo.Where(x => x.Id == condutorHelper.Id).FirstOrDefaultAsync();
                var condutorDb = await _context.Condutor.Where(x=> x.Nome == condutor.Nome && x.Cpf == condutor.Cpf).FirstOrDefaultAsync();

                if(condutorDb == null)
                {
                    _context.Add(condutor);
                    entrada.CondutorId = condutor.Id;
                    entrada.Condutor = condutor;
                    res.Mensagem = "Condutor cadastrado!";
                }
                else
                {
                    entrada.CondutorId = condutorDb.Id;
                    entrada.Condutor = condutorDb;
                    res.Mensagem = "Condutor vinculado!";
                }
                    
                _context.Update(entrada);

                res.Situacao = true;
                res.Data = condutorHelper.Id.ToString();

                await _context.SaveChangesAsync();
                return res;
            }
            res.Situacao = false;
            return res;
        }
    }
}
