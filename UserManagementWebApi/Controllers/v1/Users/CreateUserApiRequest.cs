using UserManagementWebApi.Services.Users.CreateUser;

namespace UserManagementWebApi.Controllers.v1.Users;

public class CreateUserApiRequest
{
    public string Username { get; set; }

    public CreateUserRequest ToServiceRequest()
    {
        return new CreateUserRequest(Username);
    }
}
