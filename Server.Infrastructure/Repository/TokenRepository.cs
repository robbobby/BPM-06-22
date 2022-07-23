using Server.Core.Models.Entities.Entity;

namespace Domain.Repository; 

public class TokenRepository : RepositoryBase<Token>, ITokenRepository{

    public TokenRepository(BmpDbContext dbContext) : base(dbContext) {
    }
}
