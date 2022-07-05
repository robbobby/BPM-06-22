using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Api.Controllers;
using Api.Interfaces;
using Api.Models.DbModel;
using Microsoft.IdentityModel.Tokens;

namespace Api.Services; 

public class TokenService : ITokenService {

    private readonly IConfiguration _config;

    public TokenService(IConfiguration config) {
        _config = config;
    }

    public async Task<string> GenerateToken(User user) {
        var ConfigJwtSecret = _config.GetSection("JWT").GetValue<string>("JWT_SECRET");
        var issuer = _config.GetSection("JWT").GetSection("JWT_ISSUER").Value;
        var audience = _config.GetSection("JWT").GetSection("JWT_AUDIENCE").Value;
        
        var JwtSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigJwtSecret));
        
        var tokenDescriptor = new SecurityTokenDescriptor {
            Subject = new ClaimsIdentity(new Claim[] {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim("Account" , user.DefaultAccount.ToString()),
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(JwtSecret, SecurityAlgorithms.HmacSha256Signature)
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        
        return tokenHandler.WriteToken(token);
    }
}
