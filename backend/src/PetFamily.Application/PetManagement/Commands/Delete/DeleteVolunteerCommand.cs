using PetFamily.Application.Abstractions;

namespace PetFamily.Application.PetManagement.Commands.Delete;

public record DeleteVolunteerCommand(Guid VolunteerId) : ICommand;