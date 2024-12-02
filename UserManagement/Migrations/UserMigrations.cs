using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using UserManagement.Configuration;

namespace UserManagement.Migrations;

internal class UserMigrations : IMigrationsScript
{
    private readonly IOptions<DatabaseConfiguration> _options;

    public UserMigrations(IOptions<DatabaseConfiguration> options)
    {
        _options = options;
    }

    public int PriorityOrderToRun => 1;

    public async Task InitializeAsync()
    {
        var createUsersTable = @"

        IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Users' AND xtype='U')
        BEGIN
            CREATE TABLE Users (
                Id UNIQUEIDENTIFIER PRIMARY KEY,
                Username NVARCHAR(255) NOT NULL,
                CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
            );
        END

        IF NOT EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'UQ_Username') AND parent_object_id = OBJECT_ID(N'Users'))
        ALTER TABLE Users ADD CONSTRAINT UQ_Username UNIQUE (Username);
     ";

        using var connection = new SqlConnection(_options.Value.ConnectionString);

        await connection.OpenAsync();

        using var command = new SqlCommand(createUsersTable, connection);

        await command.ExecuteNonQueryAsync();
    }
}
