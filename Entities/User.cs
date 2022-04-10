using System;
using System.Collections.Generic;

namespace Server.Entities
{
    public record UserCollection
    {
        public Guid Id {get; set;}
        public string Email {get; set;}
        public string Password {get; set;}
        public List<string> Roles { get; set; }
    }
}