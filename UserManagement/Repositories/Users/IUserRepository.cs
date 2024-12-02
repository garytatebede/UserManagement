using UserManagement.Services.Users;

namespace UserManagement.Repositories.Users;

public interface IUserRepository
{
    void Create(User user);

    User? GetByUsername(string username);

    bool ExistsByUsername(string username);

    bool Exists(Guid id);

    User? GetById(Guid id);

    bool Delete(Guid id);

    void Update(User user);
}