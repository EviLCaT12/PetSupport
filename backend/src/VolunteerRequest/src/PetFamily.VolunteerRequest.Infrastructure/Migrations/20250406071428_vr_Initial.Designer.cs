﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PetFamily.VolunteerRequest.Infrastructure.DbContexts.WriteContext;

#nullable disable

namespace PetFamily.VolunteerRequest.Infrastructure.Migrations
{
    [DbContext(typeof(WriteContext))]
    [Migration("20250406071428_vr_Initial")]
    partial class vr_Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("volunteer_request")
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("PetFamily.VolunteerRequest.Domain.Entities.VolunteerRequest", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid?>("AdminId")
                        .HasColumnType("uuid")
                        .HasColumnName("admin_id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<Guid?>("DiscussionId")
                        .HasColumnType("uuid")
                        .HasColumnName("discussion_id");

                    b.Property<string>("RejectionComment")
                        .HasColumnType("text")
                        .HasColumnName("rejection_comment");

                    b.Property<DateTime?>("RejectionDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("rejected_date");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("status");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.ComplexProperty<Dictionary<string, object>>("VolunteerInfo", "PetFamily.VolunteerRequest.Domain.Entities.VolunteerRequest.VolunteerInfo#VolunteerInfo", b1 =>
                        {
                            b1.IsRequired();

                            b1.ComplexProperty<Dictionary<string, object>>("Description", "PetFamily.VolunteerRequest.Domain.Entities.VolunteerRequest.VolunteerInfo#VolunteerInfo.Description#Description", b2 =>
                                {
                                    b2.IsRequired();

                                    b2.Property<string>("Value")
                                        .IsRequired()
                                        .HasMaxLength(2000)
                                        .HasColumnType("character varying(2000)")
                                        .HasColumnName("description");
                                });

                            b1.ComplexProperty<Dictionary<string, object>>("Email", "PetFamily.VolunteerRequest.Domain.Entities.VolunteerRequest.VolunteerInfo#VolunteerInfo.Email#Email", b2 =>
                                {
                                    b2.IsRequired();

                                    b2.Property<string>("Value")
                                        .IsRequired()
                                        .HasColumnType("text")
                                        .HasColumnName("email");
                                });

                            b1.ComplexProperty<Dictionary<string, object>>("Experience", "PetFamily.VolunteerRequest.Domain.Entities.VolunteerRequest.VolunteerInfo#VolunteerInfo.Experience#YearsOfExperience", b2 =>
                                {
                                    b2.IsRequired();

                                    b2.Property<int>("Value")
                                        .HasColumnType("integer")
                                        .HasColumnName("experience");
                                });

                            b1.ComplexProperty<Dictionary<string, object>>("FullName", "PetFamily.VolunteerRequest.Domain.Entities.VolunteerRequest.VolunteerInfo#VolunteerInfo.FullName#Fio", b2 =>
                                {
                                    b2.IsRequired();

                                    b2.Property<string>("FirstName")
                                        .IsRequired()
                                        .HasMaxLength(100)
                                        .HasColumnType("character varying(100)")
                                        .HasColumnName("first_name");

                                    b2.Property<string>("LastName")
                                        .IsRequired()
                                        .HasMaxLength(100)
                                        .HasColumnType("character varying(100)")
                                        .HasColumnName("last_name");

                                    b2.Property<string>("Surname")
                                        .IsRequired()
                                        .HasMaxLength(100)
                                        .HasColumnType("character varying(100)")
                                        .HasColumnName("surname");
                                });
                        });

                    b.HasKey("Id")
                        .HasName("pk_volunteer_requests");

                    b.ToTable("volunteer_requests", "volunteer_request");
                });
#pragma warning restore 612, 618
        }
    }
}
