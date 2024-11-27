using UserManagement.Services.Users;

namespace UserManagement.Repositories.Users.Mappers;

public interface IUserModelMapper
{
    User ToUser(TransactionUser transactionUser);

    TransactionUser ToTransactionUser(User user);
}
