namespace UserManagement.Services.Users.GetUserById;

public interface IGetUserByIdService
{
    User Get(GetByIdRequest request);
}

