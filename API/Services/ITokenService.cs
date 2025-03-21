using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    public class TokenService : ITokenService
    {

        private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration configuration)
        {
            var tokenKey = configuration["TokenKey"];

            if (string.IsNullOrEmpty(tokenKey))
            {
                throw new ArgumentNullException("TokenKey", "TokenKey is missing in the configuration.");
            }

            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
        }

        public string CreateToken(AppUser user)
        {
           var claims = new List<Claim>
           {
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.NameId, user.UserName) 
           };

           var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature);

           var tokenDescriptor = new SecurityTokenDescriptor
           {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
           };

           var tokenHandler = new JwtSecurityTokenHandler();
           var token = tokenHandler.CreateToken(tokenDescriptor);

           return tokenHandler.WriteToken(token);


        }
    }
}