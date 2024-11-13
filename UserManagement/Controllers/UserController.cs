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
    public IActionResult Create(string username, string email)
    {
        var user = new User
        {
            Username = username,
            Email = email
        };

        _userRepository.Create(user);

        return Ok(_userRepository.GetAll());
    }
    
    [HttpGet]
    public IActionResult Get(string username)
    {
        var user = _userRepository.Get(username);
        
        return Ok(user);
    }
    
    [HttpGet("all")]
    public IActionResult GetAll()
    {
        var users = _userRepository.GetAll();
        
        return Ok(users);
    }

    [HttpDelete]
    public IActionResult Delete(string username)
    {
        var bDeleted = _userRepository.Delete(username);

        if (!bDeleted)
        {
            return NotFound("User not found");
        }
        
        return Ok($"{username} deleted");
    }
}