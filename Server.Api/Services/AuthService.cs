using Server.Core.Interfaces.Repository;
using Server.Core.Interfaces.Service;
using Server.Core.Models.Entities.Entity;

namespace Api.Services;

public class AuthService : IAuthService {
    private readonly ILogger<AuthService> _logger;
    private readonly IUserRepository _userRepository;

    public AuthService(IUserRepository userRepository, ILogger<AuthService> logger) {
        _logger = logger;
        _userRepository = userRepository;
    }

    public async Task<User?> Login(string email, string password) {
        var user = await _userRepository.GetUserByEmail(email);
        if (user == null) 
            return null;
        if (!CheckHashedPassword(password, user.Salt, user.Password))
            return null;
        return user;
    }

    private bool CheckHashedPassword(string loginPassword, string userSalt, string userPassword) {
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(loginPassword, userSalt);
        return hashedPassword == userPassword;
    }
}
