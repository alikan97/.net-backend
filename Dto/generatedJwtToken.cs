using System;

namespace Server.Dtos
{
    public record generatedJwtToken
    {
        public string AccessToken {get; set;}
        public refreshToken RefreshToken {get; set;}
        public DateTime expires {get; set;}
    }
}