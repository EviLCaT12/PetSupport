using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.VolunteerRequest.Contracts.Dto;

namespace PetFamily.VolunteerRequest.Infrastructure.Configurations.Read;

public class VolunteerRequestDtoConfiguration : IEntityTypeConfiguration<VolunteerRequestDto>
{
    public void Configure(EntityTypeBuilder<VolunteerRequestDto> builder)
    {
        builder.ToTable("volunteer_requests");
        
        builder.HasKey(x => x.Id);

        builder.Property(vr => vr.Status)
            .HasConversion<string>();
    }
}