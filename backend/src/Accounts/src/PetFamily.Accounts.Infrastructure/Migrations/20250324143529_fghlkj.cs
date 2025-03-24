using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetFamily.Accounts.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fghlkj : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "first_name",
                schema: "account",
                table: "users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "last_name",
                schema: "account",
                table: "users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "social_webs",
                schema: "account",
                table: "users",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "surname",
                schema: "account",
                table: "users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "user_photo",
                schema: "account",
                table: "users",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "first_name",
                schema: "account",
                table: "users");

            migrationBuilder.DropColumn(
                name: "last_name",
                schema: "account",
                table: "users");

            migrationBuilder.DropColumn(
                name: "social_webs",
                schema: "account",
                table: "users");

            migrationBuilder.DropColumn(
                name: "surname",
                schema: "account",
                table: "users");

            migrationBuilder.DropColumn(
                name: "user_photo",
                schema: "account",
                table: "users");
        }
    }
}
