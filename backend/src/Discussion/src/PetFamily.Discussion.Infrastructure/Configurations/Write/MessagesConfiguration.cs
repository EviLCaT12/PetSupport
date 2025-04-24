using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Discussion.Domain.Entities;
using PetFamily.Discussion.Domain.ValueObjects;

namespace PetFamily.Discussion.Infrastructure.Configurations.Write;

public class MessagesConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToTable("messages");
        
        builder.HasKey(m => m.Id);
        
        builder.Property(m => m.Id)
            .HasConversion(
                idToDb => idToDb.Value,
                idFromDb => MessageId.Create(idFromDb).Value);
        
        builder.Property(m => m.UserId)
            .IsRequired();

        builder.ComplexProperty(m => m.Text, tb =>
        {
            tb.Property(t => t.Value)
                .IsRequired();
        });
        
        builder.Property(m => m.CreatedAt)
            .IsRequired();
        
        builder.Property(m => m.IsEdited)
            .IsRequired();
    }
}