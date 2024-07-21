using UMS.Infrastructure.Interface;


namespace UMS.Application;

public interface IUnitOfWork : IDisposable
{
    IUserRepository UserRepository { get; }
    Task<int> SaveChangesAsync();

}
