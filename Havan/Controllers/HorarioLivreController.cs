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

namespace Havan.Controllers
{
    public class HorarioLivreController : Controller
    {
        private readonly Contexto _context;

        public HorarioLivreController(Contexto context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.HorarioLivre.ToListAsync());
        }

        public IActionResult AddOrEdit(int id = 0)
        {
            var diaSemana = Enum.GetNames(typeof(DayOfWeek));
            ViewBag.Dias = diaSemana.ToList().Select(x => new SelectListItem() { Text = x, Value = x });

            if (id == 0)
                return View(new HorarioLivre());
            else
                return View(_context.HorarioLivre.Find(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("Id,Dia,HoraInicial,HoraFinal")] HorarioLivre horarioLivre)
        {
            if (ModelState.IsValid)
            {
                if (horarioLivre.Id == 0)
                    _context.Add(horarioLivre);
                else
                    _context.Update(horarioLivre);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(horarioLivre);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            var horarioLivre = await _context.HorarioLivre.FindAsync(id);
            _context.HorarioLivre.Remove(horarioLivre);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
