using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using UMS.Application.Common.Interfaces;
using UMS.Application.Common.Models;
using UMS.Web.Controllers;

namespace UMS.UnitTests.Web.Controllers
{
    public class UserControllerTests
    {
        private readonly UserController _controller;
        private readonly Mock<IUserService> _mockUserService;
        private readonly Mock<Serilog.ILogger> _mockLogger;
        private readonly IFixture _fixture;

        public UserControllerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _mockLogger = new Mock<Serilog.ILogger>();
            _fixture = new Fixture();
            _controller = new UserController(_mockUserService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAllUsers_WithExistingUsers_ReturnsAllUsers()
        {
            // Arrange
            var users = _fixture.Create<List<UserDto>>();

            // Setup
            _mockUserService.Setup(s => s.GetAll()).ReturnsAsync(users);

            // Act
            var response = await _controller.GetAllUsers();

            // Assert
            Assert.IsType<OkObjectResult>(response);
            _mockUserService.Verify(x => x.GetAll(), Times.Once);
        }
        [Fact]
        public async Task GetAllUsers_WithNonExistingUsers_ReturnsNotFound()
        {
            // Arrange
            var usersDto = _fixture.Build<List<UserDto>>().OmitAutoProperties().Create();

            // Setup
            _mockUserService.Setup(s => s.GetAll()).ReturnsAsync(usersDto);

            // Act
            var response = await _controller.GetAllUsers();

            // Asert
            Assert.IsType<NotFoundResult>(response);
            _mockUserService.Verify(x => x.GetAll(), Times.Once);
        }
        [Fact]
        public async Task GetUserById_WithExistingUser_ReturnsExpectedUser()
        {
            // Arrange
            var userId = _fixture.Create<int>();
            var userDto = _fixture.Create<UserDto>();

            // Setup  
            _mockUserService.Setup(s => s.GetById(userId)).ReturnsAsync(userDto);

            // Act
            var response = await _controller.GetUserById(userId);

            // assert
            Assert.IsType<OkObjectResult>(response);
            _mockUserService.Verify(x => x.GetById(userId), Times.Once);
        }
        [Fact]
        public async Task GetUserById_WithNonExistingUser_ReturnsNotFound()
        {
            // Arrange
            var userId = _fixture.Create<int>();

            // Setup  
            _mockUserService.Setup(s => s.GetById(userId)).ReturnsAsync(It.IsAny<UserDto>);

            // Act
            var response = await _controller.GetUserById(userId);

            // assert
            Assert.IsType<NotFoundResult>(response);
            _mockUserService.Verify(x => x.GetById(userId), Times.Once);
        }
        [Fact]
        public async Task CreateUser_WithUserToCreate_ReturnsOk()
        {
            // Arrange  
            var userDto = _fixture.Create<UserDto>();

            // Setup
            _mockUserService.Setup(s => s.Create(userDto)).Returns(Task.CompletedTask);

            // Act
            var response = await _controller.CreateUser(userDto);

            // Assert  
            Assert.IsType<OkResult>(response);
            _mockUserService.Verify(s => s.Create(userDto), Times.Once);
        }
        [Fact]
        public async Task UpdateUser_WithExistingUser_ReturnsNoContent()
        {
            //Arrange
            var UserName = _fixture.Create<string>();
            var userId = _fixture.Create<int>();
            var userDto = _fixture.Create<UserDto>();
            userDto.UserName = UserName;
            userDto.Id = userId;

            //Setup
            _mockUserService.Setup(x => x.GetById(userId)).ReturnsAsync(userDto);
            _mockUserService.Setup(x => x.Update(userDto)).Returns(Task.CompletedTask);

            //Act
            var response = await _controller.UpdateUser(userId, userDto);

            //Assert
            Assert.IsType<NoContentResult>(response);
            _mockUserService.Verify(svc => svc.Update(It.IsAny<UserDto>()), Times.Once);

        }
        [Fact]
        public async Task UpdateUser_WhithNonExistingUser_ReturnsNotFound()
        {
            // Arrange
            var userId = _fixture.Create<int>();
            var userDto = _fixture.Create<UserDto>();

            // Setup  
            _mockUserService.Setup(s => s.GetById(userId)).ReturnsAsync(It.IsAny<UserDto>);

            // Act
            var response = await _controller.UpdateUser(userId, userDto);

            // Assert
            Assert.IsType<NotFoundResult>(response);
            _mockUserService.Verify(svc => svc.Update(It.IsAny<UserDto>()), Times.Never);
        }
        [Fact]
        public async Task DeleteUser_WithExistingUser_ReturnsNoContent()
        {
            // Arrange
            var userId = _fixture.Create<int>();
            var userDto = _fixture.Create<UserDto>();
            userDto.Id = userId;

            //Setup
            _mockUserService.Setup(svc => svc.GetById(userId)).ReturnsAsync(userDto);
            _mockUserService.Setup(svc => svc.Delete(userId)).Returns(Task.CompletedTask);

            // Act
            var response = await _controller.DeleteUser(userId);

            // Assert
            Assert.IsType<NoContentResult>(response);
            _mockUserService.Verify(svc => svc.Delete(userId), Times.Once);
        }
        [Fact]
        public async Task DeleteUser_WithNonExistingUser_ReturnsNotFound()
        {
            // Arrange
            var userId = _fixture.Create<int>();

            //Setup
            _mockUserService.Setup(svc => svc.GetById(userId)).ReturnsAsync(It.IsAny<UserDto>);

            // Act
            var result = await _controller.DeleteUser(userId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            _mockUserService.Verify(svc => svc.Delete(It.IsAny<int>()), Times.Never);
        }
    }
}
