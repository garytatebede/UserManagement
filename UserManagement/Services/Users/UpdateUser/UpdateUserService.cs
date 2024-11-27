using UserManagement.Repositories.Users;

namespace UserManagement.Services.Users.UpdateUser;

public sealed class UpdateUserService : IUpdateUserService
{
    private readonly IUserRepository _userRepository;

    public UpdateUserService(
        IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public User Update(UpdateUserRequest request)
    {
        ValidateRequest(request);

        if (!_userRepository.Exists(request.Id))
        {
            throw new UserNotFoundException(request.Id);
        }
        
        if (_userRepository.ExistsByUsername(request.Username))
        {
            throw new UserExistsException(request.Username);
        }
        
        var user = _userRepository.GetById(request.Id);

        var updatedUser = user with { Username = request.Username };
        
        _userRepository.Update(updatedUser);

        return updatedUser;
    }

    private static void ValidateRequest(UpdateUserRequest request)
    {
        if (request.Id == Guid.Empty)
        {
            throw new ArgumentException("Guid can't be empty");
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(request.Username, nameof(request.Username));
    }
}