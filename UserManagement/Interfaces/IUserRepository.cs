using UserManagement.Models;

namespace UserManagement.Interfaces;

public interface IUserRepository
{
    public void Create(User user);
    public User Get(string username);
    public IEnumerable<User> GetAll();
    public bool Delete(string username);
}