using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Accounts.Contracts.Dto;

namespace PetFamily.Accounts.Infrastructure.Configurations.Read;

public class ParticipantAccountDtoConfiguration : IEntityTypeConfiguration<ParticipantAccountDto>
{
    public void Configure(EntityTypeBuilder<ParticipantAccountDto> builder)
    {
        builder.ToTable("participant_accounts");

        builder.HasKey(pa => pa.Id);
    }
}