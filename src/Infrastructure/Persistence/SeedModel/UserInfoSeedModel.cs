using CleanApi.Domain.Entities;

namespace CleanApi.Infrastructure.Persistence.SeedModel;

public static class UserInfoSeedModel
{
    public static readonly UserInfo Admin = new()
    {
        CreatedBy = "Migration",
        ModifiedBy = "Migration",
        Id = IdentityUserSeedModel.Admin.Id,
        UserName = IdentityUserSeedModel.Admin.UserName,
        FirstName = "Big",
        LastName = "Boss"
    };

    public static readonly UserInfo User = new()
    {
        CreatedBy = "Migration",
        ModifiedBy = "Migration",
        Id = IdentityUserSeedModel.User.Id,
        UserName = IdentityUserSeedModel.User.UserName,
        FirstName = "Normal",
        LastName = "Guy"
    };
}