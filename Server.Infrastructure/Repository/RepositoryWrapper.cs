using Server.Core.Interfaces.Repository;

namespace Domain.Repository {
    public class RepositoryWrapper : IRepositoryWrapper {
        private BmpDbContext _dbContext;
        private IUserRepository _owner;
        private IAccountRepository _account;

        public RepositoryWrapper(BmpDbContext dbContext) {
            _dbContext = dbContext;
        }

        public IUserRepository Owner {
            get {
                if (_owner == null) {
                    _owner = new UserRepository(_dbContext);
                }
                return _owner;
            }
        }

        public IAccountRepository Account {
            get {
                if (_account == null) {
                    _account = new AccountRepository(_dbContext);
                }

                return _account;
            }
        }

        public void Save() {
            _dbContext.SaveChanges();
        }
    }
}
