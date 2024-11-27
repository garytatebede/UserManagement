using FluentAssertions;
using Moq;
using UserManagement.Repositories.Users;
using UserManagement.Services.Users;
using UserManagement.Services.Users.UpdateUser;

namespace UserManagement.UnitTests.Services.Users;

[TestFixture]
public class UpdateUserServiceTests
{
    private Mock<IUserRepository> _userRepositoryMock;

    private UpdateUserService _service;

    [SetUp]
    public void BeforeEach()
    {
        _userRepositoryMock = new Mock<IUserRepository>();

        _service = new UpdateUserService(_userRepositoryMock.Object);
    }

    [Test]
    public void UpdateUser_Success()
    {
        // Arrange
        var id = Guid.NewGuid();
        var username = "Gary Tate";
        var newUsername = "John Smith";
    
        _userRepositoryMock.Setup(m => m.Exists(id)).Returns(true);
        _userRepositoryMock.Setup(m => m.GetById(id)).Returns(new User(id, username));
        var request = new UpdateUserRequest(id, newUsername);
    
        // Act
        var updated = _service.Update(request);
    
        // Assert
        updated.Id.Should().Be(id);
        updated.Username.Should().Be(newUsername);
    
        _userRepositoryMock.Verify(m => m.ExistsByUsername(username), Times.Never);
        _userRepositoryMock.Verify(m => m.ExistsByUsername(newUsername), Times.Once);
    }
    
    [TestCase("")]
    [TestCase(null)]
    [TestCase("   ")]
    public void UpdateUser_BadRequestWithUsername(string newUsername)
    {
        // Arrange
        var id = Guid.NewGuid();
        var username = "Gary Tate";
    
        _userRepositoryMock.Setup(m => m.GetById(id)).Returns(new User(id, username));
        var request = new UpdateUserRequest(id, newUsername);
    
        // Act
        Action shouldThrow = () => _service.Update(request);
        
        // Assert
        shouldThrow.Should().Throw<ArgumentException>();
    
        _userRepositoryMock.Verify(m => m.ExistsByUsername(username), Times.Never);
        _userRepositoryMock.Verify(m => m.ExistsByUsername(newUsername), Times.Never);
    }
    
    [Test]
    public void UpdateUser_IdDoesNotExist_ExpectError()
    {
        // Arrange
        var id = Guid.NewGuid();
        var username = "Gary Tate";
    
        _userRepositoryMock.Setup(m => m.GetById(id)).Returns((User)null);
        var request = new UpdateUserRequest(id, username);
    
        // Act
        Action shouldThrow = () => _service.Update(request);
    
        // Assert
        shouldThrow.Should()
            .Throw<UserNotFoundException>()
            .WithMessage($"User with Id {id} was not found");
    }
    
    [Test]
    public void UpdateUser_UsernameAlreadyExists_ExpectError()
    {
        // Arrange
        var id = Guid.NewGuid();
        var username = "Gary Tate";
    
        _userRepositoryMock.Setup(m => m.Exists(id)).Returns(true);
        _userRepositoryMock.Setup(m => m.ExistsByUsername(username)).Returns(true);

        var request = new UpdateUserRequest(id, username);
    
        // Act
        Action shouldThrow = () => _service.Update(request);
    
        // Assert
        shouldThrow.Should()
            .Throw<UserExistsException>()
            .WithMessage($"User with name {username} already exists");
        
        _userRepositoryMock.Verify(m => m.ExistsByUsername(username), Times.Once);
        _userRepositoryMock.Verify(m => m.GetById(id), Times.Never);

    }
}
