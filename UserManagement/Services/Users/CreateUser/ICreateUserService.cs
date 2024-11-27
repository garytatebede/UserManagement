namespace UserManagement.Services.Users.CreateUser;

public interface ICreateUserService
{
    User Create(CreateUserRequest request);
}
