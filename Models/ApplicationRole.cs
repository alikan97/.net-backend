using System;
using System.Collections.Generic;
using AspNetCore.Identity.Mongo.Model;
using AspNetCore.Identity.MongoDbCore.Models;
using Microsoft.AspNetCore.Identity;

namespace Server.Models
{
    public class ApplicationRole : MongoIdentityRole<string>
    {
        public ApplicationRole() : base() { }

        public ApplicationRole(string name) : base(name) { }
    }

    public class ApplicationRole<TKey> : IdentityRole<TKey> where TKey : IEquatable<TKey>
    {
        private List<IdentityRoleClaim<string>> Claims { get; set; }
        private DateTime expiry {get;set;}
        public ApplicationRole()
        {
            Claims = new List<IdentityRoleClaim<string>>();
        }

        public ApplicationRole(string name, DateTime expiration) : this()
        {
            Name = name;
            expiry = expiration;
            NormalizedName = name.ToUpperInvariant();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}