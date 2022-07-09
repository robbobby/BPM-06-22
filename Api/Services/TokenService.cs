using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Api.Controllers;
using Api.Interfaces;
using Api.Models;
using Api.Models.DbModel;
using Api.Repository;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Api.Services;

public class TokenService : ITokenService {

    private readonly IConfiguration _config;
    private readonly ILogger<TokenService> _logger;
    private readonly ITokenRepository _tokenRepository;
    private readonly IMapper _mapper;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly SymmetricSecurityKey _jwtSecret;

    public TokenService(IConfiguration config, ILogger<TokenService> logger, ITokenRepository tokenRepository, IMapper mapper) {
        _config = config;
        _logger = logger;
        _tokenRepository = tokenRepository;
        _mapper = mapper;

        var configJwtSecret = _config.GetSection("JWT").GetValue<string>("JWT_SECRET");
        _issuer = _config.GetSection("JWT").GetValue<string>("JWT_ISSUER");
        _audience = _config.GetSection("JWT").GetValue<string>("JWT_AUDIENCE");
        _jwtSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configJwtSecret));
    }


    public async Task<Token> GenerateToken(User user, string? accountId = null) {
        accountId = accountId.IsNullOrEmpty() ? 
            user.DefaultAccount.ToString() : accountId;

        var accessToken = GetAccessToken(user, accountId);
        
        var tokenDto = SaveToken(user, accountId, accessToken);

        return tokenDto;
    }

    private string GetAccessToken(User user, string? accountId) {
        var tokenDescriptor = new SecurityTokenDescriptor {
            Subject = new ClaimsIdentity(new Claim[] {
                new Claim(ClaimTypes.Role, user.AccountUsers[0].Role),
                new Claim("UserId", user.Id.ToString()),
                new Claim("Account", ((!accountId.IsNullOrEmpty() ?
                    accountId : user.AccountUsers[0].AccountId.ToString()) ?? string.Empty)),
            }),
            Expires = DateTime.UtcNow.AddMinutes(5),
            Issuer = _issuer,
            Audience = _audience,
            SigningCredentials = new SigningCredentials(_jwtSecret, SecurityAlgorithms.HmacSha256Signature)
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenSec = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(tokenSec);
    }

    private Token SaveToken(User user, string accountId, string accessToken) {
        var token = new Token {
            AccessToken = accessToken,
            RefreshToken = Guid.NewGuid().ToString(),
            ExpirationDate = DateTime.Now.AddDays(7),
            User = user,
            AccountId = accountId
        };
        _tokenRepository.Create(token);
        _tokenRepository.SaveChanges();

        return token;
    }

    public bool ValidateToken(string token) {
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters {
            ValidateIssuer = true,
            ValidIssuer = _issuer,
            ValidateAudience = true,
            ValidAudience = _audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = _jwtSecret,
        };

        try {
            tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
            string? accountId = (validatedToken as JwtSecurityToken)?.Payload["Account"].ToString();
            return true;
        } catch (Exception) {
            return false;
        }
    }

    public string? GetUserIdFromToken(string token) {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jsonToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
        var userId = jsonToken?.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
        return userId;
    }


    public async Task<TokenDto> GetRefreshedToken(RefreshTokenRequestModel refreshTokenRequestModel) {
        var token = _tokenRepository.FindByCondition(t => t.RefreshToken == refreshTokenRequestModel.RefreshToken
                && t.AccessToken == refreshTokenRequestModel.AccessToken).Include(table => table.User.AccountUsers)
            .FirstOrDefault();
        
        if (token == null) 
            throw new Exception("Invalid token");
        _logger.LogError("*******************************************************************************************");
        _logger.LogError("Time now: " + DateTime.Now.ToString());
        _logger.LogError("Tokennow: " + token.ExpirationDate.ToString());
        _logger.LogError("*******************************************************************************************");
        var accessTokenExpiryDate = new JwtSecurityTokenHandler().ReadJwtToken(token.AccessToken).ValidTo; 
        if (accessTokenExpiryDate < DateTime.UtcNow) {
            return _mapper.Map<TokenDto>(await RefreshToken(token));
        }
        
        throw new Exception("Token is still valid");
    }

    private async Task<Token> RefreshToken(Token token) {
        token.AccessToken = GetAccessToken(token.User, token.AccountId);
        var updatedToken = _tokenRepository.Update(token);
        _tokenRepository.SaveChanges();
        return updatedToken;
    }
    
    public async Task<T> GetToken<T>(User user, string? accountId = null) {
        var token = await GenerateToken(user);
        return _mapper.Map<T>(token);
    }
}
