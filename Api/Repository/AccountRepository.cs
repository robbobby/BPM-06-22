using Api.Interfaces.Repository;
using Api.Models;
using Api.Models.DbModel;

namespace Api.Repository; 

public class AccountRepository : RepositoryBase<Account>, IAccountRepository {

    public AccountRepository(BmpDbContext dbContext) : base(dbContext) {
    }
}
