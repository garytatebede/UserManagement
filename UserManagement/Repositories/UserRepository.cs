using UserManagement.Interfaces;
using UserManagement.Models;

namespace UserManagement.Repositories;

public class UserRepository : IUserRepository
{
    private List<User> _users = [];

    public void Create(User user)
    {
        _users.Add(user);
    }

    public User Get(string username)
    {
        return _users.Find(x => x.Username == username);
    }
    
    public User Get(Guid? id)
    {
        return _users.Find(x => x.Id == id);
    }
    
    public IEnumerable<User> GetAll()
    {
        return _users;
    }

    public bool Delete(Guid? id)
    {
       return _users.Remove(Get(id)); 
    }
}