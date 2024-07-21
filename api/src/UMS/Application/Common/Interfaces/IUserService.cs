using UMS.Application.Common.Models;

namespace UMS.Application.Common.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAll();
    Task<UserDto> GetById(int id);
    Task Create(UserDto request);
    Task Update(UserDto request);
    Task Delete(int id);
}
