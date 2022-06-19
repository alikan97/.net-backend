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
            string[] arr = value as string[];
            foreach (string i in arr)
            {
                if (!Enum.IsDefined(typeof(serverRoles), i))
                {
                    return false;
                }
            }
            return true;
        }
    }

    public class RoleValidationSingle : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string role = value as string;
            if (!Enum.IsDefined(typeof(serverRoles), role))
            {
                return false;
            }
            return true;
        }
    }
}