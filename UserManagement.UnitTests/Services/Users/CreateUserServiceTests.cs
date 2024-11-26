using FluentAssertions;
using Moq;
using UserManagement.Repositories.Users;
using UserManagement.Services;
using UserManagement.Services.Users;
using UserManagement.Services.Users.CreateUser;

namespace UserManagement.UnitTests.Services.Users;

[TestFixture]
public class CreateUserServiceTests
{
    private Mock<IGuidService> _guidServiceMock;
    private Mock<IUserRepository> _userRepositoryMock;

    private CreateUserService _service;

    [SetUp]
    public void BeforeEach()
    {
        _guidServiceMock = new Mock<IGuidService>();
        _userRepositoryMock = new Mock<IUserRepository>();

        _service = new CreateUserService(_guidServiceMock.Object, _userRepositoryMock.Object);
    }

    [Test]
    public void CreateUser_Success()
    {
        // Arrange
        var id = Guid.NewGuid();
        var username = "Gary Tate";

        _guidServiceMock.Setup(m => m.New()).Returns(id);
        _userRepositoryMock.Setup(m => m.ExistsByUsername(username)).Returns(false);
        var request = new CreateUserRequest(username);

        // Act
        var createdUser = _service.Create(request);

        // Assert
        createdUser.Id.Should().Be(id);
        createdUser.Username.Should().Be(username);

        _guidServiceMock.Verify(m => m.New(), Times.Once);
        _userRepositoryMock.Verify(m => m.ExistsByUsername(username), Times.Once);
    }

    [TestCase("")]
    [TestCase(null)]
    [TestCase("   ")]
    public void CreateUser_BadRquestWithUsername(string username)
    {
        // Arrange
        var request = new CreateUserRequest(username);

        // Act
        Action shouldThrow = () => _service.Create(request);

        // Assert
        shouldThrow.Should().Throw<ArgumentException>();

        _guidServiceMock.Verify(m => m.New(), Times.Never);
        _userRepositoryMock.Verify(m => m.ExistsByUsername(username), Times.Never);
    }

    [Test]
    public void CreateUser_AlreadyExists_ExpectError()
    {
        // Arrange
        var id = Guid.NewGuid();
        var username = "Gary Tate";

        _guidServiceMock.Setup(m => m.New()).Returns(id);
        _userRepositoryMock.Setup(m => m.ExistsByUsername(username)).Returns(true);
        var request = new CreateUserRequest(username);

        // Act
        Action shouldThrow = () => _service.Create(request);

        // Assert
        shouldThrow.Should()
            .Throw<UserExistsException>()
            .WithMessage($"User with name {username} already exists");
    }
}
