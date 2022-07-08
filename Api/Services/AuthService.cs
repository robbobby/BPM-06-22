using Api.Interfaces;
using Api.Interfaces.Repository;
using Api.Models.DbModel;

namespace Api.Services;

public class AuthService : IAuthService {
    private readonly ILogger<AuthService> _logger;
    private readonly IUserRepository _userRepository;

    public AuthService(ILogger<AuthService> logger, IUserRepository userRepository) {
        _logger = logger;
        _userRepository = userRepository;
    }

    public async Task<User?> Login(string loginModelEmail, string loginModelPassword) {
        var user = await _userRepository.GetUserByEmail(loginModelEmail);
        if (user == null) 
            return null;
        if (!CheckHashedPassword(loginModelPassword, user.Salt, user.Password))
            return null;
        return user;
    }

    private bool CheckHashedPassword(string loginPassword, string userSalt, string userPassword) {
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(loginPassword, userSalt);
        return hashedPassword == userPassword;
    }

    public Task<string> GenerateToken(User? user) {
        return Task.FromResult("");
    }
}
