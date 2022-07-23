using Server.Core.Models.Entities.Entity;

namespace Server.Core.Interfaces.Service;

public interface IAuthService {
    Task<User?> Login(string email, string password);
}
