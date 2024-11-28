using FluentAssertions;
using Moq;
using UserManagement.Repositories.Users;
using UserManagement.Services.Users;
using UserManagement.Services.Users.GetUserById;

namespace UserManagement.UnitTests.Services.Users;

[TestFixture]
public class GetUserByIdServiceTests
{
    private Mock<IUserRepository> _userRepositoryMock;

    private GetUserByIdService _service;

    [SetUp]
    public void BeforeEach()
    {
        _userRepositoryMock = new Mock<IUserRepository>();

        _service = new GetUserByIdService(_userRepositoryMock.Object);
    }

    [Test]
    public void GetUserById_Success()
    {
        // Arrange
        var id = Guid.NewGuid();
        var username = "Gary Tate";
        
        var request = new GetByIdRequest(id);

        _userRepositoryMock.Setup(m => m.GetById(request.Id)).Returns(new User(id, username));
        
        // Act
        var user = _service.Get(request);
        
        // Assert
        user.Id.Should().Be(id);
        user.Username.Should().Be(username);
    }
    
    [Test]
    public void GetUserById_GuidIsEmpty_Error()
    {
        // Arrange
        var id = Guid.Empty;
        
        var request = new GetByIdRequest(id);

        // Act
        Action shouldThrow = () => _service.Get(request);

        // Assert
        shouldThrow.Should()
            .Throw<ArgumentException>()
            .WithMessage("Guid can't be empty");
        
        _userRepositoryMock.Verify(m => m.GetById(It.IsAny<Guid>()), Times.Never);
    }
    
    [Test]
    public void GetUserById_UserDoesNotExist_Error()
    {
        // Arrange
        var id = Guid.NewGuid();
        
        var request = new GetByIdRequest(id);

        _userRepositoryMock.Setup(m => m.GetById(It.IsAny<Guid>())).Returns((User)null);
        
        // Act
        Action shouldThrow = () => _service.Get(request);

        // Assert
        shouldThrow.Should()
            .Throw<UserNotFoundException>()
            .WithMessage($"User with Id {id} was not found");
    }
}
