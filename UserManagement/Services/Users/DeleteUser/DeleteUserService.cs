using UserManagement.Repositories.Users;

namespace UserManagement.Services.Users.DeleteUser;

public sealed class DeleteUserService : IDeleteUserService
{
    private readonly IUserRepository _userRepository;

    public DeleteUserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public void Delete(DeleteByIdRequest request)
    {
        Validate(request);

        if (!_userRepository.Exists(request.Id))
        {
            throw new UserNotFoundException(request.Id);
        }

        _userRepository.Delete(request.Id);
    }

    private static void Validate(DeleteByIdRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.Id == Guid.Empty)
        {
            throw new ArgumentException("Guid can't be empty");
        }
    }
}
