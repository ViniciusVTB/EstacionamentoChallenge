using Havan.Models;
using Microsoft.EntityFrameworkCore;

namespace Havan.Context
{
    public class Contexto : DbContext
    {
        public Contexto(DbContextOptions<Contexto> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Preco> Preco { get; set; }
        public DbSet<HorarioLivre> HorarioLivre { get; set; }
        public DbSet<Condutor> Condutor { get; set; }
        public DbSet<EntradaVeiculo> EntradaVeiculo { get; set; }
        
    }
}
