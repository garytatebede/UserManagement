namespace UserManagementWebApi.Services.Users.DeleteUser;

public interface IDeleteUserService
{
    void Delete(DeleteByIdRequest request);
}
