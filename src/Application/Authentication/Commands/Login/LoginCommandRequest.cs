using CleanApi.Application.Common.Models;
using MediatR;

namespace CleanApi.Application.Authentication.Commands.Login;

public class LoginCommandRequest: IRequest<ServiceResult<LoginCommandResponse>>
{
    public string UserName { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}