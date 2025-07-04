using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GeoTrip.Models
{
    public class Recipe
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int DishId { get; set; }

        public int? PrepTime { get; set; } 

        public int? CookTime { get; set; }

        public int? Servings { get; set; }

        [Column(TypeName = "NVARCHAR(MAX)")]
        public string? Instructions { get; set; }

        [MaxLength(500)]
        public string? Tips { get; set; }

        [MaxLength(20)]
        public string? DifficultyLevel { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("DishId")]
        public virtual Dish Dish { get; set; }

        public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
    }
}
