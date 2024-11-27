using UserManagement.Repositories.Users.Mappers;
using UserManagement.Services.Users;

namespace UserManagement.Repositories.Users;

public class UserRepository : IUserRepository
{
    private readonly Dictionary<Guid, TransactionUser> _users;
    private readonly IUserModelMapper _userModelMapper;

    public UserRepository(IUserModelMapper userModelMapper)
    {
        _users = [];
        _userModelMapper = userModelMapper;
    }

    public void Create(User user)
    {
        var transactionUser = _userModelMapper.ToTransactionUser(user);

        _users.Add(transactionUser.Id, transactionUser);
    }

    public User? GetByUsername(string username)
    {
        var record = _users.Values.FirstOrDefault(x => x.Username.Equals(username));

        if (record == null)
        {
            return null;
        }

        return _userModelMapper.ToUser(record);
    }

    public User? GetById(Guid id)
    {
        if (!_users.TryGetValue(id, out var record))
        {
            return null;
        }

        return _userModelMapper.ToUser(record);
    }

    public bool ExistsByUsername(string username)
    {
        return _users.Values.Any(x => x.Username.Equals(username));
    }

    public bool Exists(Guid id)
    {
        return _users.ContainsKey(id);
    }

    public IEnumerable<User> GetAll()
    {
        return _users.Values.Select(_userModelMapper.ToUser);

        // Above is the short handed version of
        //return _users.Values.Select(u =>_userModelMapper.ToUser(u));
    }

    public bool Delete(Guid id)
    {
        return _users.Remove(id);
    }
    
    public void Update(User user)
    {
        var transactionUser = _userModelMapper.ToTransactionUser(user);
        
        _users[user.Id] = transactionUser;
    }
}