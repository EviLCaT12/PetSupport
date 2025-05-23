using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Core.Dto.Shared;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel.Constants;
using PetFamily.SharedKernel.SharedVO;
using PetFamily.Volunteers.Domain.ValueObjects.VolunteerVO;

namespace PetFamily.Volunteer.Infrastructure.Configurations.Write;

public class VolunteerConfiguration : IEntityTypeConfiguration<Volunteers.Domain.Entities.Volunteer>
{
    public void Configure(EntityTypeBuilder<Volunteers.Domain.Entities.Volunteer> builder)
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
                .HasColumnName("last_name");

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

        builder.HasMany(v => v.AllOwnedPets)
            .WithOne(p => p.Volunteer)
            .HasForeignKey("volunteer_id")
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
        
        builder.Property(v => v.DeletedOn)
            .IsRequired(false)
            .HasColumnName("deleted_on");
        
        builder.HasQueryFilter(v => v.IsDeleted == false);
    }
}