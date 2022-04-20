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
using Microsoft.Data.SqlClient;

namespace HavanAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrecoController : ControllerBase
    {
        private readonly Contexto _context;

        public PrecoController(Contexto context)
        {
            _context = context;
        }

        // GET: api/Preco
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Preco>>> GetPreco()
        {
            return await _context.Preco.ToListAsync();
        }

        // GET: api/Preco/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Preco>> GetPreco(int id)
        {
            var preco = await _context.Preco.FindAsync(id);

            if (preco == null)
            {
                return NotFound();
            }

            return preco;
        }

        // PUT: api/Preco/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPreco(int id, Preco preco)
        {
            if (id != preco.Id)
            {
                return BadRequest();
            }

            _context.Entry(preco).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PrecoExists(id))
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

        // POST: api/Preco
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Preco>> PostPreco(Preco preco)
        {
            var lst = _context.Preco
                       .FromSqlRaw("SELECT * FROM Preco WHERE @DataInicial"
                       + " BETWEEN DataInicial and DataFinal and @DataFinal"
                       + " BETWEEN DataInicial and DataFinal",
                       new SqlParameter("DataInicial", preco.DataInicial),
                       new SqlParameter("DataFinal", preco.DataFinal))
                       .ToList();

            if (lst.Count() >= 1)
            {
                return Ok("Já existe um preço vigente para data selecionada");
            }

            _context.Preco.Add(preco);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPreco", new { id = preco.Id }, preco);
        }

        // DELETE: api/Preco/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePreco(int id)
        {
            var preco = await _context.Preco.FindAsync(id);
            if (preco == null)
            {
                return NotFound();
            }

            _context.Preco.Remove(preco);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PrecoExists(int id)
        {
            return _context.Preco.Any(e => e.Id == id);
        }
    }
}
