using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Shared.Constants;
using PetFamily.Domain.SpeciesContext.Entities;
using PetFamily.Domain.SpeciesContext.ValueObjects.BreedVO;

namespace PetFamily.Infrastructure.Configurations;

public class BreedConfiguration : IEntityTypeConfiguration<Breed>
{
    public void Configure(EntityTypeBuilder<Breed> builder)
    {
        builder.ToTable("breeds");

        builder.HasKey(b => b.Id);
        
        builder.Property(b => b.Id)
            .HasConversion(
                b => b.Value,
                value => BreedId.Create(value));
        
        builder.ComplexProperty(b => b.Name, nb =>
        {
            nb.Property(n => n.Value)
                .IsRequired()
                .HasMaxLength(SpeciesConstant.MAX_NAME_LENGTH)
                .HasColumnName("name");
        });
    }
}