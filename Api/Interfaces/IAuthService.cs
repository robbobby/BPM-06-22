using Api.Models.DbModel;

namespace Api.Interfaces;

public interface IAuthService {
    Task<User?> Login(string loginModelEmail, string loginModelPassword);
    Task<string> GenerateToken(User? user);
}