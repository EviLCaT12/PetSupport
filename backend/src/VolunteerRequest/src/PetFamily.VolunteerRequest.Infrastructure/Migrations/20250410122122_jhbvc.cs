using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetFamily.VolunteerRequest.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class jhbvc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "volunteer_request");

            migrationBuilder.CreateTable(
                name: "volunteer_requests",
                schema: "volunteer_request",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    admin_id = table.Column<Guid>(type: "uuid", nullable: true),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    discussion_id = table.Column<Guid>(type: "uuid", nullable: true),
                    status = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    rejected_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    rejection_comment = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    experience = table.Column<int>(type: "integer", nullable: false),
                    first_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    last_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    surname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_volunteer_requests", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "volunteer_requests",
                schema: "volunteer_request");
        }
    }
}
