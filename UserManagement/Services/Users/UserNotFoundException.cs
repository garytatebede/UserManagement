namespace UserManagement.Services.Users;

public sealed class UserNotFoundException : Exception
{
    public UserNotFoundException(Guid id) : base($"User with Id {id} was not found")
    {
        Id = id;
    }

    public Guid Id { get; }
}


