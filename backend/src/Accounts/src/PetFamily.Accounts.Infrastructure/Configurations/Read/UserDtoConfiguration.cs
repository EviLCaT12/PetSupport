using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Accounts.Contracts.Dto;
using PetFamily.Core.Dto.Shared;

namespace PetFamily.Accounts.Infrastructure.Configurations.Read;

public class UserDtoConfiguration : IEntityTypeConfiguration<UserDto>
{
    public void Configure(EntityTypeBuilder<UserDto> builder)
    {
        builder.ToTable("users");
        
        builder.HasKey(a => a.Id);

        builder.Property(a => a.UserPhoto)
            .HasConversion(
                photo => JsonSerializer.Serialize(string.Empty, JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<PhotoDto>(json, JsonSerializerOptions.Default)!);

        builder.Property(a => a.SocialWebs)
            .HasConversion(
                socialWebs => JsonSerializer.Serialize(string.Empty, JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<SocialWebDto[]>(json, JsonSerializerOptions.Default));

        builder.HasOne(a => a.Admin)
            .WithOne()
            .HasForeignKey<AdminAccountDto>(aa => aa.UserId)
            .IsRequired(false);
        
        builder.HasOne(a => a.Volunteer)
            .WithOne()
            .HasForeignKey<VolunteerAccountDto>(va => va.UserId)
            .IsRequired(false);

        builder.HasOne(a => a.Participant)
            .WithOne()
            .HasForeignKey<ParticipantAccountDto>(pa => pa.UserId)
            .IsRequired(false);
    }
}
