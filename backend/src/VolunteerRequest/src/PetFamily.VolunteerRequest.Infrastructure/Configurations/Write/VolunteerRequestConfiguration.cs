using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.SharedKernel.Constants;
using PetFamily.SharedKernel.SharedVO;
using PetFamily.VolunteerRequest.Domain.ValueObjects;

namespace PetFamily.VolunteerRequest.Infrastructure.Configurations.Write;

public class VolunteerRequestConfiguration : IEntityTypeConfiguration<Domain.Entities.VolunteerRequest>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.VolunteerRequest> builder)
    {
        builder.ToTable("volunteer_requests");
        
        builder.HasKey(vr => vr.Id);
        
        builder.Property(vr => vr.Id)
            .HasConversion(
                idFromApp => idFromApp.Value,
                idFromDb => VolunteerRequestId.Create(idFromDb).Value);
        
        builder.Property(vr => vr.AdminId)
            .IsRequired(false)
            .HasColumnName("admin_id");
        
        builder.Property(vr => vr.UserId)
            .IsRequired()
            .HasColumnName("user_id");
        
        builder.Property(vr => vr.DiscussionId)
            .IsRequired(false)
            .HasColumnName("discussion_id");

        builder.ComplexProperty(v => v.VolunteerInfo, volunteerInfo =>
        {
            volunteerInfo.ComplexProperty(v => v.FullName, fio =>
            {
                fio.Property(f => f.FirstName)
                    .IsRequired()
                    .HasMaxLength(VolunteerConstant.MAX_NAME_LENGTH)
                    .HasColumnName("first_name");

                fio.Property(f => f.LastName)
                    .IsRequired()
                    .HasMaxLength(VolunteerConstant.MAX_NAME_LENGTH)
                    .HasColumnName("last_name");

                fio.Property(f => f.Surname)
                    .IsRequired()
                    .HasMaxLength(VolunteerConstant.MAX_NAME_LENGTH)
                    .HasColumnName("surname");
            });
        });

        builder.ComplexProperty(v => v.VolunteerInfo, volunteerInfo =>
        {
            volunteerInfo.ComplexProperty(v => v.Description, description =>
            {
                description.Property(d => d.Value)
                    .IsRequired()
                    .HasMaxLength(VolunteerConstant.MAX_DESCRIPTION_LENGHT)
                    .HasColumnName("description");
            });
        });
        
        builder.ComplexProperty(v => v.VolunteerInfo, volunteerInfo =>
        {
            volunteerInfo.ComplexProperty(v => v.Email, email =>
            {
                email.Property(d => d.Value)
                    .IsRequired()
                    .HasColumnName("email");
            });
        });
        
        builder.ComplexProperty(v => v.VolunteerInfo, volunteerInfo =>
        {
            volunteerInfo.ComplexProperty(v => v.Experience, exp =>
            {
                exp.Property(d => d.Value)
                    .IsRequired()
                    .HasColumnName("experience");
            });
        });

        builder.Property(v => v.Status)
            .HasConversion<string>()
            .HasColumnName("status");

        builder.Property(v => v.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at");
        
        builder.Property(v => v.RejectionDate)
            .IsRequired(false)
            .HasColumnName("rejected_date");

        builder.Property(v => v.RejectionComment)
            .HasConversion(
                valueToDb => valueToDb.Description.Value,
                valueFromDb => new RejectionComment(Description.Create(valueFromDb).Value))
            .IsRequired(false)
            .HasColumnName("rejection_comment");
    }
}