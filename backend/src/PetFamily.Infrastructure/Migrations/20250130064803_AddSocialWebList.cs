using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetFamily.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSocialWebList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "social_web_link",
                table: "volunteers");

            migrationBuilder.DropColumn(
                name: "social_web_name",
                table: "volunteers");

            migrationBuilder.AddColumn<string>(
                name: "SocialWeb",
                table: "volunteers",
                type: "jsonb",
                nullable: false,
                defaultValue: "{}");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SocialWeb",
                table: "volunteers");

            migrationBuilder.AddColumn<string>(
                name: "social_web_link",
                table: "volunteers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "social_web_name",
                table: "volunteers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
