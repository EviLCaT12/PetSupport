using PetFamily.Core.Abstractions;

namespace PetFamily.VolunteerRequest.Application.Commands.SendRequestToRevision;

public record SendRequestToRevisionCommand(Guid RequestId, string Discription) : ICommand;