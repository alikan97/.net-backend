using Server.Dtos;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Server.Config;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System;

namespace Server.Controllers
{
    public class UserService
    {
        private readonly IMongoCollection<UserDto> users;
        private readonly JwtConfig jwtSettings;

        public UserService(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("mongoDb"));
            var database = client.GetDatabase("server");
            users = database.GetCollection<UserDto>("users");
            jwtSettings = config.GetSection(nameof(JwtConfig)).Get<JwtConfig>();
        }

        public async Task<List<UserDto>> GetUsers() => await users.Find(user => true).ToListAsync();

        public async Task<UserDto> GetUser(string id) => await users.Find<UserDto>(user => user.Id == id).FirstOrDefaultAsync();

        public async Task<UserDto> Create(UserDto user)
        {
            await users.InsertOneAsync(user);
            return user;
        }

        public async Task<Object> Authenticate(UserDto tempUser)
        {
            var user = await this.users.Find(x => x.Email == tempUser.Email && x.Password == tempUser.Password).FirstOrDefaultAsync();
            if (user == null) return null;

            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Email, tempUser.Email));

            // Add roles as multiple claims
            foreach (var role in tempUser.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
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
                expires = token.ValidTo
            };

            // var tokenHandler = new JwtSecurityTokenHandler();
            // var tokenKey = System.Text.Encoding.ASCII.GetBytes(jwtSettings.Secret.ToString());

            // var claims = new List<Claim>();
            // claims.Add(new Claim(ClaimTypes.Email, tempUser.Email));

            // foreach (var role in tempUser.Roles)
            // {
            //     claims.Add(new Claim(ClaimTypes.Role, role));
            // }

            // var tokenDescriptor = new SecurityTokenDescriptor() {
            //     Subject = new ClaimsIdentity(claims),
            //     Expires = DateTime.UtcNow.AddHours(1),
            //     SigningCredentials = new SigningCredentials (
            //         new SymmetricSecurityKey(tokenKey),
            //         SecurityAlgorithms.HmacSha256Signature
            //     ),
            //     Issuer = jwtSettings.Issuer.ToString(),
            //     Audience = jwtSettings.Audience.ToString()
            // };

            // var token = tokenHandler.CreateToken(tokenDescriptor);
            // return tokenHandler.WriteToken(token);
        }
    }
}
