using Api.Interfaces.Repository;
using Api.Models;
using Api.Models.DbModel;

namespace Api.Repository; 

public class UserRepository : RepositoryBase<User>, IUserRepository {

    public UserRepository(BmpDbContext dbContext) : base(dbContext) {
    }
}
