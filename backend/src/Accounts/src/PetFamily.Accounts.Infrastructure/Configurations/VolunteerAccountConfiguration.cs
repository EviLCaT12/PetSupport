using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Accounts.Domain.Entities.AccountEntitites;
using PetFamily.Core.Dto.Shared;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel.SharedVO;

namespace PetFamily.Accounts.Infrastructure.Configurations;

public class VolunteerAccountConfiguration : IEntityTypeConfiguration<VolunteerAccount>
{
    public void Configure(EntityTypeBuilder<VolunteerAccount> builder)
    {
        builder.ToTable("volunteer_accounts");

        builder.ComplexProperty(va => va.Experience, eb =>
        {
            eb.Property(v => v.Value)
                .HasColumnName("experience");
        });
        
        builder.Property(va => va.TransferDetails)
            .IsRequired(false)
            .Json1DeepLvlVoCollectionConverter(
                transferDetails => new TransferDetailDto(transferDetails.Name, transferDetails.Description),
                dto => TransferDetails.Create(dto.Name, dto.Description).Value)
            .HasColumnName("requisites");
    }
}