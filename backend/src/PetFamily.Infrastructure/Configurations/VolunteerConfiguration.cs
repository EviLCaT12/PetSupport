using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.PetContext.Entities;
using PetFamily.Domain.PetContext.ValueObjects.VolunteerVO;
using PetFamily.Domain.Shared.Constants;

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
                value => VolunteerId.Create(value));

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
        
        builder.Property(v => v.SumPetsWithHome)
            .HasColumnName("pets_with_home");
        
        builder.Property(v => v.SumPetsTryFindHome)
            .HasColumnName("pets_without_home");
                
        builder.Property(v => v.SumPetsUnderTreatment)
            .HasColumnName("pets_under_treatment");

        builder.ComplexProperty(v => v.SocialWeb, sb =>
        {
            sb.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(VolunteerConstant.MAX_NAME_LENGTH)
                .HasColumnName("social_web_name");
            
            sb.Property(s => s.Link)
                .IsRequired()
                .HasColumnName("social_web_link");
        });
        
        builder.ComplexProperty(v => v.TransferDetails, tb =>
        {
            tb.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(VolunteerConstant.MAX_NAME_LENGTH)
                .HasColumnName("transfer_details_name");
            
            tb.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(VolunteerConstant.MAX_DESCRIPTION_LENGHT)
                .HasColumnName("transfer_details_description");
        });

        builder.OwnsOne(v => v.AllOwnedPets, ia =>
        {
            ia.ToJson();

            ia.OwnsMany(v => v.Value);
            
        });
    }
}