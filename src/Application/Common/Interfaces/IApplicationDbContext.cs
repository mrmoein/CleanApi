using Microsoft.EntityFrameworkCore;

namespace CleanApi.Application.Common.Interfaces;

public interface IApplicationDbContext
{

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
