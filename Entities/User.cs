using System;
using System.Collections.Generic;
using Server.Dtos;

namespace Server.Entities
{
    public record UserCollection
    {
        public Guid Id {get; init;}
        public string Email {get; set;}
        public string Password {get; set;}
        public string FullName {get; set;}
        public refreshToken RefreshToken {get; set;}
        public List<string> Roles { get; set; }
    }
}