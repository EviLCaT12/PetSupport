using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Contracts;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel.Error;
using PetFamily.SharedKernel.SharedVO;
using PetFamily.VolunteerRequest.Application.Abstractions;
using PetFamily.VolunteerRequest.Domain.ValueObjects;

namespace PetFamily.VolunteerRequest.Application.Commands.Create;

public class CreateVolunteerRequestHandler : ICommandHandler<Guid, CreateVolunteerRequestCommand>
{
    private readonly IValidator<CreateVolunteerRequestCommand> _commandValidator;
    private readonly ILogger<CreateVolunteerRequestHandler> _logger;
    private readonly IVolunteerRequestRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAccountContract _accountContract;

    public CreateVolunteerRequestHandler(
        IValidator<CreateVolunteerRequestCommand> commandValidator,
        ILogger<CreateVolunteerRequestHandler> logger,
        IVolunteerRequestRepository repository,
        [FromKeyedServices(ModuleKey.VolunteerRequest)] IUnitOfWork unitOfWork,
        IAccountContract accountContract)
    {
        _commandValidator = commandValidator;
        _logger = logger;
        _repository = repository;
        _unitOfWork = unitOfWork;
        _accountContract = accountContract;
    }
    public async Task<Result<Guid, ErrorList>> HandleAsync(CreateVolunteerRequestCommand command,
        CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        //Проверяем валидность введенных данных
        var validationResult = await _commandValidator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        //Проверям, что пользователь с заявки не является волонтером 
        var isAlreadyVolunteer = await _accountContract.IsUserAlreadyVolunteer(command.UserId, cancellationToken);
        if (isAlreadyVolunteer)
            return Errors.VolunteerRequest.UserAlreadyVolunteer();
        
        //Проверям, что у пользователя нет временного бана на отправку запросов
        var isUserInBan = await _accountContract
            .IsUserCanSendVolunteerRequests(command.UserId, cancellationToken);
        
        if (isUserInBan.IsFailure)
            return isUserInBan.Error;

        if (isUserInBan.Value == false)
            return Errors.VolunteerRequest.UserInTimeBan();
        
        var volunteerRequest = CreateVolunteerRequest(command);
        
        await _repository.AddVolunteerRequestAsync(volunteerRequest, cancellationToken);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        transaction.Commit();
        
        return volunteerRequest.Id.Value;

    }

    private Domain.Entities.VolunteerRequest CreateVolunteerRequest(CreateVolunteerRequestCommand command)
    {
        var id = VolunteerRequestId.NewVolunteerRequestId();
            
        var fullName = Fio.Create(
            command.FullName.FirstName,
            command.FullName.LastName,
            command.FullName.SurName).Value;

        var description = Description.Create(command.Description).Value;
            
        var email = Email.Create(command.Email).Value;
            
        var experience = YearsOfExperience.Create(command.Experience).Value;
            
        var volunteerInfo = new VolunteerInfo(fullName, description, email, experience);
            
        var volunteerRequest = new Domain.Entities.VolunteerRequest(
            id, command.UserId, volunteerInfo);
        
        return volunteerRequest;
    }
}