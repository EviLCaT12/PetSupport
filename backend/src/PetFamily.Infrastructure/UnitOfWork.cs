using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using PetFamily.Application.DataBase;
using PetFamily.Infrastructure.DbContexts;

namespace PetFamily.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly WriteDbDbContext _context;

    public UnitOfWork(WriteDbDbContext context)
    {
        _context = context;
    }

    public async Task<IDbTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        return transaction.GetDbTransaction();
    }
    
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken); 
    }
}

