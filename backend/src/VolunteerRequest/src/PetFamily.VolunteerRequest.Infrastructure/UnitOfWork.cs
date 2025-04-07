using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using PetFamily.Core.Abstractions;
using PetFamily.VolunteerRequest.Infrastructure.DbContexts.WriteContext;

namespace PetFamily.VolunteerRequest.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    public UnitOfWork(WriteContext context)
    {
        _context = context;
    }
    
    private readonly WriteContext _context;

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