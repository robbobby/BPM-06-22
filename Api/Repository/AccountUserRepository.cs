using Api.Models;
using Api.Models.DbModel;

namespace Api.Repository; 

public class AccountUserRepository : RepositoryBase<AccountUser>, IAccountUserRepository {
    private readonly BmpDbContext _dbContext;


    public AccountUserRepository(BmpDbContext dbContext) : base(dbContext) {
        _dbContext = dbContext;
    }

    public List<Guid> GetAllUserAccountIds(string? userId) {
        return _dbContext.AccountUsers.Where(x => x.UserId.ToString() == userId)
            .Select(x => x.AccountId)
            .ToList();
    }
}
