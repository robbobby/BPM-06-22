using Server.Core.Models;
using Server.Core.Models.Entities.Entity;

namespace Domain.Repository; 


// TODO: Tests for this class
public class AccountUserRepository : RepositoryBase<AccountUser>, IAccountUserRepository {
    private readonly BmpDbContext _dbContext;
    
    public AccountUserRepository(BmpDbContext dbContext) : base(dbContext) {
        _dbContext = dbContext;
    }

    public IQueryable<Guid> GetAllUserAccountIds(string? userId) {
        return _dbContext.AccountUsers.Where(x => x.UserId.ToString() == userId)
            .Select(x => x.AccountId);
    }

    public IQueryable<AccountUserIdsRole> GetAllUserAccountsIdsRole(string? userId) {
        return _dbContext.AccountUsers.Where(x => x.UserId.ToString() == userId)
            .Select(x => new AccountUserIdsRole {
                AccountId = x.AccountId.ToString(),
                UserId = x.UserId.ToString(),
                Role = x.Role.ToString()
            });
    }
}