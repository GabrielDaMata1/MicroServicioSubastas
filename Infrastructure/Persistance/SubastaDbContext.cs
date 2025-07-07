using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Models.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance
{
    /// <summary>
    /// Clase persistance que representa el contexto de base de datos en PostgreSQL del Microservicio Subasta.
    /// </summary>
    public class SubastaDbContext : DbContext
    {
        public SubastaDbContext(DbContextOptions<SubastaDbContext> options) : base(options) { }

        /// <summary>
        /// Atributo que corresponde a la tabla Subasta en la base de datos PostgreSQL.
        /// </summary>
        public DbSet<SubastaPostgreSQL> Subasta { get; set; }
        /// <summary>
        /// Atributo que corresponde a la tabla HistorialSubasta en la base de datos PostgreSQL.
        /// </summary>
        public DbSet<HistorialSubastasPostgreSQL> HistorialSubasta { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SubastaPostgreSQL>()
                .HasIndex(u => u.Id)
                .IsUnique();

            base.OnModelCreating(modelBuilder);

        }
    }
}
