using FluentValidation;
using CleanApi.Application.Common.Interfaces;

namespace CleanApi.Application.User.Commands.RegisterUser;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommandRequest>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(v => v.UserName)
            .EmailAddress()
            .WithMessage("UserName is not a valid email address")
            .NotEmpty().NotNull();
        RuleFor(v => v.FirstName).NotEmpty().NotNull();
        RuleFor(v => v.LastName).NotEmpty().NotNull();
        RuleFor(v => v.Password).NotEmpty().NotNull();
        RuleFor(v => v.ConfirmPassword)
            .Equal(u => u.Password)
            .WithMessage("'Password' and 'Confirm password' are not the same")
            .NotEmpty().NotNull();
    }
}