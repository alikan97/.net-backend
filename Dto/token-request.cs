using System.ComponentModel.DataAnnotations;

namespace Server.Dtos
{
    public record TokenRequest
    {
        [Required]
        public string AccessToken { get; set; }
        [Required]
        public string RefreshToken { get; set; }
    }
}