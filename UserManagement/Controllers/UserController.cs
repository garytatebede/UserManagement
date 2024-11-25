using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Interfaces;
using UserManagement.Models;
using UserManagement.Repositories;

namespace UserManagement.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private IUserRepository _userRepository;

    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    [HttpPost]
    public IActionResult Create(string username)
    {
        // Validate that it is a unique Username
        if (_userRepository.Get(username) != null)
        {
            return BadRequest($"A user called '{username}' already exists");
        }
        
        var user = new User
        {
            Username = username,
            Id = Guid.NewGuid()
        };

        _userRepository.Create(user);

        return Ok(user);
    }
    
    [HttpGet("{id:guid?}")]
    public IActionResult Get(Guid? id)
    {
        if (!id.HasValue)
        {
            return Ok(_userRepository.GetAll());
        }

        var user = _userRepository.Get(id);

        // Seems to auto add the NotFound is user is null
        return Ok(user);
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        var bDeleted = _userRepository.Delete(id);

        if (!bDeleted)
        {
            return NotFound("User not found");
        }
        
        return Ok($"{id} deleted");
    }
}