using GeoTrip.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
namespace GeoTrip.Data

{
    public class GeoVoyageDbContext : DbContext
    {
        public GeoVoyageDbContext(DbContextOptions<GeoVoyageDbContext> options) : base(options)
        {
        }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Place> Places { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dish>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(10,2)")
                    .IsRequired();

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(e => e.Description)
                    .HasMaxLength(500);

                entity.Property(e => e.Category)
                    .HasMaxLength(50);

                entity.Property(e => e.ImageUrl)
                  .HasMaxLength(8000);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(t => t.Id);

                entity.Property(t => t.Id).ValueGeneratedOnAdd();
                entity.Property(t => t.Name).HasMaxLength(100).IsRequired();
                entity.Property(t => t.Description).HasMaxLength(1000).IsRequired();
                entity.Property(t => t.Image).HasMaxLength(8000);
            });

            modelBuilder.Entity<Place>(entity =>
            {
                entity.HasKey(f => f.Id);

                entity.Property(f => f.Id).ValueGeneratedOnAdd();
                entity.Property(f => f.Name).HasMaxLength(200).IsRequired();
                entity.Property(f => f.Description).HasMaxLength(5000).IsRequired();
                entity.Property(f => f.ImageUrl).HasMaxLength(8000);
            });
        }
    }
}
