using UMS.Application;
using UMS.Application.Repositories;
using UMS.Infrastructure.Data;
using UMS.Infrastructure.Interface;

namespace UMS.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private bool _disposed;
    private readonly AppDbContext _context;

    public IUserRepository UserRepository { get; }

    public UnitOfWork(AppDbContext dbContext)
    {
        _context = dbContext;
        UserRepository = new UserRepository(_context);
    }

    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
