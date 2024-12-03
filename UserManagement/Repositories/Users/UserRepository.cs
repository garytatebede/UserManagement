using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using UserManagement.Configuration;
using UserManagement.Repositories.Users.Mappers;
using UserManagement.Services.Users;

namespace UserManagement.Repositories.Users;

internal sealed class UserRepository : IUserRepository
{
    private readonly IOptions<DatabaseConfiguration> _options;
    private readonly IUserModelMapper _modelMapper;

    public UserRepository(IOptions<DatabaseConfiguration> options, IUserModelMapper modelMapper)
    {
        _options = options;
        _modelMapper = modelMapper;
    }

    public void Create(User user)
    {
        using var connection = GetConnection();

        var transactionModel = _modelMapper.ToTransactionUser(user);

        const string sql = @"
            INSERT INTO Users (Id, Username)
            VALUES (@Id, @Username);";

        connection.Execute(sql, user);
    }

    public User? GetByUsername(string username)
    {
        using var connection = GetConnection();

        const string sql = @"
            SELECT Id, Username
            FROM Users
            WHERE Username = @Username;";

        var transactionModel = connection.QueryFirstOrDefault<TransactionUser>(sql, new { Username = username });

        if (transactionModel is null)
        {
            return null;
        }

        return _modelMapper.ToUser(transactionModel);
    }

    public bool ExistsByUsername(string username)
    {
        using var connection = GetConnection();

        const string sql = @"
            SELECT CASE 
                WHEN EXISTS (
                    SELECT 1
                    FROM Users
                    WHERE Username = @Username
                ) 
                THEN CAST(1 AS BIT) 
                ELSE CAST(0 AS BIT) 
            END;";

        return connection.ExecuteScalar<bool>(sql, new { Username = username });
    }

    public bool Exists(Guid id)
    {
        using var connection = GetConnection();

        const string sql = @"
            SELECT CASE 
            WHEN EXISTS (
                    SELECT 1
                    FROM Users
                    WHERE Id = @Id
                ) 
                THEN CAST(1 AS BIT) 
                ELSE CAST(0 AS BIT) 
            END;";

        return connection.ExecuteScalar<bool>(sql, new { Id = id });
    }

    public User? GetById(Guid id)
    {
        using var connection = GetConnection();

        const string sql = @"
            SELECT Id, Username
            FROM Users
            WHERE Id = @Id;";

        var transactionModel = connection.QueryFirstOrDefault<TransactionUser>(sql, new { Id = id });

        if (transactionModel is null)
        {
            return null;
        }

        return _modelMapper.ToUser(transactionModel);
    }

    public bool Delete(Guid id)
    {
        using var connection = GetConnection();

        const string sql = @"
            DELETE FROM Users
            WHERE Id = @Id;";

        var rowsAffected = connection.Execute(sql, new { Id = id });
        return rowsAffected > 0;
    }

    public void Update(User user)
    {
        using var connection = GetConnection();

        const string sql = @"
            UPDATE Users
            SET Username = @Username
            WHERE Id = @Id;";

        connection.Execute(sql, user);
    }

    private SqlConnection GetConnection()
    {
        return new SqlConnection(_options.Value.ConnectionString);
    }
}
