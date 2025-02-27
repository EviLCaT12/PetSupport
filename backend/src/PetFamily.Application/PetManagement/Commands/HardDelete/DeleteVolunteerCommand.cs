using PetFamily.Application.Abstractions;

namespace PetFamily.Application.PetManagement.Commands.HardDelete;

public record DeleteVolunteerCommand(Guid VolunteerId) : ICommand;