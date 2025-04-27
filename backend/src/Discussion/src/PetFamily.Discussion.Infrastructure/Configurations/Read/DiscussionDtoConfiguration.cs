using Contracts.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PetFamily.Discussion.Infrastructure.Configurations.Read;

public class DiscussionDtoConfiguration : IEntityTypeConfiguration<DiscussionDto>
{
    public void Configure(EntityTypeBuilder<DiscussionDto> builder)
    {
        builder.ToTable("discussions");
        
        builder.HasKey(d => d.Id);

        builder.HasMany(d => d.Messages)
            .WithOne()
            .HasForeignKey(m => m.DiscussionId);
    }
}