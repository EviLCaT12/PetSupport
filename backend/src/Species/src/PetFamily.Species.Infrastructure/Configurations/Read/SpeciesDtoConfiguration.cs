using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Species.Contracts.Dto.BreedDto;
using PetFamily.Species.Contracts.Dto.SpeciesDto;

namespace PetFamily.Species.Infrastructure.Configurations.Read;

public class SpeciesDtoConfiguration : IEntityTypeConfiguration<SpeciesDto>
{
    public void Configure(EntityTypeBuilder<SpeciesDto> builder)
    {
        builder.ToTable("species");
        
        builder.HasKey(dto => dto.Id);

        builder.HasMany<BreedDto>(s => s.Breeds)
            .WithOne()
            .HasForeignKey(b => b.SpeciesId)
            .IsRequired();
    }
}