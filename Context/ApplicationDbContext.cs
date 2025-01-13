using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using MonitorJudicial.Models;
using System.Data.Entity.Infrastructure;

namespace MonitorJudicial.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("SQLConnectionString") { }

        public DbSet<EstadoPrestamosConPorcentaje> EstadoPrestamos { get; set; }
    }

    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("name=SQLConnectionString")
        {
        }

        public DbSet<Prestamo> Prestamos { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Prestamo>().ToTable("Prestamos");
            base.OnModelCreating(modelBuilder);
        }

        public virtual DbRawSqlQuery<Prestamo> GetPrestamos()
        {
            return this.Database.SqlQuery<Prestamo>("EXEC [FBS_REPORTES].[CONSULTARPRESTAMOS]");
        }
    }
}