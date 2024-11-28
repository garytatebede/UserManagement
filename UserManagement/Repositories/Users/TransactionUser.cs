namespace UserManagement.Repositories.Users;

public record TransactionUser
{
    public Guid Id { get; init; }

    public string Username { get; init; }
}