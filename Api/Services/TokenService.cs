using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Api.Controllers;
using Api.Interfaces;
using Api.Models.DbModel;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Api.Services; 

public class TokenService : ITokenService {

    private readonly IConfiguration _config;
    private readonly ILogger<TokenService> _logger;

    public TokenService(IConfiguration config, ILogger<TokenService> logger) {
        _config = config;
        _logger = logger;
    }

    public async Task<string> GenerateToken(User user) {
        var configJwtSecret = _config.GetSection("JWT").GetValue<string>("JWT_SECRET");
        var issuer = _config.GetSection("JWT").GetValue<string>("JWT_ISSUER");
        var audience = _config.GetSection("JWT").GetValue<string>("JWT_AUDIENCE");
        
        var jwtSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configJwtSecret));

        var tokenDescriptor = new SecurityTokenDescriptor {
            Subject = new ClaimsIdentity(new Claim[] {
                new Claim(ClaimTypes.Role, user.AccountUsers[0].Role),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim("Account", user.AccountUsers[0].AccountId.ToString()),
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(jwtSecret, SecurityAlgorithms.HmacSha256Signature)
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        
        return tokenHandler.WriteToken(token);
    }
 
    // method to validate token
    public bool ValidateToken(string token) {
        var configJwtSecret = _config.GetSection("JWT").GetValue<string>("JWT_SECRET");
        var issuer = _config.GetSection("JWT").GetValue<string>("JWT_ISSUER");
        var audience = _config.GetSection("JWT").GetValue<string>("JWT_AUDIENCE");
        
        var jwtSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configJwtSecret));
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters {
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = true,
            ValidAudience = audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = jwtSecret,
        };
        
        try {
            tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
            string? accountId = (validatedToken as JwtSecurityToken)?.Payload["Account"].ToString();
            return true;
        } catch (Exception) {
            return false;
        }
    }
}
