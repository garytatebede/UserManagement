using UserManagement.Models;

namespace UserManagement.Interfaces;

public interface IUserRepository
{
    public void Create(User user);
    public User Get(string username);
    public User Get(Guid? id);
    public IEnumerable<User> GetAll();
    public bool Delete(Guid? id);
}