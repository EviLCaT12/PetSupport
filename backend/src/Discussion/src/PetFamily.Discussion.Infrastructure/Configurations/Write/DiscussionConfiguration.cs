using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Discussion.Domain.ValueObjects;

namespace PetFamily.Discussion.Infrastructure.Configurations.Write;

public class DiscussionConfiguration : IEntityTypeConfiguration<Domain.Entities.Discussion>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Discussion> builder)
    {
        builder.ToTable("discussions");
        
        builder.HasKey(d => d.Id);
        
        builder.Property(d => d.Id)
            .HasConversion(
                idToDb => idToDb.Value,
                idFromDb => DiscussionsId.Create(idFromDb).Value);
        
        builder.Property(d => d.RelationId)
            .IsRequired();

        builder.Property(d => d.Members)
            .HasColumnType("uuid[]")
            .IsRequired()
            .HasColumnName("members");
        
        builder.HasMany(d => d.Messages)
            .WithOne(m => m.Discussion)
            .HasForeignKey("discussion_id")
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.Property(d => d.Status)
            .HasConversion<string>()
            .IsRequired();


    }
}