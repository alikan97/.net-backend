using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Server.Dtos;
using Server.Entities;

public class JwtHelper
{
    public static generatedJwtToken GetJwtToken(
        UserCollection subjectUser,
        string issuer,
        string audience,
        string uniqueKey
    )
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenExpiry = DateTime.UtcNow.AddMinutes(30);
        var refreshExpiry = DateTime.UtcNow.AddMonths(1);

        var additionalClaims = new List<Claim>();
        // Add roles as multiple claims
        if (subjectUser.Roles != null)
        {
            foreach (var role in subjectUser.Roles)
            {
                additionalClaims.Add(new Claim(ClaimTypes.Role, role));
            }
        }

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, subjectUser.Email),
            new Claim(JwtRegisteredClaimNames.Name, subjectUser.FullName),
            new Claim(JwtRegisteredClaimNames.Sub, subjectUser.Id.ToString())
        };
        foreach (Claim claim in additionalClaims)
        {
            claims.Add(claim);
        }
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = issuer,
            Audience = audience,
            IssuedAt = DateTime.UtcNow,
            NotBefore = DateTime.UtcNow.AddSeconds(5),
            Expires = tokenExpiry,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(uniqueKey)),
                        SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);
        
        var refreshToken = new refreshToken()
        {
            jwtAccessId = token.Id,
            usageCount = 0,
            isRevoked = false,
            userId = subjectUser.Id,
            AddedDate = DateTime.UtcNow,
            Expiry = refreshExpiry,
            token = Randomstring(35) + Guid.NewGuid(),
        };

        return new generatedJwtToken
        {
            AccessToken = jwtToken,
            RefreshToken = refreshToken,
            expires = refreshExpiry
        };
    }

    private static string Randomstring(int length)
    {
        var random = new Random();
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length).Select(x => x[random.Next(x.Length)]).ToArray());
    }
}