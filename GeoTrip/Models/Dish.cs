using System.ComponentModel.DataAnnotations;

namespace GeoTrip.Models
{
    public class Dish
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string ImageUrl { get; set; }

        [MaxLength(20)]
        public string? Difficulty { get; set; }
        public decimal? Rating { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public virtual Recipe? Recipe { get; set; }
    }
}
