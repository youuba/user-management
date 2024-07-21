using System.Linq.Expressions;

namespace UMS.Infrastructure.Interface;

public interface IBaseRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(object id);
    Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter);
    Task CreateAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
}
