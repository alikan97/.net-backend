using System.ComponentModel.DataAnnotations;

namespace Server.Dtos
{
        public record UserDto
    {
        [Required]
        public string Email { get; init; }
        [Required]
        public string Password { get; init; }
    }
}