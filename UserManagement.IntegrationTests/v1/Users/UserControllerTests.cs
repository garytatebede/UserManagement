using FluentAssertions;
using UserManagementWebApi.Controllers.v1.Users;

namespace UserManagement.IntegrationTests.v1.Users;

[TestFixture]
public class UserControllerTests : BaseSetup
{
    [Test]
    public async Task UserController_Should_Crud_Success()
    {
        // Create User
        var createUserRequest = new CreateUserApiRequest
        {
            Username = "AnthVestBede",
        };

        var createdUserResponse = await CreateUser(createUserRequest);

        createdUserResponse.Should().NotBe(null);
        createdUserResponse.Id.Should().NotBe(Guid.Empty);
        createdUserResponse.Username.Should().Be(createUserRequest.Username);

        // Get User by id
        var getUserResponse = await GetUserById(createdUserResponse.Id);
        getUserResponse.Should().NotBe(null);
        getUserResponse.Id.Should().Be(createdUserResponse.Id);
        getUserResponse.Username.Should().Be(createdUserResponse.Username);

        // Update user
        var updatedUserRequest = new UpdateUserApiRequest
        {
            Username = "Changed"
        };

        await UpdateUser(getUserResponse.Id, updatedUserRequest);

        var getUpdatedUserResponse = await GetUserById(createdUserResponse.Id);
        getUpdatedUserResponse.Should().NotBe(null);
        getUpdatedUserResponse.Username.Should().Be(updatedUserRequest.Username);

        await DeleteUser(getUserResponse.Id);

        Action shouldThrow = () => GetUserById(getUserResponse.Id).GetAwaiter().GetResult();

        shouldThrow.Should()
          .Throw<Exception>()
          .WithMessage("NotFound");
    }
}
