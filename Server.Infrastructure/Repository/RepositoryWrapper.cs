using Server.Core.Interfaces.Repository;

namespace Domain.Repository {
    public class RepositoryWrapper : IRepositoryWrapper {
        private BmpDbContext _dbContext;
        private IUserRepository _owner;
        private IAccountRepository _account;

        public RepositoryWrapper(BmpDbContext dbContext) {
            _dbContext = dbContext;
        }

        public IUserRepository Owner => _owner ??= new UserRepository(_dbContext);

        public IAccountRepository Account => _account ??= new AccountRepository(_dbContext);

        public void Save() {
            _dbContext.SaveChanges();
        }
    }
}
