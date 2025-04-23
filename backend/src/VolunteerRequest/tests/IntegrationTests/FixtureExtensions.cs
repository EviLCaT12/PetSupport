using AutoFixture;
using PetFamily.Core.Dto.Shared;
using PetFamily.VolunteerRequest.Application.Commands.Create;
using PetFamily.VolunteerRequest.Application.Commands.EditRequest;

namespace IntegrationTests;

public static class FixtureExtensions
{
    public static CreateVolunteerRequestCommand AddCreateCommand(this IFixture fixture)
    {
        return fixture.Build<CreateVolunteerRequestCommand>()
            .With(c => c.Email, "email@mail.ru")
            .With(c => c.FullName,
                new FioDto("string", "string", "string"))
            .Create();
    }

    public static EditCommand AddEditCommand(this IFixture fixture, Guid requestId)
    {
        return fixture.Build<EditCommand>()
            .With(c => c.RequestId, requestId)
            .With(c => c.Fio,
                new FioDto("newString", "newString", "newString"))
            .With(c => c.Email, "newEmail@mail.ru")
            .Create();
    }
}