using System.ComponentModel.DataAnnotations;
using CleanApi.Application.Common.Models;
using MediatR;

namespace CleanApi.Application.User.Commands.CreateUser;

public record CreateUserCommandRequest : IRequest<ServiceResult<CreateUserCommandResponse>>
{
    [DataType(DataType.EmailAddress)] public string UserName { get; init; } = string.Empty;

    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string ConfirmPassword { get; init; } = string.Empty;
}