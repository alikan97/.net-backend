using System;
using System.ComponentModel.DataAnnotations;

namespace Server.Dtos
{
    public record CreateRoleDto
    {
        [Required]
        public string roleName {get; set;}
        public DateTime expires {get;set;}
    }
}