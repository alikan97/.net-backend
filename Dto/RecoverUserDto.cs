using System;

namespace Server.Dtos
{
    public record recoverUserDto
    {
        // Use Record types for immutable objects
        public string Email {get; set;}
    }
}