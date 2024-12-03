namespace UserManagementWebApi.Services.Users.GetUserById;

public interface IGetUserByIdService
{
    User Get(GetByIdRequest request);
}

