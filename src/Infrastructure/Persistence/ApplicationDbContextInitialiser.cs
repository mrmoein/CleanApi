using CleanApi.Domain.Entities;
using CleanApi.Domain.Enums;
using CleanApi.Infrastructure.Identity;
using CleanApi.Infrastructure.Persistence.SeedModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanApi.Infrastructure.Persistence;

public class ApplicationDbContextInitialiser
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger,
        ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            if (_context.Database.IsSqlServer())
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    private void AddUserInfo(UserInfo userInfo)
    {
        _context.UserInfos.Add(userInfo);
    }

    private async Task AddIdentityUserAsync(ApplicationUser userAccount, string password, string role)
    {
        await _userManager.CreateAsync(userAccount, password);
        await _userManager.AddToRolesAsync(userAccount, new[] { role });
    }

    private async Task AddRoleAsync(IdentityRole role)
    {
        if (!_roleManager.Roles.Any(r => r.Name == role.Name))
        {
            await _roleManager.CreateAsync(role);
        }
    }

    public async Task TrySeedAsync()
    {
        if (!_roleManager.Roles.Any())
        {
            await AddRoleAsync(new IdentityRole(Roles.Admin.ToString()));
            await AddRoleAsync(new IdentityRole(Roles.User.ToString()));
        }

        if (!_userManager.Users.Any())
        {
            const string sharedPassword = "Test@123";

            await AddIdentityUserAsync(IdentityUserSeedModel.Admin, sharedPassword, Roles.Admin.ToString());
            await AddIdentityUserAsync(IdentityUserSeedModel.User, sharedPassword, Roles.User.ToString());
        }

        if (!_context.UserInfos.Any())
        {
            AddUserInfo(UserInfoSeedModel.Admin);
            AddUserInfo(UserInfoSeedModel.User);
        }

        await _context.SaveChangesAsync();
    }
}