using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Api.Repository; 

public interface IRepositoryBase<T> {
    IQueryable<T> FindAll();
    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
    void Create(T entity);
    T Update(T entity);
    void Delete(T entity);
    
    void SaveChanges();
}
