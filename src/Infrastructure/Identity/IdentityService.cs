using System.Security.Claims;
using AutoMapper;
using CleanApi.Application.Common.Exceptions;
using CleanApi.Application.Common.Interfaces;
using CleanApi.Application.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace CleanApi.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly IAuthorizationService _authorizationService;
    private readonly IMapper _mapper;
    private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
    private readonly UserManager<ApplicationUser> _userManager;

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
        IAuthorizationService authorizationService,
        IMapper mapper)
    {
        _userManager = userManager;
        _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        _authorizationService = authorizationService;
        _mapper = mapper;
    }

    public async Task<string> GetUserNameAsync(string userId)
    {
        ApplicationUser user = await GetUser(userId);

        return user.UserName;
    }

    public async Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password)
    {
        ApplicationUser user = new ApplicationUser { UserName = userName, Email = userName };

        IdentityResult? result = await _userManager.CreateAsync(user, password);

        return (result.ToApplicationResult(), user.Id);
    }

    public async Task<bool> IsInRoleAsync(string userId, string role)
    {
        ApplicationUser user = await GetUser(userId);

        return await _userManager.IsInRoleAsync(user, role);
    }

    public async Task<IList<string>> GetUserRoleAsync(string userId)
    {
        ApplicationUser user = await GetUser(userId);
        return await _userManager.GetRolesAsync(user);
    }

    public async Task<bool> CheckPasswordAsync(string userId, string password)
    {
        ApplicationUser user = await GetUser(userId);
        return await _userManager.CheckPasswordAsync(user, password);
    }

    public async Task<bool> AuthorizeAsync(string userId, string policyName)
    {
        ApplicationUser? user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        if (user == null)
        {
            return false;
        }

        ClaimsPrincipal? principal = await _userClaimsPrincipalFactory.CreateAsync(user);

        AuthorizationResult result = await _authorizationService.AuthorizeAsync(principal, policyName);

        return result.Succeeded;
    }

    public async Task<Result> DeleteUserAsync(string userId)
    {
        ApplicationUser? user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        return user != null ? await DeleteUserAsync(user) : Result.Success();
    }

    private async Task<Result> DeleteUserAsync(ApplicationUser userIdentity)
    {
        IdentityResult? result = await _userManager.DeleteAsync(userIdentity);

        return result.ToApplicationResult();
    }

    private async Task<ApplicationUser> GetUser(string userId)
    {
        ApplicationUser? user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            throw new NotFoundException(ServiceError.UserNotFound.Message);
        }

        return user;
    }
}