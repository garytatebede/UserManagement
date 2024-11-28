using FluentAssertions;
using Moq;
using UserManagement.Repositories.Users;
using UserManagement.Services;
using UserManagement.Services.Users;
using UserManagement.Services.Users.CreateUser;
using UserManagement.Services.Users.DeleteUser;

namespace UserManagement.UnitTests.Services.Users;

[TestFixture]
public class DeleteUserServiceTests
{
    private Mock<IUserRepository> _userRepositoryMock;

    private DeleteUserService _service;

    [SetUp]
    public void BeforeEach()
    {
        _userRepositoryMock = new Mock<IUserRepository>();

        _service = new DeleteUserService(_userRepositoryMock.Object);
    }

    [Test]
    public void DeleteUser_Success()
    {
        // Arrange
        var id = Guid.NewGuid();

        var request = new DeleteByIdRequest(id);
        
        _userRepositoryMock.Setup(m => m.Exists(id)).Returns(true);

        // Act
        _service.Delete(request);

        // Assert
        _userRepositoryMock.Verify(m => m.Delete(id), Times.Once);
    }
    
    [Test]
    public void DeleteUser_UserNotFound_ReturnsError()
    {
        // Arrange
        var id = Guid.NewGuid();

        var request = new DeleteByIdRequest(id);
        
        _userRepositoryMock.Setup(m => m.Exists(id)).Returns(false);

        // Act
        Action shouldThrow = () => _service.Delete(request);
        
        // Assert
        shouldThrow.Should()
            .Throw<UserNotFoundException>()
            .WithMessage($"User with Id {id} was not found");

        _userRepositoryMock.Verify(m => m.Delete(id), Times.Never);
    }
    
    [Test]
    public void DeleteUser_GuidEmpty_ReturnsError()
    {
        // Arrange
        var id = Guid.Empty;

        var request = new DeleteByIdRequest(id);
        
        // Act
        Action shouldThrow = () => _service.Delete(request);
        
        // Assert
        shouldThrow.Should()
            .Throw<ArgumentException>()
            .WithMessage("Guid can't be empty");

        _userRepositoryMock.Verify(m => m.Delete(id), Times.Never);
    }
}
