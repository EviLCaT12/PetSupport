using PetFamily.Core.Abstractions;

namespace PetFamily.Accounts.Application.Commands.BanUserForSendVolunteerRequest;

public record BanUserForSendVolunteerRequestCommand(Guid UserId) :ICommand;