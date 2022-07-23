using Server.Core.Models.Entities.Entity;

namespace Server.Core.Interfaces.Repository; 

public interface IUserRepository : IRepositoryBase<User> {
    Task<User?> GetUserByEmail(string loginModelEmail);
}
