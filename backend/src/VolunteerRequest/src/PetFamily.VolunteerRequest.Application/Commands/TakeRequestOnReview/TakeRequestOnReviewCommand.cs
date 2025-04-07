using PetFamily.Core.Abstractions;

namespace PetFamily.VolunteerRequest.Application.Commands.TakeRequestOnReview;

public record TakeRequestOnReviewCommand(Guid RequestId, Guid AdminId) : ICommand;