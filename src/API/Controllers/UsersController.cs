using CleanApi.Application.Common.Models;
using CleanApi.Application.User.Commands.CreateUser;
using CleanApi.Application.User.Queries.GetCurrent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanApi.API.Controllers;

public class UsersController : ApiControllerBase
{
    // /// <summary>
    // /// Create user
    // /// </summary>
    [HttpPost]
    public async Task<ActionResult<ServiceResult<CreateUserCommandResponse>>> CreateUser(
        CreateUserCommandRequest request)
    {
        return await Mediator.Send(request);
    }
    
    // /// <summary>
    // /// Get current user
    // /// </summary>
    // /// <remarks>Get current user information According to JWT</remarks>
    [HttpGet("CurrentUser")]
    public async Task<ActionResult<ServiceResult<GetCurrentCommandResponse>>> GetCurrentUser()
    {
        return await Mediator.Send(new GetCurrentCommandRequest());
    }
}