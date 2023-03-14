using FluentValidation;
using CleanApi.Application.Common.Interfaces;

namespace CleanApi.Application.User.Queries.GetCurrent;

public class GetCurrentCommandValidator : AbstractValidator<GetCurrentCommandRequest>
{
    public GetCurrentCommandValidator()
    {
    }
}