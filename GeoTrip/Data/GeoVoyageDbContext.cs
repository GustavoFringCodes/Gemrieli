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
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<RecipeIngredient> RecipeIngredients { get; set; }

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

            modelBuilder.Entity<Recipe>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Instructions).HasColumnType("NVARCHAR(MAX)");
                entity.Property(e => e.Tips).HasMaxLength(500);
                entity.Property(e => e.DifficultyLevel).HasMaxLength(20);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");

                entity.HasOne(e => e.Dish)
                      .WithOne(d => d.Recipe)
                      .HasForeignKey<Recipe>(e => e.DishId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<RecipeIngredient>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.IngredientName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Quantity).HasMaxLength(50);
                entity.Property(e => e.Unit).HasMaxLength(30);

                entity.HasOne(e => e.Recipe)
                      .WithMany(r => r.RecipeIngredients)
                      .HasForeignKey(e => e.RecipeId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
