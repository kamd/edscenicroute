using System;
using System.Collections.Generic;
using System.Text;
using EDScenicRouteCoreModels;
using Microsoft.EntityFrameworkCore;

namespace EDScenicRouteCore.Data
{
    public class GalacticSystemContext : DbContext
    {
        public GalacticSystemContext(DbContextOptions<GalacticSystemContext> options)
            : base(options)
        {
            
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GalacticSystem>()
                .Ignore(s => s.GalMapUrl);
            modelBuilder.Entity<GalacticPOI>().OwnsOne(x => x.Coordinates);
            modelBuilder.Entity<GalacticSystem>().OwnsOne(x => x.Coordinates);
            modelBuilder.Entity<GalacticPOI>().Property(x => x.Name)
                .HasColumnType("VARCHAR(200) UNIQUE")
                .HasMaxLength(200);
            modelBuilder.Entity<GalacticPOI>().Property(x => x.GalMapUrl)
                .HasMaxLength(1000);
            modelBuilder.Entity<GalacticPOI>().Property(x => x.GalMapSearch)
                .HasMaxLength(600);
            modelBuilder.Entity<GalacticSystem>().Property(x => x.Name)
                .HasColumnType("VARCHAR(200) UNIQUE")
                .HasMaxLength(200);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        public DbSet<GalacticSystem> GalacticSystems { get; set; }

        public DbSet<GalacticPOI> GalacticPOIs { get; set; }
    }
}
