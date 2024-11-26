using Microsoft.AspNetCore.Mvc;
using UserManagement.Services.Users.CreateUser;
using UserManagement.Services.Users.DeleteUser;
using UserManagement.Services.Users.GetUserById;

namespace UserManagement.Controllers.v1.Users;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly ICreateUserService _createUserService;
    private readonly IGetUserByIdService _getUserByIdService;
    private readonly IDeleteUserService _deleteUserService;

    public UserController(
        ICreateUserService createUserService,
        IGetUserByIdService getUserByIdService,
        IDeleteUserService deleteUserService)
    {
        _createUserService = createUserService;
        _getUserByIdService = getUserByIdService;
        _deleteUserService = deleteUserService;
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateUserApiRequest request)
    {
        var user = _createUserService.Create(request.ToServiceRequest());

        return Ok(user);
    }

    [HttpGet("{id:guid}")]
    public IActionResult Get(Guid id)
    {
        var user = _getUserByIdService.Get(new GetByIdRequest(id));

        return Ok(user);
    }


    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        _deleteUserService.Delete(new DeleteByIdRequest(id));

        return NoContent();
    }
}