using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using UserManagement.Controllers;
using UserManagement.Repositories.Users;
using UserManagement.Repositories.Users.Mappers;
using UserManagement.Services;
using UserManagement.Services.Users.CreateUser;
using UserManagement.Services.Users.DeleteUser;
using UserManagement.Services.Users.GetUserById;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Services
builder.Services.AddScoped<ICreateUserService, CreateUserService>();
builder.Services.AddScoped<IDeleteUserService, DeleteUserService>();
builder.Services.AddScoped<IGetUserByIdService, GetUserByIdService>();
builder.Services.AddScoped<IGuidService, GuidService>();

// Repos
builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IUserModelMapper, UserModelMapper>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline
app.MapControllers();

app.Run("http://localhost:5000"); // Start the server