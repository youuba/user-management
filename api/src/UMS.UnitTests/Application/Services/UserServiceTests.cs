using AutoFixture;
using AutoMapper;
using UMS.Application;
using UMS.Application.Common.Models;
using UMS.Application.Services;
using UMS.Domain.Entities;

namespace UMS.UnitTests.Application.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUnitOfWork> _uow;
        private readonly Mock<IMapper> _mapper;
        private readonly UserService _userSvc;
        private readonly IFixture _fixture;

        public UserServiceTests()
        {
            _uow = new Mock<IUnitOfWork>();
            _mapper = new Mock<IMapper>();
            _userSvc = new UserService(_uow.Object, _mapper.Object);
            _fixture = new Fixture();
        }
        [Fact]
        public async Task GetAll_WithExistingUsers_ReturnsAllUsers()
        {
            // Arrange: Create a list of User entities
            var users = _fixture.Create<IEnumerable<User>>();

            // Mock the repository to return the list of users
            _uow.Setup(u => u.UserRepository.GetAllAsync()).ReturnsAsync(users);

            // Setup the mapping of User entities to UserDto objects
            var userDtos = _fixture.Create<IEnumerable<UserDto>>();

            _mapper.Setup(m => m.Map<IEnumerable<UserDto>>(users)).Returns(userDtos);

            // Act: Call the GetAll method of the service
            var result = await _userSvc.GetAll();

            // Assert: Ensure the result is as expected  
            Assert.NotNull(result);
            Assert.Equal(users.Count(), result.Count());
            _uow.Verify(u => u.UserRepository.GetAllAsync(), Times.Once);
        }
        [Fact]
        public async Task GetAll_WhenNoUserExists_ShouldReturnEmptyResult()
        {
            // Arrange 
            var emptyUsersList = new List<User>();
            var emptyUserDtosList = new List<UserDto>();

            // Setup
            _uow.Setup(u => u.UserRepository.GetAllAsync()).ReturnsAsync(emptyUsersList);

            _mapper.Setup(m => m.Map<IEnumerable<UserDto>>(emptyUsersList)).Returns(emptyUserDtosList);

            // Act
            var result = await _userSvc.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _uow.Verify(u => u.UserRepository.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetById_WhenUserExists_ShouldReturnUser()
        {
            // Arrange
            var userId = _fixture.Create<int>();
            var userEntity = _fixture.Create<User>();
            userEntity.Id = userId;
            var userDto = _fixture.Create<UserDto>();
            userDto.Id = userId;

            //Setup
            _uow.Setup(u => u.UserRepository.GetByIdAsync(userId)).ReturnsAsync(userEntity);

            _mapper.Setup(m => m.Map<UserDto>(userEntity)).Returns(userDto);

            // Act
            var actual = await _userSvc.GetById(userId);

            // Assert 
            Assert.NotNull(actual);
            Assert.Equal(userEntity.Id, actual.Id);
            _uow.Verify(u => u.UserRepository.GetByIdAsync(userId), Times.Once);
        }
        [Fact]
        public async Task GetById_WhenUserNotFound_ShouldReturnNull()
        {
            // Arrange
            var userId = _fixture.Create<int>();
            _uow.Setup(u => u.UserRepository.GetByIdAsync(userId))
                .ReturnsAsync(It.IsAny<User>);

            // Act
            var actual = await _userSvc.GetById(userId);

            // Assert
            Assert.Null(actual);
            _uow.Verify(u => u.UserRepository.GetByIdAsync(userId), Times.Once);
        }
        [Fact]
        public async Task Add_WhenValidUserGiven_ShouldAddUser()
        {
            // Arrange
            var userDto = _fixture.Create<UserDto>();
            var userEntity = _fixture.Create<User>();

            //Setup
            _mapper.Setup(m => m.Map<User>(userDto)).Returns(userEntity);

            _uow.Setup(u => u.UserRepository.CreateAsync(userEntity))
                .Returns(Task.CompletedTask);
            _uow.Setup(u => u.SaveChangesAsync());

            //Act
            await _userSvc.Create(userDto);

            //Assert
            _uow.Verify(u => u.UserRepository.CreateAsync(userEntity), Times.Once);
            _uow.Verify(u => u.SaveChangesAsync(), Times.Once);
        }
        [Fact]
        public async Task Update_WhenUserExists_ShouldUpdateUser()
        {
            // Arrange
            var userDto = _fixture.Create<UserDto>();
            var existingUser = _fixture.Create<User>();
            var updatedUserName = _fixture.Create<string>();
            userDto.UserName = updatedUserName;

            // Setup 
            _uow.Setup(u => u.UserRepository.GetByIdAsync(userDto.Id))
                .ReturnsAsync(existingUser);

            _mapper.Setup(mapper => mapper.Map(userDto, existingUser))
            .Callback<UserDto, User>((dto, user) => user.UserName = dto.UserName)
            .Returns(existingUser);

            // Act
            await _userSvc.Update(userDto);

            // Assert
            Assert.Equal(updatedUserName, existingUser.UserName);
            _uow.Verify(u => u.UserRepository.Update(existingUser), Times.Once);
            _uow.Verify(u => u.SaveChangesAsync(), Times.Once);
        }
        [Fact]
        public async Task Delete_WhenUserExists_ShouldDeleteUser()
        {
            //Arrange
            int userId = _fixture.Create<int>();
            var userEntity = _fixture.Create<User>();
            userEntity.Id = userId;

            //Setup
            _uow.Setup(u => u.UserRepository.GetByIdAsync(userId))
                .ReturnsAsync(userEntity);

            _uow.Setup(u => u.UserRepository.Delete(userEntity));

            _uow.Setup(u => u.SaveChangesAsync());

            //Act
            await _userSvc.Delete(userId);

            //Assert
            var userDeleted = await _userSvc.GetById(userId);
            Assert.Null(userDeleted);
            _uow.Verify(u => u.UserRepository.Delete(userEntity), Times.Once);
            _uow.Verify(u => u.SaveChangesAsync(), Times.Once);
        }
    }
}