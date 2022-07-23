using Server.Core;
using Server.Core.Models;
using Server.Core.Models.Entities.Entity;

namespace Domain.Repository; 

public interface IAccountUserRepository : IRepositoryBase<AccountUser> {
    IQueryable<Guid> GetAllUserAccountIds(string? userId);
    IQueryable<AccountUserIdsRole> GetAllUserAccountsIdsRole(string? userId);
}
