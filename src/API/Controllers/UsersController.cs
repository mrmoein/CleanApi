using CleanApi.Application.Common.Models;
using CleanApi.Application.User.Commands.RegisterUser;
using Microsoft.AspNetCore.Mvc;

namespace CleanApi.API.Controllers;

public class UsersController : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ServiceResult<RegisterUserCommandResponse>>> RegisterUser(RegisterUserCommandRequest request)
    {
        return await Mediator.Send(request);
    }
}