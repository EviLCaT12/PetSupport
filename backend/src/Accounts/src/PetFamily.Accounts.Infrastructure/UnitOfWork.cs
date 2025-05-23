using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using PetFamily.Accounts.Infrastructure.Contexts;
using PetFamily.Core.Abstractions;

namespace PetFamily.Accounts.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly WriteAccountsDbContext _dbContext;

    public UnitOfWork(WriteAccountsDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<IDbTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        return transaction.GetDbTransaction();
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}  