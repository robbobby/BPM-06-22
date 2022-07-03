using System.Linq.Expressions;
using Api.Models;
using Api.Models.DbModel;
using Microsoft.EntityFrameworkCore;

namespace Api.Repository; 

public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class, IDbModel {
    private readonly BmpDbContext _dbContext;

    public RepositoryBase(BmpDbContext dbContext) {
        _dbContext = dbContext;
    }

    public IQueryable<T> FindAll() {
        _dbContext.Set<T>().AsNoTracking();
        throw new NotImplementedException();
    }

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression) {
        return _dbContext.Set<T>().Where(expression).AsNoTracking();
    }

    public void Create(T entity) {
        _dbContext.Set<T>().Add(entity);
    }

    public void Update(T entity) {
        _dbContext.Set<T>().Update(entity);
    }

    public void Delete(T entity) {
        _dbContext.Set<T>().Remove(entity);
    }

    public void SaveChanges() {
        _dbContext.SaveChanges();
    }
}
