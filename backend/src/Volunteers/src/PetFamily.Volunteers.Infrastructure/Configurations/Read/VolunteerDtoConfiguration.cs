using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Core.Dto.Shared;
using PetFamily.Volunteers.Contracts.Dto.PetDto;
using PetFamily.Volunteers.Contracts.Dto.VolunteerDto;

namespace PetFamily.Volunteer.Infrastructure.Configurations.Read;

public class VolunteerDtoConfiguration : IEntityTypeConfiguration<VolunteerDto>
{
    public void Configure(EntityTypeBuilder<VolunteerDto> builder)
    {
        builder.ToTable("volunteers");
        
        builder.HasKey(v => v.Id);

        builder.Property(v => v.SocialWebs)
            .HasConversion(
                socialWebs => JsonSerializer.Serialize(string.Empty, JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<SocialWebDto[]>(json, JsonSerializerOptions.Default)!);
        
        builder.Property(v => v.TransferDetails)
            .HasConversion(
                socialWebs => JsonSerializer.Serialize(string.Empty, JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<TransferDetailDto[]>(json, JsonSerializerOptions.Default)!);

        builder.HasMany<PetDto>(v => v.Pets)
            .WithOne()
            .HasForeignKey(p => p.VolunteerId)
            .IsRequired(false);
        
        builder.HasQueryFilter(v => v.IsDeleted == false);
    }
}