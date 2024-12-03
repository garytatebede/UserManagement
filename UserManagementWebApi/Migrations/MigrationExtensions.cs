using System.Reflection;

namespace UserManagementWebApi.Migrations;

internal static class MigrationExtensions
{
    public static IServiceCollection RegisterAllMigrationScripts(this IServiceCollection services)
    {
        var migrationScriptTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(type => type.IsClass && !type.IsAbstract && typeof(IMigrationsScript).IsAssignableFrom(type));

        foreach (var implementationType in migrationScriptTypes)
        {
            var interfaceType = implementationType.GetInterfaces().FirstOrDefault(i => i == typeof(IMigrationsScript));

            if (interfaceType != null)
            {
                services.AddSingleton(interfaceType, implementationType);
            }
        }

        return services;
    }
}
