using Server.Dtos;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Server.Config;
using System.Security.Claims;
using System;
using Server.Entities;

namespace Server.Repositories
{
    public class UserService
    {
        private readonly IMongoCollection<UserCollection> users;
        private readonly JwtConfig jwtSettings;

        public UserService(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("mongoDb"));
            var database = client.GetDatabase("server");
            users = database.GetCollection<UserCollection>("users");
            jwtSettings = config.GetSection(nameof(JwtConfig)).Get<JwtConfig>();
        }

        public async Task<List<UserCollection>> GetUsers() => await users.Find(user => true).ToListAsync();

        public async Task<UserCollection> GetUser(string email) => await users.Find<UserCollection>(user => user.Email == email).FirstOrDefaultAsync();

        public async Task<UserCollection> Create(userRegistrationDto user)
        {
            var id = new Guid();
            var roles = new List<string>();

            foreach (var role in user.Roles)
            {
                roles.Add(role);
            }

            var newUser = new UserCollection
            {
                Id = id,
                Email = user.Email,
                Password = user.Password,
                Roles = roles
            };

            await users.InsertOneAsync(newUser);
            return newUser;
        }

        public async Task<Object> Authenticate(UserLoginDto tempUser)
        {
            var user = await this.users.Find(x => x.Email == tempUser.Email && x.Password == tempUser.Password).FirstOrDefaultAsync();
            if (user == null) return null;

            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Email, tempUser.Email));

            // Add roles as multiple claims
            if (user.Roles != null)
            {
                foreach (var role in user.Roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            var token = JwtHelper.GetJwtToken(
                tempUser.Email,
                jwtSettings.Issuer,
                jwtSettings.Audience,
                TimeSpan.FromMinutes(60),
                jwtSettings.Secret.ToString(),
                claims.ToArray());

            return new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expires = token.ValidTo.ToLocalTime()
            };
        }

        public async Task AddRoleToUser (UserCollection existingUser, string Role)
        {
            existingUser.Roles.Add(Role);
            await Task.CompletedTask;
        }
    }
}
