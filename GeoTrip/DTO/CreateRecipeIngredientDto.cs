using System.ComponentModel.DataAnnotations;

namespace GeoTrip.DTO
{
    public class CreateRecipeIngredientDto
    {
        [Required]
        public string IngredientName { get; set; }
        public string? Quantity { get; set; }
        public string? Unit { get; set; }
        public int OrderIndex { get; set; } = 1;
    }
}
