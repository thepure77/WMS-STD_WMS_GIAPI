using GIDataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;

namespace DataAccess
{
    public class BinbalanceDbContext : DbContext
    {
        
        public DbSet<wm_BinBalance> wm_BinBalance { get; set; }
        public DbSet<wm_BinCard> wm_BinCard { get; set; }
        public DbSet<wm_BinCardReserve> wm_BinCardReserve { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder();
                builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"), optional: false);

                var configuration = builder.Build();

                var connectionString = configuration.GetConnectionString("Binbalance_Connection").ToString();

                optionsBuilder.UseSqlServer(connectionString);
            }
        }
    }
}
