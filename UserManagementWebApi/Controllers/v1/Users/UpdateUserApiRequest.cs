﻿using UserManagementWebApi.Services.Users.UpdateUser;

namespace UserManagementWebApi.Controllers.v1.Users;

public class UpdateUserApiRequest
{
    public string Username { get; set; }

    public UpdateUserRequest ToServiceRequest(Guid id)
    {
        return new UpdateUserRequest(id, Username);
    }
}
