using AutoMapper;
using CleanApi.Application.Common.Exceptions;
using CleanApi.Application.Common.Interfaces;
using CleanApi.Application.Common.Models;
using CleanApi.Application.Common.Security;
using CleanApi.Domain.Entities;
using MediatR;

namespace CleanApi.Application.User.Queries.GetCurrent;

[Authorize(Roles = "Admin,User")]
public class
    GetCurrentCommandHandler : IRequestHandler<GetCurrentCommandRequest, ServiceResult<GetCurrentCommandResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly ITokenService _tokenService;
    private readonly IIdentityService _identityService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public GetCurrentCommandHandler(IApplicationDbContext context, ITokenService tokenService,
        IIdentityService identityService, ICurrentUserService currentUserService, IMapper mapper)
    {
        _context = context;
        _tokenService = tokenService;
        _identityService = identityService;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<ServiceResult<GetCurrentCommandResponse>> Handle(GetCurrentCommandRequest request,
        CancellationToken cancellationToken)
    {
        string? currentUserId = _currentUserService.UserId;

        if (string.IsNullOrEmpty(currentUserId))
        {
            throw new UnauthorizedAccessException();
        }

        var roles = await _identityService.GetUserRoleAsync(currentUserId);
        var userInfo = _context.UserInfos.FirstOrDefault(u => u.Id == currentUserId);

        var result = _mapper.Map<GetCurrentCommandResponse>(userInfo);
        result.Roles = roles;

        return ServiceResult.Success(result);
    }
}