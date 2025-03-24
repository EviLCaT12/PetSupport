using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Accounts.Domain.Entitues;
using PetFamily.Accounts.Domain.ValueObjects;
using PetFamily.Core.Dto.PetDto;
using PetFamily.Core.Dto.Shared;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel.SharedVO;

namespace PetFamily.Accounts.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder
            .HasMany(u => u.Roles)
            .WithMany()
            .UsingEntity<IdentityUserRole<Guid>>();

        // builder.ComplexProperty(u => u.FullName, fb =>
        // {
        //     fb.Property(f => f.FirstName)
        //         .IsRequired()
        //         .HasColumnName("first_name");
        //
        //     fb.Property(f => f.LastName)
        //         .IsRequired()
        //         .HasColumnName("last_name");
        //
        //     fb.Property(f => f.Surname)
        //         .IsRequired()
        //         .HasColumnName("surname");
        // });
        //
        // builder.Property(u => u.SocialWebs)
        //     .Json1DeepLvlVoCollectionConverter(
        //         socialWebs => new SocialWebDto(socialWebs.Link, socialWebs.Name),
        //         dto => SocialWeb.Create(dto.Link, dto.Name).Value)
        //     .HasColumnName("social_webs");
        //
        // builder.Property(u => u.Photo)
        //     .HasConversion(
        //         photo => JsonSerializer.Serialize(photo, JsonSerializerOptions.Default),
        //         json => JsonSerializer.Deserialize<Photo>(json, JsonSerializerOptions.Default)!)
        //     .HasColumnName("user_photo");
    }
}