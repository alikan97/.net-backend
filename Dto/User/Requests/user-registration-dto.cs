using System.ComponentModel.DataAnnotations;
namespace Server.Dtos
{
    public record userRegistrationDto
    {
        [Required]
        [EmailAddress]
        public string Email {get; set;}
        [Required]
        public string Password {get;set;}
        public string[] Roles {get; set;}
    }
}