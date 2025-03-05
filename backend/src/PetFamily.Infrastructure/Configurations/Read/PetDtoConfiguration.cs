using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Application.Dto.PetDto;
using PetFamily.Domain.PetContext.ValueObjects.PetVO;
using PetFamily.Domain.Shared.Constants;

namespace PetFamily.Infrastructure.Configurations.Read;

public class PetDtoConfiguration : IEntityTypeConfiguration<PetDto>
{
    public void Configure(EntityTypeBuilder<PetDto> builder)
    {
        builder.ToTable("pets");
        
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Photos)
            .HasConversion(
                photos => JsonSerializer.Serialize(string.Empty, JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<IEnumerable<PetPhoto>>(json, JsonSerializerOptions.Default)!
                    .Select(photo => new PhotoDto
                    {
                        PathToStorage = photo.PathToStorage.Path
                    })
                    .ToArray());
        
        builder.HasQueryFilter(p => p.IsDeleted == false);

    }
}