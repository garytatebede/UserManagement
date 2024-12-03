using Microsoft.AspNetCore.Mvc;
using UserManagement.Services.Users.CreateUser;
using UserManagement.Services.Users.DeleteUser;
using UserManagement.Services.Users.GetUserById;
using UserManagement.Services.Users.UpdateUser;

namespace UserManagement.Controllers.v1.Users;

[ApiController]
[Route("api/v1/users")]
public class UserController : ControllerBase
{
    private readonly ICreateUserService _createUserService;
    private readonly IGetUserByIdService _getUserByIdService;
    private readonly IDeleteUserService _deleteUserService;
    private readonly IUpdateUserService _updateUserService;

    public UserController(
        ICreateUserService createUserService,
        IGetUserByIdService getUserByIdService,
        IDeleteUserService deleteUserService,
        IUpdateUserService updateUserService)
    {
        _createUserService = createUserService;
        _getUserByIdService = getUserByIdService;
        _deleteUserService = deleteUserService;
        _updateUserService = updateUserService;
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateUserApiRequest request)
    {
        var user = _createUserService.Create(request.ToServiceRequest());

        return Created(nameof(Create), user);
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

    [HttpPut("{id:guid}")]
    public IActionResult Update(Guid id, [FromBody] UpdateUserApiRequest request)
    {
        var user = _updateUserService.Update(request.ToServiceRequest(id));

        return Ok(user);
    }
}