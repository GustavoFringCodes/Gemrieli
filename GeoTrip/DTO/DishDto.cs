namespace GeoTrip.DTO
{
    public class DishDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string? Category { get; set; }
        public string? Difficulty { get; set; }
        public decimal? Rating { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool HasRecipe { get; set; }
    }
}
