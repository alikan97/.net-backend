using System.ComponentModel.DataAnnotations;
namespace Server.Dtos 
{
    public record updateItemDto // Records are good to use for DTOs
    {
        [Required]
        public string Name { get; init; }
        [Required]
        [Range(0,5000)]
        public decimal Price { get; init; }
    }
}