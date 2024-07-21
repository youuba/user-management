using AutoFixture;
using UMS.Domain.Entities;
using UMS.Infrastructure;
using UMS.UnitTests.Infrastracture;

namespace UMS.UnitTests.Application.Repositories
{
    public class UserRepositoryTests
    {
        private readonly InMemoryDbContext _context;
        private readonly UnitOfWork _unitOfWork;
        private readonly IFixture _fixture;

        public UserRepositoryTests()
        {
            _context = new InMemoryDbContext();
            _unitOfWork = new UnitOfWork(_context);
            _fixture = new Fixture();
        }
        [Fact]
        public async Task GetAllAsync_ShouldReturnExpectedUsersList()
        {
            // Arrange: Create two new user objects
            var user1 = _fixture.Create<User>();
            var user2 = _fixture.Create<User>();

            //Add both users to the repository
            await _unitOfWork.UserRepository.CreateAsync(user1);
            await _unitOfWork.UserRepository.CreateAsync(user2);
            await _unitOfWork.SaveChangesAsync();

            // Act : Fetch all users from the repository
            var users = await _unitOfWork.UserRepository.GetAllAsync();

            //Assert : Ensure both user1 and user2 are found in the fetched users list
            Assert.Contains(users, u => u.Id == user1.Id);
            Assert.Contains(users, u => u.Id == user2.Id);
            Assert.Equal(2, users.Count());
        }
        [Fact]
        public async Task GetAllAsync_WhenNoDataExists_ShouldReturnEmptyList()
        {
            // Act
            var users = await _unitOfWork.UserRepository.GetAllAsync();

            // Assert
            Assert.Empty(users);
        }
        [Fact]
        public async Task GetByIdAsync_ShouldReturnExpectedUser()
        {
            // Arrange: Create and add a new user
            var user = _fixture.Create<User>();
            await _unitOfWork.UserRepository.CreateAsync(user);
            await _context.SaveChangesAsync();

            // Act: Get the user by their ID
            var retrievedUser = await _unitOfWork.UserRepository.GetByIdAsync(user.Id);

            // Assert: Ensure the user was found 
            Assert.NotNull(retrievedUser);
            Assert.Equal(user.Id, retrievedUser.Id);
        }
        [Fact]
        public async Task GetByIdAsync_WhenUserNotFound_ShouldReturnNull()
        {
            // Arrange: Define an ID that doesn't exist in the repository
            var nonExistentUserId = _fixture.Create<int>();

            // Act: Try to fetch a user with the non-existent ID
            var result = await _unitOfWork.UserRepository.GetByIdAsync(nonExistentUserId);

            // Assert: Verify that the result is null
            Assert.Null(result);
        }
        [Fact]
        public async Task AddAsync_ShouldAddUser()
        {
            // Arrange : Create new user
            var newUser = _fixture.Create<User>();

            // Act :Add the user to the repository
            await _unitOfWork.UserRepository.CreateAsync(newUser);
            await _context.SaveChangesAsync();

            // Assert :Fetch the user to ensure it was added successfully
            var addedUser = await _unitOfWork.UserRepository.FirstOrDefaultAsync(x => x.Id == newUser.Id);
            Assert.NotNull(addedUser);
            Assert.Equal(newUser.Id, addedUser.Id);
        }
        [Fact]
        public async Task Update_ShouldUpdateUser()
        {
            // Arrange: Create and add a new user
            var user = _fixture.Create<User>();
            await _unitOfWork.UserRepository.CreateAsync(user);
            await _context.SaveChangesAsync();

            // Act : Update the user’s name
            user.UserName = _fixture.Create<string>();
            _unitOfWork.UserRepository.Update(user);
            await _context.SaveChangesAsync();

            // Assert : Fetch the user to ensure the user’s name was updated as expected
            var updatedUser = await _unitOfWork.UserRepository.FirstOrDefaultAsync(x => x.Id == user.Id);
            Assert.NotNull(updatedUser);
            Assert.Equal(user.UserName, updatedUser.UserName);
        }
        [Fact]
        public async Task Delete_ShouldDeleteUser()
        {
            // Arrange: Create and add a new user
            var user = _fixture.Create<User>();
            await _unitOfWork.UserRepository.CreateAsync(user);
            await _context.SaveChangesAsync();

            var usr = await _unitOfWork.UserRepository.GetByIdAsync(user.Id);
            _unitOfWork.UserRepository.Delete(usr!);
            await _context.SaveChangesAsync();

            // Assert: Try to find the deleted user in the repository
            var deletedUser = await _unitOfWork.UserRepository.FirstOrDefaultAsync(u => u.Id == user.Id);
            Assert.Null(deletedUser);
        }
    }
}
