using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UserManagement.Configuration;
using UserManagement.Controllers;
using UserManagement.Migrations;
using UserManagement.Repositories.Users;
using UserManagement.Repositories.Users.Mappers;
using UserManagement.Services;
using UserManagement.Services.Users.CreateUser;
using UserManagement.Services.Users.DeleteUser;
using UserManagement.Services.Users.GetUserById;
using UserManagement.Services.Users.UpdateUser;

namespace UserManagement;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.Configure<DatabaseConfiguration>(builder.Configuration.GetSection("Database"));

        // Services
        builder.Services.AddScoped<ICreateUserService, CreateUserService>();
        builder.Services.AddScoped<IDeleteUserService, DeleteUserService>();
        builder.Services.AddScoped<IGetUserByIdService, GetUserByIdService>();
        builder.Services.AddScoped<IUpdateUserService, UpdateUserService>();
        builder.Services.AddScoped<IGuidService, GuidService>();

        // Repos
        builder.Services.AddSingleton<IUserRepository, UserRepository>();
        builder.Services.AddSingleton<IUserModelMapper, UserModelMapper>();

        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

        builder.Services.AddProblemDetails();

        // Migration scripts
        builder.Services.RegisterAllMigrationScripts();

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var scripts = scope.ServiceProvider.GetServices<IMigrationsScript>()
                .OrderBy(s => s.PriorityOrderToRun);

            foreach (var databaseSetup in scripts)
            {
                databaseSetup.InitializeAsync().GetAwaiter().GetResult();
            }
        }

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseExceptionHandler();

        app.UseHttpsRedirection();

        app.MapControllers();

        app.Run(); // Start the server
    }
}