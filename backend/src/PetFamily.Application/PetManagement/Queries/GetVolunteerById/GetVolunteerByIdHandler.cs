using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.DataBase;
using PetFamily.Application.Dto.VolunteerDto;
using PetFamily.Domain.Shared.Error;

namespace PetFamily.Application.PetManagement.Queries.GetVolunteerById;

public class GetVolunteerByIdHandler : IQueryHandler<VolunteerDto, GetVolunteerByIdQuery>
{
    private readonly IReadDbContext _context;
    private readonly ILogger<GetVolunteerByIdHandler> _logger;

    public GetVolunteerByIdHandler(IReadDbContext context, ILogger<GetVolunteerByIdHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<VolunteerDto, ErrorList>> HandleAsync(GetVolunteerByIdQuery query, CancellationToken cancellationToken)
    {
        var volunteer = await _context.Volunteers
            .FirstOrDefaultAsync(v => v.Id == query.Id, cancellationToken);

        if (volunteer == null)
        {
            var msg = $"Volunteer with id {query.Id} not found";
            _logger.LogWarning(msg, query.Id);
            var error = Errors.General.ValueNotFound(query.Id);
            return new ErrorList([error]);
        }

        return volunteer;
    }
}