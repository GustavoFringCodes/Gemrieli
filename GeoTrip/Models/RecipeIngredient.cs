using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GeoTrip.Models
{
    public class RecipeIngredient
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RecipeId { get; set; }

        [Required]
        [MaxLength(100)]
        public string IngredientName { get; set; }

        [MaxLength(50)]
        public string? Quantity { get; set; }

        [MaxLength(30)]
        public string? Unit { get; set; } 

        public int OrderIndex { get; set; } = 1;

        [ForeignKey("RecipeId")]
        public virtual Recipe Recipe { get; set; }
    }
}
