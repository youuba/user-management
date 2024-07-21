using AutoMapper;
using UMS.Application.Common.Interfaces;
using UMS.Application.Common.Models;

namespace UMS.Application.Services;

public class UserService(IUnitOfWork unitOfWork, IMapper mapper) : IUserService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<UserDto>> GetAll()
    {
        var users = await _unitOfWork.UserRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<UserDto>>(users);
    }
    public async Task<UserDto> GetById(int id)
    {
        var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
        return user == null ? null! : _mapper.Map<UserDto>(user);
    }
    public async Task Create(UserDto request)
    {
        var userEntity = _mapper.Map<User>(request);
        await _unitOfWork.UserRepository.CreateAsync(userEntity);
        await _unitOfWork.SaveChangesAsync();
    }
    public async Task Update(UserDto request)
    {
        var existingUser = await _unitOfWork.UserRepository.GetByIdAsync(request.Id);
        var userEntity = _mapper.Map(request, existingUser);
        _unitOfWork.UserRepository.Update(userEntity!);
        await _unitOfWork.SaveChangesAsync();
    }
    public async Task Delete(int id)
    {
        var userEntity = await _unitOfWork.UserRepository.GetByIdAsync(id);
        _unitOfWork.UserRepository.Delete(userEntity!);
        await _unitOfWork.SaveChangesAsync();
    }

}
