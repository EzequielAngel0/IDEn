using Microsoft.EntityFrameworkCore;
using IDEn.Core.Models;

namespace IDEn.Infrastructure.Data
{
    public class IdenDbContext : DbContext
    {
        public DbSet<ProduccionMensual> Producciones { get; set; }
        public DbSet<ConsumoEnergia> Consumos { get; set; }
        public DbSet<IdenMensual> IdenesMensuales { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=iden.db");

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<ProduccionMensual>().HasKey(x => new { x.Anio, x.Mes });
            mb.Entity<ConsumoEnergia>().HasKey(x => new { x.Anio, x.Mes });
            mb.Entity<IdenMensual>().HasKey(x => new { x.Anio, x.Mes });
        }
    }
}
