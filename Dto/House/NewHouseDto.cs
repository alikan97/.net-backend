using System.ComponentModel.DataAnnotations;
namespace Server.Dtos
{
    public record createHouseDto
    {
        [Required]
        public decimal Temperature {get; set;}
        [Required]
        public decimal Humidity {get;set;}
        [Required]
        public string Name {get;set;}
        public string[] Occupants {get; set;}
    }
}