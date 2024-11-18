using Microsoft.IdentityModel.Tokens;
using RestApiDesign.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RestApiDesign.Services
{
    public class JwtTokenService
    {
        private readonly IConfiguration _configuration;
        private readonly ISinService _sinService;

        public JwtTokenService(IConfiguration configuration, ISinService sinService)
        {
            _configuration = configuration;
            _sinService = sinService;
        }
        public string GenerateToken(string username, string nationalId)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, username),
            new Claim("nationalid", nationalId),  // Including the nationalId in the payload
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Unique token ID
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64), // Issued at time
        };

            var secretKey = _configuration["Jwt:SecretKey"];
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
