using System.Diagnostics.CodeAnalysis;
using UMS.Infrastructure.Data;
using UMS.Infrastructure.Interface;

namespace UMS.Application.Repositories;

[ExcludeFromCodeCoverage]
public class UserRepository(AppDbContext context) :
    BaseRepository<User>(context), IUserRepository
{
}
