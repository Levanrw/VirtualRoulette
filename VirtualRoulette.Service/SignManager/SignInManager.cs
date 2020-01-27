using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using VirtualRoulette.Models;

namespace VirtualRoulette.Service.SignManager
{
    public class SignInManager : ISignInManager
    {
        private readonly TokenSettings _tokenSettings;

        public SignInManager(IOptions<TokenSettings> tokenSettings)
        {
            _tokenSettings = tokenSettings.Value;
        }

        public string GetPassword(string flatPassword, string salt)
        {
            string inputString = $"{flatPassword}{salt}";

            var crypter = new SHA256Managed();
            var stringBuilder = new StringBuilder();

            byte[] crypto = crypter.ComputeHash(Encoding.UTF8.GetBytes(inputString));

            foreach (byte theByte in crypto)
                stringBuilder.Append(theByte.ToString("x2"));

            return stringBuilder.ToString();
        }

        public string GenerateToken(string UserName, string UserId)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_tokenSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, UserId)
                    }),
                    Expires = DateTime.Now.AddMinutes(_tokenSettings.AccessExpiration),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);

                return tokenHandler.WriteToken(token);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
