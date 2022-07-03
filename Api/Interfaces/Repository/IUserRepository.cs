using Api.Models.DbModel;
using Api.Repository;

namespace Api.Interfaces.Repository; 

public interface IUserRepository : IRepositoryBase<User> {
    void SaveChanges();

}
