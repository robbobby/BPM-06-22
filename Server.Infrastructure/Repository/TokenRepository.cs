using Server.Core.Models.Entities.Entity;

namespace Domain.Repository;

public class TokenRepository : RepositoryBase<Token>, ITokenRepository {
    private readonly BmpDbContext _dbContext;

    public TokenRepository(BmpDbContext dbContext) : base(dbContext) {
        _dbContext = dbContext;
    }

    public override void Create(Token entity) {
        // add a new token to the database without the user
        _dbContext.Tokens.Add(entity);
    }
}
