using System;
using System.Collections.Generic;

namespace Server.Dtos
{
    public record filteredUser
    {
        public string Email {get; set;}
        public Guid Id {get; set;}
        public List<string> Roles {get; set;}
        public string Info {get; set;}
    }
}