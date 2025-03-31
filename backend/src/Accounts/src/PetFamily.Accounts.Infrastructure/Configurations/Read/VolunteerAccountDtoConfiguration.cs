using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Core.Dto.AccountDto;
using PetFamily.Core.Dto.Shared;

namespace PetFamily.Accounts.Infrastructure.Configurations.Read;

public class VolunteerAccountDtoConfiguration : IEntityTypeConfiguration<VolunteerAccountDto>
{
    public void Configure(EntityTypeBuilder<VolunteerAccountDto> builder)
    {
        builder.ToTable("volunteer_accounts");
        
        builder.HasKey(va => va.Id);

        builder.Property(va => va.Requisites)
            .HasConversion(
                requisite => JsonSerializer.Serialize(string.Empty, JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<TransferDetailDto[]>(json, JsonSerializerOptions.Default)!);
        
    }
    
}