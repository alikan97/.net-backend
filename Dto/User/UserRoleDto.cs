using System;
using System.ComponentModel.DataAnnotations;

namespace Server.Dtos
{
    public record UserRoleDto
    {
        [Required]
        public string roleName {get; set;}
        
        [Required]
        [EmailAddress]
        public string Email {get; set;}
    }
}