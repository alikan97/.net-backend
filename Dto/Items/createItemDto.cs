using System.ComponentModel.DataAnnotations;
namespace Server.Dtos 
{
    public record CreateItemDto
    {
        [Required]
        public string Name { get; init; }
        [Required]
        [Range(0,5000)]
        public decimal Price { get; init; }
        [Required]
        public string Category { get; set; }
    }
}