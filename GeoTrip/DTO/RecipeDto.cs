namespace GeoTrip.DTO
{
    public class RecipeDto
    {
        public int Id { get; set; }
        public int DishId { get; set; }
        public int? PrepTime { get; set; }
        public int? CookTime { get; set; }
        public int? Servings { get; set; }
        public string? Instructions { get; set; }
        public string? Tips { get; set; }
        public string? DifficultyLevel { get; set; }
        public List<RecipeIngredientDto> Ingredients { get; set; } = new List<RecipeIngredientDto>();
    }
}
