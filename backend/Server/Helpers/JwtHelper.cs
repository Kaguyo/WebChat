using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace Server.Helpers
{
    public static class JwtHelper
    {
        public static string GenerateToken(string username, string secret){
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]{
                new Claim(ClaimTypes.Name, username),
            };

            var token = new JwtSecurityToken(
                claims : claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );
    
            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}