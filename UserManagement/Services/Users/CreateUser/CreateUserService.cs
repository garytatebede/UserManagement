using UserManagement.Repositories.Users;

namespace UserManagement.Services.Users.CreateUser;

public sealed class CreateUserService : ICreateUserService
{
    private readonly IGuidService _guidService;
    private readonly IUserRepository _userRepository;

    public CreateUserService(
        IGuidService guidService,
        IUserRepository userRepository)
    {
        _guidService = guidService;
        _userRepository = userRepository;
    }

    public User Create(CreateUserRequest request)
    {
        ValidateRequest(request);

        if (_userRepository.ExistsByUsername(request.Username))
        {
            throw new UserExistsException(request.Username);
        }

        var id = _guidService.New();

        var user = new User(id, request.Username);

        _userRepository.Create(user);

        return user;
    }

    private static void ValidateRequest(CreateUserRequest request)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(request.Username, nameof(request.Username));
    }
}