using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Core.Dto.Shared;
using PetFamily.SharedKernel.SharedVO;
using PetFamily.Volunteers.Contracts.Dto.PetDto;

namespace PetFamily.Volunteer.Infrastructure.Configurations.Read;

public class PetDtoConfiguration : IEntityTypeConfiguration<PetDto>
{
    public void Configure(EntityTypeBuilder<PetDto> builder)
    {
        builder.ToTable("pets");
        
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Photos)
            .HasConversion(
                photos => JsonSerializer.Serialize(string.Empty, JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<IEnumerable<Photo>>(json, JsonSerializerOptions.Default)!
                    .Select(photo => new PhotoDto
                    {
                        Id = photo.Id.Id
                    })
                    .ToArray());
        
        builder.HasQueryFilter(p => p.IsDeleted == false);

    }
}