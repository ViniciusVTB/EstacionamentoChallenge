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
    public class EntradaVeiculoController : ControllerBase
    {
        private readonly Contexto _context;

        public EntradaVeiculoController(Contexto context)
        {
            _context = context;
        }

        // GET: api/EntradaVeiculo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EntradaVeiculo>>> GetEntradaVeiculo()
        {
            return await _context.EntradaVeiculo.ToListAsync();
        }

        // GET: api/EntradaVeiculo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EntradaVeiculo>> GetEntradaVeiculo(int id)
        {
            var entradaVeiculo = await _context.EntradaVeiculo.FindAsync(id);

            if (entradaVeiculo == null)
            {
                return NotFound();
            }

            return entradaVeiculo;
        }

        // PUT: api/EntradaVeiculo/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEntradaVeiculo(int id, EntradaVeiculo entradaVeiculo)
        {
            if (id != entradaVeiculo.Id)
            {
                return BadRequest();
            }

            _context.Entry(entradaVeiculo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EntradaVeiculoExists(id))
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

        // POST: api/EntradaVeiculo
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EntradaVeiculo>> PostEntradaVeiculo(EntradaVeiculo entradaVeiculo)
        {
            _context.EntradaVeiculo.Add(entradaVeiculo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEntradaVeiculo", new { id = entradaVeiculo.Id }, entradaVeiculo);
        }

        private bool EntradaVeiculoExists(int id)
        {
            return _context.EntradaVeiculo.Any(e => e.Id == id);
        }
    }
}
