using System.ComponentModel.DataAnnotations;

namespace GeoTrip.DTO
{
    public class UpdateDishDto
    {
        [MaxLength(100)]
        public string? Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [MaxLength(255)]
        public string? ImageUrl { get; set; }

        [MaxLength(50)]
        public string? Category { get; set; }

        [MaxLength(20)]
        public string? Difficulty { get; set; }

        [Range(0, 5)]
        public decimal? Rating { get; set; }
    }
}
