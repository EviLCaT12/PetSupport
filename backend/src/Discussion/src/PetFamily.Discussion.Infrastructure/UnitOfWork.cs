using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using PetFamily.Core.Abstractions;
using PetFamily.Discussion.Infrastructure.Contexts;

namespace PetFamily.Discussion.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly WriteDbContext _writeDbContext;

    public UnitOfWork(WriteDbContext writeDbContext)
    {
        _writeDbContext = writeDbContext;
    }
    public async Task<IDbTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        var transaction = await _writeDbContext.Database.BeginTransactionAsync(cancellationToken);
        return transaction.GetDbTransaction();
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _writeDbContext.SaveChangesAsync(cancellationToken);
    }
}