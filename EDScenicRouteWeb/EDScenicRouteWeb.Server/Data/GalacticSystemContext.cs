using Microsoft.EntityFrameworkCore;
using GalacticPOI = EDScenicRouteWeb.Server.Models.GalacticPOI;
using GalacticSystem = EDScenicRouteWeb.Server.Models.GalacticSystem;

namespace EDScenicRouteWeb.Server.Data
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
            modelBuilder.Entity<GalacticPOI>(e =>
            {
                e.Property(x => x.Name)
                    .HasColumnType("VARCHAR(200)")
                    .HasMaxLength(200);

                e.Property(x => x.GalMapUrl).HasMaxLength(1000);
                e.Property(x => x.GalMapSearch).HasMaxLength(600);
                e.Property(x => x.X).HasColumnName("Coordinates_X");
                e.Property(x => x.Y).HasColumnName("Coordinates_Y");
                e.Property(x => x.Z).HasColumnName("Coordinates_Z");
                e.Ignore(x => x.Coordinates);
                e.Property(x => x.Body).HasMaxLength(150);
            });
            modelBuilder.Entity<GalacticSystem>(e =>
            {
                e.Ignore(s => s.GalMapUrl);
                e.Property(x => x.Name)
                    .HasColumnType("VARCHAR(200) UNIQUE")
                    .HasMaxLength(200);
                e.Property(x => x.X).HasColumnName("Coordinates_X");
                e.Property(x => x.Y).HasColumnName("Coordinates_Y");
                e.Property(x => x.Z).HasColumnName("Coordinates_Z");
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                
            }
        }

        public DbSet<GalacticSystem> GalacticSystems { get; set; }

        public DbSet<GalacticPOI> GalacticPOIs { get; set; }
    }
}
