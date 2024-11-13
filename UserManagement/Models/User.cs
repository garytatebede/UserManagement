using UserManagement.Interfaces;

namespace UserManagement.Models;

public class User : IUser
{
    public string Username { get; set; }
    public string Email { get; set; }
}