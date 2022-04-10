using System;
using System.Collections.Generic;
using AspNetCore.Identity.Mongo.Model;
using AspNetCore.Identity.MongoDbCore.Models;
using Microsoft.AspNetCore.Identity;

namespace Server.Models
{
    public class ApplicationUser : MongoIdentityUser<string>
    {
        public ApplicationUser() : base() { }
        public ApplicationUser(string userName) : base(userName) { }
    }

    public class ApplicationUser<TKey> : IdentityUser<TKey> where TKey : IEquatable<TKey>
    {
        public List<string> Roles { get; set; }
        public List<IdentityUserClaim<string>> Claims { get; set; }
        public List<IdentityUserLogin<string>> Login { get; set; }
        public List<IdentityUserToken<string>> Token { get; set; }
        public ApplicationUser()
        {
            Roles = new List<string>();
            Claims = new List<IdentityUserClaim<string>>();
            Login = new List<IdentityUserLogin<string>>();
            Token = new List<IdentityUserToken<string>>();
        }

        public ApplicationUser(string userName) : this()
        {
            UserName = userName;
            NormalizedUserName = userName.ToUpperInvariant();
        }
    }
}