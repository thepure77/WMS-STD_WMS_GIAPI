using GIDataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;

namespace DataAccess
{
    public class MasterDbContext : DbContext
    {
        
        public DbSet<View_CheckLocation> View_CheckLocation { get; set; }
        public DbSet<MS_Product> MS_Product { get; set; }
        public DbSet<ms_Printer> ms_Printer { get; set; }
        public DbSet<ms_Location> ms_Location { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder();
                builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"), optional: false);

                var configuration = builder.Build();

                var connectionString = configuration.GetConnectionString("Master_ConnectionString").ToString();

                optionsBuilder.UseSqlServer(connectionString);
            }

          
        }
    }
    
}
