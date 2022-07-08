using Api.Models;
using Api.Models.DbModel;

namespace Api.Repository; 

public class TokenRepository : RepositoryBase<Token>, ITokenRepository{

    public TokenRepository(BmpDbContext dbContext) : base(dbContext) {
    }
}
