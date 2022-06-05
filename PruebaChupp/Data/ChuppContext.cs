using Microsoft.EntityFrameworkCore;
using PruebaChupp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaChupp.Data
{
    public class ChuppContext : DbContext
    {
        public DbSet<Personas> personas { get; set; }
        public DbSet<Seguros> seguros { get; set; }
        public DbSet<TipoSeguros> tipoSeguros { get; set; }
        public DbSet<Ventas> ventas { get; set; }
        public DbSet<Rangos> rangos { get; set; }
        public DbSet<Porcentajes> porcentajes { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server=SMARTSERVER;Database=pruebachupp;Integrated Security=True;");
        }
    }
}
