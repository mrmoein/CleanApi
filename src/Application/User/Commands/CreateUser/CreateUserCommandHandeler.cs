using CleanApi.Application.Common.Exceptions;
using CleanApi.Application.Common.Interfaces;
using CleanApi.Application.Common.Models;
using CleanApi.Domain.Entities;
using MediatR;

namespace CleanApi.Application.User.Commands.CreateUser;

public class
    CreateUserCommandHandler : IRequestHandler<CreateUserCommandRequest, ServiceResult<CreateUserCommandResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;
    private readonly ITokenService _tokenService;

    public CreateUserCommandHandler(IApplicationDbContext context, ITokenService tokenService,
        IIdentityService identityService)
    {
        _context = context;
        _tokenService = tokenService;
        _identityService = identityService;
    }

    public async Task<ServiceResult<CreateUserCommandResponse>> Handle(CreateUserCommandRequest request,
        CancellationToken cancellationToken)
    {
        (Result identityResult, string userId) =
            await _identityService.CreateUserAsync(request.UserName, request.Password);

        if (!identityResult.Succeeded)
        {
            throw new ValidationException(identityResult.Errors);
        }

        UserInfo userInfo = new UserInfo
        {
            Id = userId, UserName = request.UserName, FirstName = request.FirstName, LastName = request.LastName
        };

        _context.UserInfos.Add(userInfo);

        await _context.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(new CreateUserCommandResponse { UserId = userId });
    }
}