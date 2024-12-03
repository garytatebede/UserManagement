namespace UserManagementWebApi.Services.Users.CreateUser;

public interface ICreateUserService
{
    User Create(CreateUserRequest request);
}
