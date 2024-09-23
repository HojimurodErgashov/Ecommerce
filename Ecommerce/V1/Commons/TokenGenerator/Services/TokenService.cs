using Ecommerce.V1.Commons.TokenGenerator.Interface;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ecommerce.V1.Commons.TokenGenerator.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        public TokenService(IConfiguration configuration)
            => _configuration = configuration;
        public string WriteToken(IEnumerable<Claim> claims)
        {
            var authSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:ValidIssuer"],
                expires: DateTime.Now.AddHours(int.Parse(_configuration["JWT:Expire"])),
                claims:claims,
                signingCredentials:new SigningCredentials(
                    key: authSigningKey,
                    algorithm:SecurityAlgorithms.HmacSha256)
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
