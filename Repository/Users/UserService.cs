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
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using AspNetCore.Identity.Mongo.Mongo;

namespace Server.Repositories
{
    public class UserService
    {
        private readonly IMongoCollection<UserCollection> users;
        private readonly UpdateDefinitionBuilder<UserCollection> userUpdate = Builders<UserCollection>.Update;
        private readonly JwtConfig jwtSettings;
        private readonly TokenValidationParameters _tokenValidationParameters;
        public UserService(IConfiguration config, TokenValidationParameters tokVal)
        {
            var client = new MongoClient(config.GetConnectionString("mongoDb"));
            var database = client.GetDatabase("server");
            users = database.GetCollection<UserCollection>("users");
            jwtSettings = config.GetSection(nameof(JwtConfig)).Get<JwtConfig>();
            _tokenValidationParameters = tokVal;
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
                FullName = user.FullName,
                RefreshToken = null,
                Email = user.Email,
                Password = user.Password,
                Roles = roles
            };

            await users.InsertOneAsync(newUser);
            return newUser;
        }

        public async Task<Object> Authenticate(UserLoginDto subjectUser)
        {
            var user = await this.users.Find(x => x.Email == subjectUser.Email && x.Password == subjectUser.Password).FirstOrDefaultAsync();
            if (user == null) return null;

            var authenticated = JwtHelper.GetJwtToken(
                user,
                jwtSettings.Issuer,
                jwtSettings.Audience,
                jwtSettings.Secret.ToString()
            );

            // Update the refresh token field in DB
            var filter = userUpdate.Set(x => x.RefreshToken, authenticated.RefreshToken);
            await users.UpdateOneAsync(x => x.Id == user.Id, filter);

            return new
            {
                token = authenticated.AccessToken,
                refreshToken = authenticated.RefreshToken.token,
                expires = authenticated.expires
            };
        }

        public async Task<AuthResponse> VerifyToken(TokenRequest tokenRequest)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                // Validate against the validation parameters in startup.cs (Issuer, Audience, Expiry & Algorithm check)
                var tokenVerification = tokenHandler.ValidateToken(tokenRequest.AccessToken, _tokenValidationParameters, out var tokeValidated);

                var user = await users.Find(x => x.RefreshToken.token == tokenRequest.RefreshToken).FirstOrDefaultAsync(); // This thing fucks up on second use of refresh token

                if (user == null) {
                    return new AuthResponse() {
                        Token = null,
                        ErrorContent = "Cannot match refresh tokens, refresh token is already used",
                        Success = false,
                        statusCode = 401,
                    };
                }
                
                var storedToken = user.RefreshToken;

                if (storedToken == null)
                {
                    return new AuthResponse()
                    {
                        Token = null,
                        ErrorContent = "Refresh token does not exist",
                        Success = false,
                        statusCode = 401
                    };
                }

                if (storedToken.usageCount >= 5)
                {
                    return new AuthResponse()
                    {
                        Token = null,
                        ErrorContent = "Token has exceeded its maximum usage",
                        Success = false,
                        statusCode = 401
                    };
                }

                if (storedToken.isRevoked)
                {
                    return new AuthResponse()
                    {
                        Token = null,
                        ErrorContent = "Token has been revoked!",
                        Success = false,
                        statusCode = 403
                    };
                }

                var jti = tokenVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

                if (storedToken.jwtAccessId != jti)
                {
                    return new AuthResponse()
                    {
                        Token = null,
                        ErrorContent = "Token JTI does not match",
                        Success = false,
                        statusCode = 403
                    };
                }

                var authenticated = JwtHelper.GetJwtToken(
                    user,
                    jwtSettings.Issuer,
                    jwtSettings.Audience,
                    jwtSettings.Secret.ToString()
                );

                storedToken.usageCount++;
                storedToken.token = authenticated.RefreshToken.token;
                Console.WriteLine(storedToken.jwtAccessId);
                Console.WriteLine(authenticated.RefreshToken.jwtAccessId);
                storedToken.jwtAccessId = authenticated.RefreshToken.jwtAccessId;

                var updateDef = userUpdate.Set(x => x.RefreshToken, storedToken);
                await users.UpdateOneAsync(x => x.RefreshToken.userId == storedToken.userId, updateDef);

                var returnToken = new
                {
                    accessToken = authenticated.AccessToken,
                    refreshToken = authenticated.RefreshToken.token,
                    expires = authenticated.expires
                };

                return new AuthResponse()
                {
                    Token = returnToken,
                    ErrorContent = null,
                    Success = true,
                    statusCode = 200
                };
            }
            catch (Exception error)
            {
                return new AuthResponse() {
                    Token = null,
                    ErrorContent = error.Message.ToString(),
                    Success = false,
                    statusCode = 403
                };
            }
        }

        public async Task AddRoleToUser(UserCollection existingUser, string Role)
        {
            existingUser.Roles.Add(Role);
            await Task.CompletedTask;
        }

        public async Task Test()
        {
            var newPass = "aaa";
            var id = Guid.Parse("1e556c32-6853-4733-8f6f-92f3e8537f56");
            var updateDef = userUpdate.Set(x => x.Password, newPass);

            await users.UpdateOneAsync(x => x.Id == id, updateDef);
        }
    }
}
