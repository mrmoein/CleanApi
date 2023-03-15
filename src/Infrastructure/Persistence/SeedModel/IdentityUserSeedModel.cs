using CleanApi.Infrastructure.Identity;

namespace CleanApi.Infrastructure.Persistence.SeedModel;

public static class IdentityUserSeedModel
{
    public static readonly ApplicationUser Admin = new()
    {
        Id = "3be0a10c-408c-4de5-9ab8-57a78bad7beb", UserName = "admin@localhost", Email = "admin@localhost"
    };

    public static readonly ApplicationUser User = new()
    {
        Id = "CC80F94E-02FA-4E6C-F320-C68F0F926FA2", UserName = "user@localhost", Email = "user@localhost"
    };
}