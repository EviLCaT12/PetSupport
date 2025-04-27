using Contracts.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PetFamily.Discussion.Infrastructure.Configurations.Read;

public class MessageDtoConfiguration : IEntityTypeConfiguration<MessageDto>
{
    public void Configure(EntityTypeBuilder<MessageDto> builder)
    {
        builder.ToTable("messages");
        
        builder.HasKey(m => m.Id);
        
    }
}