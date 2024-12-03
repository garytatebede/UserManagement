namespace UserManagementWebApi.Migrations;

public interface IMigrationsScript
{
    int PriorityOrderToRun { get; }

    Task InitializeAsync();
}
