using Microsoft.EntityFrameworkCore;
using Server.Core.Interfaces.Repository;
using Server.Core.Models.Entities.Entity;

namespace Domain.Repository;

public class UserRepository : RepositoryBase<User>, IUserRepository {
    private BmpDbContext _dbContext;

    public UserRepository(BmpDbContext dbContext) : base(dbContext) {
        _dbContext = dbContext;
    }

    public async Task<User?> GetUserByEmail(string loginModelEmail) {
        var user = await _dbContext.Users.Include(u => u.AccountUsers).FirstOrDefaultAsync(u => u.EmailAddress == loginModelEmail);
        return user;
    }
}
