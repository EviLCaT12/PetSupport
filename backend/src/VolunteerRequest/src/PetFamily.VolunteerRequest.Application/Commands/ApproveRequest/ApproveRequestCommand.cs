using PetFamily.Core.Abstractions;

namespace PetFamily.VolunteerRequest.Application.Commands.ApproveRequest;

public record ApproveRequestCommand(Guid RequestId, string PhoneNumber) : ICommand;