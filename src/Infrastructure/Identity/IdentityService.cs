using CleanApi.Application.Common.Interfaces;
using CleanApi.Application.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CleanApi.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<CustomIdentityUser> _userManager;
    private readonly IUserClaimsPrincipalFactory<CustomIdentityUser> _userClaimsPrincipalFactory;
    private readonly IAuthorizationService _authorizationService;

    public IdentityService(
        UserManager<CustomIdentityUser> userManager,
        IUserClaimsPrincipalFactory<CustomIdentityUser> userClaimsPrincipalFactory,
        IAuthorizationService authorizationService)
    {
        _userManager = userManager;
        _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        _authorizationService = authorizationService;
    }

    public async Task<string> GetUserNameAsync(string userId)
    {
        var user = await _userManager.Users.FirstAsync(u => u.Id == userId);

        return user.UserName;
    }

    public async Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password)
    {
        var user = new CustomIdentityUser { UserName = userName, Email = userName, };

        var result = await _userManager.CreateAsync(user, password);

        return (result.ToApplicationResult(), user.Id);
    }

    public async Task<bool> IsInRoleAsync(string userId, string role)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        return user != null && await _userManager.IsInRoleAsync(user, role);
    }

    public async Task<bool> CheckPasswordAsync(string userId, string password)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        if (user != null)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        return false;
    }

    public async Task<bool> AuthorizeAsync(string userId, string policyName)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        if (user == null)
        {
            return false;
        }

        var principal = await _userClaimsPrincipalFactory.CreateAsync(user);

        var result = await _authorizationService.AuthorizeAsync(principal, policyName);

        return result.Succeeded;
    }

    public async Task<Result> DeleteUserAsync(string userId)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        return user != null ? await DeleteUserAsync(user) : Result.Success();
    }

    public async Task<Result> DeleteUserAsync(CustomIdentityUser userIdentity)
    {
        var result = await _userManager.DeleteAsync(userIdentity);

        return result.ToApplicationResult();
    }
}