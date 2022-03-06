using System;

namespace Server.Entities
{
    public record User
    {
        public Guid Id {get; set;}
        public string Email {get; set;}
        public string Password {get; set;}
    }
}