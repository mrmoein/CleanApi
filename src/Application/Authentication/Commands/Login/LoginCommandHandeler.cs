using CleanApi.Application.Common.Interfaces;
using CleanApi.Application.Common.Models;
using CleanApi.Domain.Entities;
using MediatR;

namespace CleanApi.Application.Authentication.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommandRequest, ServiceResult<LoginCommandResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;
    private readonly ITokenService _tokenService;

    public LoginCommandHandler(IApplicationDbContext context, ITokenService tokenService,
        IIdentityService identityService)
    {
        _context = context;
        _tokenService = tokenService;
        _identityService = identityService;
    }

    public async Task<ServiceResult<LoginCommandResponse>> Handle(LoginCommandRequest request,
        CancellationToken cancellationToken)
    {
        UserInfo? userInfo = _context.UserInfos.FirstOrDefault(u => u.UserName == request.UserName);

        if (userInfo is null)
        {
            return ServiceResult.Failed<LoginCommandResponse>(ServiceError.UserNotFound);
        }

        if (!await _identityService.CheckPasswordAsync(userInfo.Id, request.Password))
        {
            return ServiceResult.Failed<LoginCommandResponse>(
                ServiceError.CustomMessage("UserName or Password is invalid"));
        }

        string token = _tokenService.CreateJwtSecurityToken(userInfo.Id) ?? string.Empty;

        return ServiceResult.Success(new LoginCommandResponse { Token = token });
    }
}