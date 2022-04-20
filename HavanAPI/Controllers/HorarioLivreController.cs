#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Havan.Context;
using Havan.Models;

namespace HavanAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HorarioLivreController : ControllerBase
    {
        private readonly Contexto _context;

        public HorarioLivreController(Contexto context)
        {
            _context = context;
        }

        // GET: api/HorarioLivre
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HorarioLivre>>> GetHorarioLivre()
        {
            return await _context.HorarioLivre.ToListAsync();
        }

        // GET: api/HorarioLivre/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HorarioLivre>> GetHorarioLivre(int id)
        {
            var horarioLivre = await _context.HorarioLivre.FindAsync(id);

            if (horarioLivre == null)
            {
                return NotFound();
            }

            return horarioLivre;
        }

        // PUT: api/HorarioLivre/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHorarioLivre(int id, HorarioLivre horarioLivre)
        {
            if (id != horarioLivre.Id)
            {
                return BadRequest();
            }

            _context.Entry(horarioLivre).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HorarioLivreExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/HorarioLivre
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<HorarioLivre>> PostHorarioLivre(HorarioLivre horarioLivre)
        {
            _context.HorarioLivre.Add(horarioLivre);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHorarioLivre", new { id = horarioLivre.Id }, horarioLivre);
        }

        // DELETE: api/HorarioLivre/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHorarioLivre(int id)
        {
            var horarioLivre = await _context.HorarioLivre.FindAsync(id);
            if (horarioLivre == null)
            {
                return NotFound();
            }

            _context.HorarioLivre.Remove(horarioLivre);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HorarioLivreExists(int id)
        {
            return _context.HorarioLivre.Any(e => e.Id == id);
        }
    }
}
