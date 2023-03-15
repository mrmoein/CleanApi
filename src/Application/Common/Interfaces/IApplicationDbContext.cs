using CleanApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanApi.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<UserInfo> UserInfos { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}