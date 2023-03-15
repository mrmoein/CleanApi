using AutoMapper;
using CleanApi.Application.Common.Interfaces;
using CleanApi.Application.Common.Models;
using CleanApi.Application.Common.Security;
using CleanApi.Domain.Entities;
using MediatR;

namespace CleanApi.Application.User.Queries.GetCurrent;

[Authorize(Roles = "Admin,User")]
public class
    GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQueryRequest, ServiceResult<GetCurrentUserQueryResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IIdentityService _identityService;
    private readonly IMapper _mapper;
    private readonly ITokenService _tokenService;

    public GetCurrentUserQueryHandler(IApplicationDbContext context, ITokenService tokenService,
        IIdentityService identityService, ICurrentUserService currentUserService, IMapper mapper)
    {
        _context = context;
        _tokenService = tokenService;
        _identityService = identityService;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<ServiceResult<GetCurrentUserQueryResponse>> Handle(GetCurrentUserQueryRequest request,
        CancellationToken cancellationToken)
    {
        string? currentUserId = _currentUserService.UserId;

        if (string.IsNullOrEmpty(currentUserId))
        {
            throw new UnauthorizedAccessException();
        }

        IList<string> roles = await _identityService.GetUserRoleAsync(currentUserId);
        UserInfo? userInfo = _context.UserInfos.FirstOrDefault(u => u.Id == currentUserId);

        GetCurrentUserQueryResponse? result = _mapper.Map<GetCurrentUserQueryResponse>(userInfo);
        result.Roles = roles;

        return ServiceResult.Success(result);
    }
}