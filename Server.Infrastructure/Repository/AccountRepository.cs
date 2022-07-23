using Server.Core.Interfaces.Repository;
using Server.Core.Models.Entities.Entity;

namespace Domain.Repository; 

public class AccountRepository : RepositoryBase<Account>, IAccountRepository {

    public AccountRepository(BmpDbContext dbContext) : base(dbContext) {
    }
}
