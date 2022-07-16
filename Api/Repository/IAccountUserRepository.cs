using Api.Models.DbModel;

namespace Api.Repository; 

public interface IAccountUserRepository : IRepositoryBase<AccountUser> {
    List<Guid> GetAllUserAccountIds(string? userId);
}
