using FluentValidation;
using CleanApi.Application.Common.Interfaces;

namespace CleanApi.Application.User.Commands.CreateUser;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommandRequest>
{
    public CreateUserCommandValidator()
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