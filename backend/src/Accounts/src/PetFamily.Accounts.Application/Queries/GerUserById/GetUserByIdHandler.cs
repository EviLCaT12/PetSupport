using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Application.DataBase;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dto.AccountDto;
using PetFamily.SharedKernel.Error;

namespace PetFamily.Accounts.Application.Queries.GerUserById;

public class GetUserByIdHandler : IQueryHandler<UserDto, GetUserByIdQuery>
{
    private readonly IReadDbContext _readDbContext;
    private readonly ILogger<GetUserByIdHandler> _logger;

    public GetUserByIdHandler(IReadDbContext readDbContext, ILogger<GetUserByIdHandler> logger)
    {
        _readDbContext = readDbContext;
        _logger = logger;
    }
    public async Task<Result<UserDto, ErrorList>> HandleAsync(GetUserByIdQuery query, CancellationToken cancellationToken)
    {
        var account = await _readDbContext.Accounts
            .Include(u => u.Admin)
            .Include(u => u.Participant)
            .Include(u => u.Volunteer)
            .FirstOrDefaultAsync(a => a.Id == query.UserId, cancellationToken);

        if (account is null)
        {
            _logger.LogWarning($"User with id: {query.UserId} not found");
            return  Errors.General.ValueNotFound(query.UserId).ToErrorList();
        }
            

        return account;
    }
}
