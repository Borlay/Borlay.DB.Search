using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Borlay.DB.Search.Tests
{
    public class TestContext : DbContext
    {
        public DbSet<VartotojaiTable> Vartotojai { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = Environment.GetEnvironmentVariable("ConnectionString");

            if (connectionString == null)
                throw new ArgumentNullException(nameof(connectionString));

            connectionString = connectionString.Replace("[Application Name]", "Borlay.DB.Search.Tests");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
