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
            modelBuilder.Entity<GalacticPOI>().Property(x => x.Name).
                HasColumnType("TEXT COLLATE NOCASE");
            modelBuilder.Entity<GalacticSystem>().Property(x => x.Name).
                HasColumnType("TEXT COLLATE NOCASE");
            modelBuilder.Entity<GalacticPOI>().HasIndex(x => x.Name);
            modelBuilder.Entity<GalacticSystem>().HasIndex(x => x.Name);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        public DbSet<GalacticSystem> GalacticSystems { get; set; }

        public DbSet<GalacticPOI> GalacticPOIs { get; set; }
    }
}
