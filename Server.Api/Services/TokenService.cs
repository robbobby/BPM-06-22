using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Server.Core.Exceptions;
using Server.Core.Interfaces.Service;
using Server.Core.Models;
using Server.Core.Models.Entities.Entity;

namespace Api.Services;

public class TokenService : ITokenService {
    private readonly ILogger<TokenService> _logger;
    private readonly ITokenRepository _tokenRepository;
    private readonly IMapper _mapper;
    private readonly IAccountUserRepository _accountUserRepository;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly SymmetricSecurityKey _jwtSecret;

    public TokenService(IConfiguration config, ITokenRepository tokenRepository, IAccountUserRepository accountUserRepository, IMapper mapper, ILogger<TokenService> logger) {
        _logger = logger;
        _tokenRepository = tokenRepository;
        _mapper = mapper;
        this._accountUserRepository = accountUserRepository;

        var configJwtSecret = config.GetSection("JWT").GetValue<string>("JWT_SECRET");
        _issuer = config.GetSection("JWT").GetValue<string>("JWT_ISSUER");
        _audience = config.GetSection("JWT").GetValue<string>("JWT_AUDIENCE");
        _jwtSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configJwtSecret));
    }

    public Task<Token> GenerateToken(User user, string? accountId = null) {
        accountId = accountId.IsNullOrEmpty() ?
            user.DefaultAccount.ToString() : accountId;

        var accessToken = GetAccessToken(user, accountId);

        var tokenDto = SaveToken(user, accountId, accessToken);

        return Task.FromResult(tokenDto);
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
            string? accountId = (validatedToken as JwtSecurityToken)?.Payload["AccountId"].ToString();
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

    public async Task<TokenDto> RefreshToken(TokenDto requestToken) {
        var token = _tokenRepository.FindByCondition(t => t.RefreshToken == requestToken.RefreshToken
                && t.AccessToken == requestToken.AccessToken).Include(table => table.User.AccountUsers)
            .FirstOrDefault();

        if (token == null)
            throw new InvalidTokenException("Invalid token");

        var accessTokenExpiryDate = new JwtSecurityTokenHandler().ReadJwtToken(requestToken.AccessToken).ValidTo;
        if (accessTokenExpiryDate < DateTime.UtcNow) {
            return _mapper.Map<TokenDto>(await RefreshToken(token));
        }

        throw new TokenStillValidException("Token is still valid");
    }

    public async Task<T> GetToken<T>(User user, string? accountId = null) {
        var token = await GenerateToken(user);
        return _mapper.Map<T>(token);
    }

    private string GetAccessToken(User user, string? accountId) {
        accountId = accountId == null ? accountId : user.DefaultAccount.ToString();

        var accountUser = user.AccountUsers.Find(au => au.AccountId.ToString().Equals(accountId));
        if (accountUser == null) {
            var accountUsersDb = _accountUserRepository.GetAllUserAccountsIdsRole(user.Id.ToString()).ToList();
            if (accountUsersDb.Count == 0) {
                throw new AccountUserNotFoundException();
            }
            var accountUsers = accountUsersDb.Find(au => au.AccountId == accountId);
            accountUser = _mapper.Map<AccountUser>(accountUsers);

            if (accountUser == null) {
                throw new AccountUserNotFoundException();
            }
        }

        var tokenDescriptor = new SecurityTokenDescriptor {
            Subject = new ClaimsIdentity(new Claim[] {
                new Claim(ClaimTypes.Role, accountUser.Role),
                new Claim("UserId", user.Id.ToString()),
                new Claim("AccountId", accountUser.AccountId.ToString()),
            }),
            Expires = DateTime.UtcNow.AddMinutes(15),
            Issuer = _issuer,
            Audience = _audience,
            SigningCredentials = new SigningCredentials(_jwtSecret, SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenSec = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(tokenSec);
    }

    private Task<Token> RefreshToken(Token token) {
        token.AccessToken = GetAccessToken(token.User, token.AccountId);
        var updatedToken = _tokenRepository.Update(token);
        _tokenRepository.SaveChanges();
        return Task.FromResult(updatedToken);
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
}
