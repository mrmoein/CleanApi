using CleanApi.Domain.Entities;
using CleanApi.Infrastructure.Identity;

namespace CleanApi.Infrastructure.Persistence.SeedModel;

public static class IdentityUserSeedModel
{
    public static readonly CustomIdentityUser Admin = new()
    {
        Id = Guid.NewGuid().ToString(),
        UserName = "admin@localhost",
        Email = "admin@localhost"
    };
    
    public static readonly CustomIdentityUser User = new()
    {
        Id = Guid.NewGuid().ToString(),
        UserName = "user@localhost",
        Email = "user@localhost"
    };
}