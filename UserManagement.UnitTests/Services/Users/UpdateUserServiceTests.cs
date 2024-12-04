using FluentAssertions;
using Moq;
using UserManagementWebApi.Repositories.Users;
using UserManagementWebApi.Services.Users;
using UserManagementWebApi.Services.Users.UpdateUser;

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

    [TestCase("Old Name", "Old Name")]
    [TestCase("Old Name", "old Name")]
    public void UpdateUser_UsernameStaysSame_Success(string oldName, string newName)
    {
        // Arrange
        var id = Guid.NewGuid();

        _userRepositoryMock.Setup(m => m.GetById(id)).Returns(new User(id, oldName));
        _userRepositoryMock.Setup(m => m.ExistsByUsername(oldName)).Returns(true);

        var request = new UpdateUserRequest(id, newName);

        // Act
        var updated = _service.Update(request);

        // Assert
        updated.Id.Should().Be(id);
        updated.Username.Should().Be(oldName);

        _userRepositoryMock.Verify(m => m.GetById(id), Times.Once);
        _userRepositoryMock.Verify(m => m.ExistsByUsername(oldName), Times.Never);
        _userRepositoryMock.Verify(m => m.Update(It.IsAny<User>()), Times.Never);
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
        var username = "Old";
        var newUsername = "New";

        _userRepositoryMock.Setup(m => m.GetById(id)).Returns(new User(id, username));
        _userRepositoryMock.Setup(m => m.ExistsByUsername(newUsername)).Returns(true);

        var request = new UpdateUserRequest(id, newUsername);

        // Act
        Action shouldThrow = () => _service.Update(request);

        // Assert
        shouldThrow.Should()
            .Throw<UserExistsException>()
            .WithMessage($"User with name {newUsername} already exists");

        _userRepositoryMock.Verify(m => m.ExistsByUsername(newUsername), Times.Once);
        _userRepositoryMock.Verify(m => m.GetById(id), Times.Once);
    }

}
