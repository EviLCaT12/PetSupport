using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Application.Dto.BreedDto;
using PetFamily.Application.Dto.SpeciesDto;
using PetFamily.Application.Dto.VolunteerDto;

namespace PetFamily.Infrastructure.Configurations.Read;

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