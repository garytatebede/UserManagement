using UserManagement.Services.Users;

namespace UserManagement.Repositories.Users.Mappers;

public sealed class UserModelMapper : IUserModelMapper
{
    public TransactionUser ToTransactionUser(User user)
    {
        return new TransactionUser
        {
            Id = user.Id,
            Username = user.Username
        };
    }

    public User ToUser(TransactionUser transactionUser)
    {
        return new User(transactionUser.Id, transactionUser.Username);
    }
}