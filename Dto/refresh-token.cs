using System;

namespace Server.Dtos
{
    public class refreshToken
    {
        public int id { get; set; }
        public Guid userId { get; set; }
        public string token { get; set; }
        public string jwtAccessId { get; set; }
        public int usageCount { get; set; }
        public bool isRevoked { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime Expiry { get; set; }
    }
}