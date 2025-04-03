using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Accounts.Contracts.Dto;
using PetFamily.Accounts.Domain.Entities.AccountEntitites;

namespace PetFamily.Accounts.Infrastructure.Configurations.Read;

public class AdminAccountDtoConfiguration : IEntityTypeConfiguration<AdminAccountDto>
{
    public void Configure(EntityTypeBuilder<AdminAccountDto> builder)
    {
        builder.ToTable("admin_accounts");
        builder.HasKey(aa => aa.Id);
    }
}