using GameServerSP.Domain.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace GameServerSP.Infrastructure.Repositories;

public class UnitOfWork<TDbContext> : IUnitOfWork where TDbContext : DbContext
{
    private readonly TDbContext _dbContext;

    public UnitOfWork(TDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
