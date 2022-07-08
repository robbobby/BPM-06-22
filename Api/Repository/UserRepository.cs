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

    public async Task<User?> GetUserByEmail(string loginModelEmail) {
        // get User by email with AccountUser[] where user.EmailAddress == loginModelEmail
        var user = await _dbContext.Users.Include(u => u.AccountUsers).FirstOrDefaultAsync(u => u.EmailAddress == loginModelEmail);
        // var accountUser = await _dbContext.AccountUsers.FirstOrDefaultAsync(au => au.UserId == user.Id && au.AccountId == user.DefaultAccount);
        // if (accountUser != null && user != null)
            // user.AccountUsers = new List<AccountUser>() {
                // accountUser
            // };
        return user;
    }
}
