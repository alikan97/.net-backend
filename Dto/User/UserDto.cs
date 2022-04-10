using System.ComponentModel.DataAnnotations;

namespace Server.Dtos
{
    public record UserDto
    {
        [Required]
        [EmailAddress]
        public string Email {get; set;}
        [Required]
        public string Password {get;set;}
    }
}