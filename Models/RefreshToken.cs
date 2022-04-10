using System;
using System.ComponentModel.DataAnnotations.Schema;
using Server.Models;

namespace App.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Token { get; set; }
        public string JwtId { get; set; }
        public bool IsUsed { get; set; }
        public bool isRevoked { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime ExpiresIn { get; set; }
        [ForeignKey(nameof(UserId))]
        public ApplicationUser user { get; set; }
    }
}