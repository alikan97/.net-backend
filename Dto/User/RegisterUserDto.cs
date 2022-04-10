using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Server.Dtos
{
    public record UserRegistrationDto
    {
        [Required]
        [EmailAddress]
        public string Email {get; set;}
        [Required]
        public string Password {get;set;}
        [Required]
        public string UserName {get;set;}
    }
}