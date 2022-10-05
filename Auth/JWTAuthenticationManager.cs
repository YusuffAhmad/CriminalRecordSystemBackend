using Microsoft.IdentityModel.Tokens;
using PrivateEye.DTOs;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace PrivateEye.Auth
{
    public class JWTAuthenticationManager : IJWTAuthenticationManager
    {
        private readonly string _key;

        public JWTAuthenticationManager(string key)
        {
            _key = key;
        }
        public string GenerateToken(UserDTO user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            
            var tokenKey = Encoding.ASCII.GetBytes(_key);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
            };
            foreach (var item in user.UserRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, item.Name));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                IssuedAt = DateTime.Now,
                Expires = DateTime.Now.AddHours(2),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature),
                Audience = "Private Eye Users",
                Issuer = "Private Eye",
                
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
