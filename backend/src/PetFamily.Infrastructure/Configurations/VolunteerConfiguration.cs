using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.PetContext.Entities;
using PetFamily.Domain.PetContext.ValueObjects.VolunteerVO;
using PetFamily.Domain.Shared.Constants;
using PetFamily.Infrastructure.Extensions;

namespace PetFamily.Infrastructure.Configurations;

public class VolunteerConfiguration : IEntityTypeConfiguration<Volunteer>
{
    public void Configure(EntityTypeBuilder<Volunteer> builder)
    {
        builder.ToTable("volunteers");
        
        builder.HasKey(v => v.Id);

        builder.Property(v => v.Id)
            .HasConversion(
                id => id.Value,
                value => VolunteerId.Create(value).Value);

        builder.ComplexProperty(v => v.Fio, fb =>
        {
            fb.Property(f => f.FirstName)
                .IsRequired()
                .HasMaxLength(VolunteerConstant.MAX_NAME_LENGTH)
                .HasColumnName("first_name");

            fb.Property(f => f.LastName)
                .IsRequired()
                .HasMaxLength(VolunteerConstant.MAX_NAME_LENGTH)
                .HasColumnName("second_name");

            fb.Property(f => f.Surname)
                .IsRequired()
                .HasMaxLength(VolunteerConstant.MAX_NAME_LENGTH)
                .HasColumnName("surname");
        });

        builder.ComplexProperty(v => v.Phone, pb =>
        {
            pb.Property(p => p.Number)
                .IsRequired()
                .HasMaxLength(VolunteerConstant.MAX_PHONE_NUMBER_LENTH)
                .HasColumnName("phone");
        });

        builder.ComplexProperty(v => v.Email, eb =>
        {
            eb.Property(e => e.Value)
                .IsRequired()
                .HasColumnName("email");
        });

        builder.ComplexProperty(v => v.Description, db =>
        {
            db.Property(d => d.Value)
                .IsRequired()
                .HasMaxLength(VolunteerConstant.MAX_DESCRIPTION_LENGHT)
                .HasColumnName("description");
        });
        
        builder.ComplexProperty(v => v.YearsOfExperience, yb =>
        {
            yb.Property(y => y.Value)
                .IsRequired()
                .HasColumnName("years_of_experience");
        });

        builder.Property(v => v.SocialWebList)
            .JsonValueObjectCollectionConversion()
            .HasColumnName("social_webs")
            .HasField("_socialWebs"); //Если не использовать этот метод, то падает ошибка
            
        builder.Property(v => v.TransferDetailsList)
            .JsonValueObjectCollectionConversion()
            .HasColumnName("transfer_details")
            .HasField("_transferDetails");

        builder.HasMany(v => v.AllOwnedPets)
            .WithOne(p => p.Volunteer)
            .HasForeignKey("pet_id")
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
        
        builder.Property<bool>("_isDeleted")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("is_deleted");
    }
}