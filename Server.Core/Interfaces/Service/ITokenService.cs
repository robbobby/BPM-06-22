using Server.Core.Models;
using Server.Core.Models.Entities.Entity;

namespace Server.Core.Interfaces.Service;

public interface ITokenService {
    Task<Token> GenerateToken(User user, string? accountId = null);
    bool ValidateToken(string token);
    string? GetUserIdFromToken(string token);
    Task<T> GetToken<T>(User user, string? accountId = null);
    Task<TokenDto> RefreshToken(TokenDto requestToken);
}
