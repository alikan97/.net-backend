using System.ComponentModel.DataAnnotations;
using Server.Utilities;
namespace Server.Dtos
{
        public record UserDto
    {
        [Required]
        [EmailValidation] //Custom validation bitch
        public string Email { get; init; }
        [Required]
        public string Password { get; init; }
    }
}