using Server.Core.Aggregates;
using Server.Core.Models;
using Server.Core.Models.Entities.Entity;

namespace Server.Core.Interfaces.Service;

public interface IUserService {
    Task<TokenDto> CreateUser(UserRequest userRequest);
    Task<IQueryable<Guid>> GetAllUserAccountIds(string? userId);
}
