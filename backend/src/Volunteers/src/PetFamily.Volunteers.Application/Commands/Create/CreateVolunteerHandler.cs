using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dto.Shared;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel.Error;
using PetFamily.SharedKernel.SharedVO;
using PetFamily.Volunteers.Domain.ValueObjects.VolunteerVO;

namespace PetFamily.Volunteers.Application.Commands.Create;

public class CreateVolunteerHandler(
    ILogger<CreateVolunteerHandler> logger,
    IVolunteersRepository volunteersRepository,
    IValidator<CreateVolunteerCommand> createVolunteerCommandValidator,
    [FromKeyedServices(ModuleKey.Volunteer)] IUnitOfWork unitOfWork) : ICommandHandler<Guid, CreateVolunteerCommand>
{
    public async Task<Result<Guid, ErrorList>> HandleAsync(
        CreateVolunteerCommand createVolunteerCommand,
        CancellationToken cancellationToken = default)
    {
        var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var createVolunteerValidationResult = await createVolunteerCommandValidator.ValidateAsync(
            createVolunteerCommand,
            cancellationToken);
            if (createVolunteerValidationResult.IsValid == false)
                return createVolunteerValidationResult.ToErrorList();
            
            var volunteerId = VolunteerId.NewVolunteerId();

            var fioCreateResult = Fio
                .Create(createVolunteerCommand.Fio.FirstName, createVolunteerCommand.Fio.LastName, createVolunteerCommand.Fio.SurName)
                .Value;
        

            var phoneNumberCreateResult = Phone.Create(createVolunteerCommand.PhoneNumber).Value;



            var emailCreateResult = Email.Create(createVolunteerCommand.Email).Value;

            var descriptionCreateResult = Description.Create(createVolunteerCommand.Description).Value;
            
            List<TransferDetails> transferDetails = [];
            transferDetails.AddRange(createVolunteerCommand.TransferDetailDto
                .Select(transferDetail => TransferDetails.Create(transferDetail.Name, transferDetail.Description))
                .Select(transferDetailsCreateResult => transferDetailsCreateResult.Value));
            var transferDetailsList = new List<TransferDetails>(transferDetails);
        
            var validVolunteer = Volunteers.Domain.Entities.Volunteer.Create(
                volunteerId,
                fioCreateResult,
                phoneNumberCreateResult,
                emailCreateResult,
                descriptionCreateResult,
                transferDetailsList
            );
        
        await volunteersRepository.AddAsync(validVolunteer.Value, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        transaction.Commit();
            
        logger.LogInformation("Created volunteer with ID: {volunteerId}", volunteerId);
        
        return validVolunteer.Value.Id.Value;
        }
        catch (Exception e)
        {
            logger.LogError(e,
                "Fail to add volunteer in transaction");
            
            transaction.Rollback();
            
            var error = Error.Failure("volunteer.failure", "Error during add volunteer transaction");

            return new ErrorList([error]);
        }
        
    }
}