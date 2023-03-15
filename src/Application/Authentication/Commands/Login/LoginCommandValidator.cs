using FluentValidation;

namespace CleanApi.Application.Authentication.Commands.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommandRequest>
{
    public LoginCommandValidator()
    {
        RuleFor(v => v.UserName).NotEmpty().NotNull();
        RuleFor(v => v.Password).NotEmpty().NotNull();
    }
}