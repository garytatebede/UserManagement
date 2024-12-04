namespace UserManagementWebApi.Services.Users;

public sealed class UserExistsException : Exception
{
    public UserExistsException(string username) : base($"User with name {username} already exists")
    {
        Username = username;
    }

    public string Username { get; }
}

