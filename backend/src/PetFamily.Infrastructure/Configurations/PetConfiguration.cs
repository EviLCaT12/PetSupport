using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.PetContext.Entities;
using PetFamily.Domain.PetContext.ValueObjects.PetVO;
using PetFamily.Domain.Shared.Constants;
using PetFamily.Infrastructure.Extensions;

namespace PetFamily.Infrastructure.Configurations;

public class PetConfiguration : IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> builder)
    {
        builder.ToTable("pets");
        
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Id)
            .HasConversion(
                id => id.Value,
                value => PetId.Create(value).Value);

        builder.ComplexProperty(p => p.Name, nb =>
        {
            nb.Property(n => n.Value)
                .IsRequired()
                .HasMaxLength(PetConstants.MAX_NAME_LENGTH)
                .HasColumnName("name");
        });

        builder.ComplexProperty(p => p.Classification, cb =>
        {
            cb.Property(c => c.SpeciesId)
                .IsRequired()
                .HasColumnName("species_id");

            cb.Property(c => c.BreedId)
                .IsRequired()
                .HasColumnName("breed_id");
        });

        builder.ComplexProperty(p => p.Description, db =>
        {
            db.Property(d => d.Value)
                .IsRequired()
                .HasMaxLength(PetConstants.MAX_DESCRIPTION_LENGHT)
                .HasColumnName("description");
        });
        
        builder.ComplexProperty(p => p.Color, cb =>
        {
            cb.Property(c => c.Value)
                .IsRequired()
                .HasColumnName("color");
        });
        
        builder.ComplexProperty(p => p.Position, sb =>
        {
            sb.Property(s => s.Value)
                .IsRequired()
                .HasColumnName("position");
        });
        
        builder.ComplexProperty(p => p.HealthInfo, hb =>
        {
            hb.Property(d => d.Value)
                .IsRequired()
                .HasMaxLength(PetConstants.MAX_DESCRIPTION_LENGHT)
                .HasColumnName("health_info");
        });
        
        builder.ComplexProperty(p => p.Address, ab =>
        {
            ab.Property(a => a.City)
                .IsRequired()
                .HasColumnName("city");

            ab.Property(a => a.Street)
                .IsRequired()
                .HasColumnName("street");
            
            ab.Property(a => a.HouseNumber)
                .IsRequired()
                .HasColumnName("house_number");
        });
        
        builder.ComplexProperty(p => p.Dimensions, db =>
        {
            db.Property(d => d.Height)
                .IsRequired()
                .HasColumnName("height");

            db.Property(d => d.Weight)
                .IsRequired()
                .HasColumnName("weight");
        });
        
        builder.ComplexProperty(p => p.OwnerPhoneNumber, pb =>
        {
            pb.Property(p => p.Number)
                .IsRequired()
                .HasMaxLength(VolunteerConstant.MAX_PHONE_NUMBER_LENTH)
                .HasColumnName("owner_phone");
        });

        builder.Property(p => p.IsCastrate)
            .IsRequired();
        
        builder.Property(p => p.DateOfBirth)
            .IsRequired();
        
        builder.Property(p => p.IsVaccinated)
            .IsRequired();

        builder.Property(p => p.HelpStatus)
            .IsRequired()
            .HasConversion<string>();
        
        builder.Property(v => v.TransferDetailsList)
            .JsonValueObjectCollectionConversion()
            .HasColumnName("transfer_details")
            .HasField("_transferDetails");
            
        builder.Property(p => p.CreatedAt)
            .HasDefaultValue(DateTime.MinValue)
            .IsRequired()
            .HasColumnName("created_at");
        
        builder.Property<bool>("_isDeleted")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("is_deleted");

        // builder.OwnsOne(p => p.PhotoList, plb =>
        // {
        //     plb.ToJson();
        //
        //     plb.OwnsMany(p => p.Values, pb =>
        //     {
        //         pb.OwnsOne(path => path.PathToStorage, pathBuilder =>
        //         {
        //             pathBuilder.Property(p => p.Path)
        //                 .IsRequired(false);
        //         });
        //     });
        // });
        
        builder.Property(p => p.PhotoList!)
            .JsonValueObjectCollectionConversion()
            .HasColumnName("photos")
            .HasField("_photos");
    }
}