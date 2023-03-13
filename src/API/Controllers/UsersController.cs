using CleanApi.Application.Common.Models;
using CleanApi.Application.User.Commands.CreateUser;
using Microsoft.AspNetCore.Mvc;

namespace CleanApi.API.Controllers;

public class UsersController : ApiControllerBase
{

    /// <summary>
    /// Deletes a specific TodoItem.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ServiceResult<CreateUserCommandResponse>>> CreateUser(CreateUserCommandRequest request)
    {
        return await Mediator.Send(request);
    }
}