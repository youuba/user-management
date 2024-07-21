using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using UMS.Infrastructure.Data;
using UMS.Infrastructure.Interface;

namespace UMS.Application.Repositories;

[ExcludeFromCodeCoverage]
public class BaseRepository<T>(AppDbContext context) :
    IBaseRepository<T> where T : class
{

    protected DbSet<T> _dbSet = context.Set<T>();

    public async Task<IEnumerable<T>> GetAllAsync()
        => await _dbSet.ToListAsync();

    public async Task<T?> GetByIdAsync(object id)
        => await _dbSet.FindAsync(id);

    public async Task CreateAsync(T entity)
        => await _dbSet.AddAsync(entity);

    public void Update(T entity)
        => _dbSet.Update(entity);


    public void Delete(T entity)
        => _dbSet.Remove(entity);

    public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter)
        => (await _dbSet.IgnoreQueryFilters()
                        .AsNoTracking()
                        .FirstOrDefaultAsync(filter))!;



}
