using System.ComponentModel.DataAnnotations;
using Server.Utilities;

namespace Server.Dtos
{
    public record userRegistrationDto
    {
        [Required]
        [EmailAddress]
        public string Email {get; set;}
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Password {get;set;}
        [Required]
        [RoleValidation]
        public string[] Roles {get; set;}
    }
}