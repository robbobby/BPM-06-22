using Api.Models.DbModel;

namespace Api.Interfaces;

public interface ITokenService {
    Task<string> GenerateToken(User user);
}
