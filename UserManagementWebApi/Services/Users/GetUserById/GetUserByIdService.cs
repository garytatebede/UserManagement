using UserManagementWebApi.Repositories.Users;

namespace UserManagementWebApi.Services.Users.GetUserById;

public sealed class GetUserByIdService : IGetUserByIdService
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public User Get(GetByIdRequest request)
    {
        Validate(request);

        var user = _userRepository.GetById(request.Id);

        if (user is null)
        {
            throw new UserNotFoundException(request.Id);
        }

        return user;
    }

    private static void Validate(GetByIdRequest request)
    {
        if (request.Id == Guid.Empty)
        {
            throw new ArgumentException("Guid can't be empty");
        }
    }
}

