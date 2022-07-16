using Api.Controllers;
using Api.Models;
using Api.Models.DbModel;

namespace Api.Interfaces;

public interface ITokenService {
    Task<Token> GenerateToken(User user, string? accountId = null);
    bool ValidateToken(string token);
    string? GetUserIdFromToken(string token);
    Task<T> GetToken<T>(User user, string? accountId = null);
    Task<TokenDto> GetRefreshedToken(RefreshTokenRequestModel refreshTokenRequestModel);
}
