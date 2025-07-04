namespace GeoTrip.DTO
{
    public class CreateRecipeDto
    {
        public int? PrepTime { get; set; }
        public int? CookTime { get; set; }
        public int? Servings { get; set; }
        public string? Instructions { get; set; }
        public string? Tips { get; set; }
        public string? DifficultyLevel { get; set; }
        public List<CreateRecipeIngredientDto>? Ingredients { get; set; }
    }
}
