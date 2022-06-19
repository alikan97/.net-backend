using System.ComponentModel.DataAnnotations;
using Server.Utilities;

namespace Server.Dtos
{
    public class addRoleToUser
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RoleValidationSingle]
        public string Role { get; set; }
    }
}