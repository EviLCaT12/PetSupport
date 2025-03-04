using PetFamily.Application.Abstractions;

namespace PetFamily.Application.PetManagement.Commands.DeletePet;

public record DeletePetCommand(Guid VolunteerId, Guid PetId) : ICommand;