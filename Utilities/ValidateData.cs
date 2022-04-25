using System;
using System.ComponentModel.DataAnnotations;
using Server.Entities;
namespace Server.Utilities
{
    public class EmailValidation : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return new RegularExpressionAttribute(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").IsValid(Convert.ToString(value).Trim());
        }
    }

    public class RoleValidation : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return Enum.IsDefined(typeof(serverRoles), value);
        }
    }
}