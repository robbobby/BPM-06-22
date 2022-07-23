using Server.Core.Aggregates;
using Server.Core.Models;
using Server.Core.Models.Entities.Entity;

namespace Server.Core.Interfaces.Service;

public interface IUserService {
    User GetUser();
    Task<TokenDto> CreateUser(UserRequest userRequest);
    bool ValidateToken(string token);
    Task<IQueryable<Guid>> GetAllUserAccountIds(string? userId);
}
