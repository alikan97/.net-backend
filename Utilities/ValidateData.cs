using System;
using System.ComponentModel.DataAnnotations;

namespace Server.Utilities
{
    public class EmailValidation : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return new RegularExpressionAttribute(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").IsValid(Convert.ToString(value).Trim());
        }
    }
}