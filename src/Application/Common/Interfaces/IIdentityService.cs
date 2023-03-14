using CleanApi.Application.Common.Models;
using CleanApi.Domain.Entities;

namespace CleanApi.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<string> GetUserNameAsync(string userId);

    Task<bool> IsInRoleAsync(string userId, string role);
    
    Task<IList<string>> GetUserRoleAsync(string userId);

    Task<bool> AuthorizeAsync(string userId, string policyName);
    
    Task<bool> CheckPasswordAsync(string userId, string password);

    Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password);

    Task<Result> DeleteUserAsync(string userId);
}
