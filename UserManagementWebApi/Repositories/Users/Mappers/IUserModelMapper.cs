using UserManagementWebApi.Services.Users;

namespace UserManagementWebApi.Repositories.Users.Mappers;

public interface IUserModelMapper
{
    User ToUser(TransactionUser transactionUser);

    TransactionUser ToTransactionUser(User user);
}
