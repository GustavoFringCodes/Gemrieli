namespace GeoTrip.DTO
{
    public class RecipeIngredientDto
    {
        public int Id { get; set; }
        public string IngredientName { get; set; }
        public string? Quantity { get; set; }
        public string? Unit { get; set; }
        public int OrderIndex { get; set; }
    }
}
