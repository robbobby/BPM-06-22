using Api.Interfaces.Repository;
using Api.Models;
using Api.Models.DbModel;
using Microsoft.EntityFrameworkCore;

namespace Api.Repository; 

public class UserRepository : RepositoryBase<User>, IUserRepository {
    private BmpDbContext _dbContext;
    
    public UserRepository(BmpDbContext dbContext) : base(dbContext) {
        _dbContext = dbContext;
    }

    public Task<User?> GetUserByEmail(string loginModelEmail) {
        return _dbContext.Users.FirstOrDefaultAsync(x => x.EmailAddress == loginModelEmail);    
    }
}
