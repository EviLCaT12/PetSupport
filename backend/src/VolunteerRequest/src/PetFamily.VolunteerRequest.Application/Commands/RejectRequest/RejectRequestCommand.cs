using PetFamily.Core.Abstractions;

namespace PetFamily.VolunteerRequest.Application.Commands.RejectRequest;

public record RejectRequestCommand(Guid RequestId, string Description) : ICommand;