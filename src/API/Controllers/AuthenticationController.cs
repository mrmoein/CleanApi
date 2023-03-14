using CleanApi.Application.Authentication.Commands.Login;
using CleanApi.Application.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace CleanApi.API.Controllers;

public class AuthenticationController : ApiControllerBase
{
    // /// <summary>
    // /// Login
    // /// </summary>
    // /// <remarks>Create Login JWT</remarks>
    [HttpPost]
    public async Task<ActionResult<ServiceResult<LoginCommandResponse>>> Login(LoginCommandRequest request)
    {
        return await Mediator.Send(request);
    }
}