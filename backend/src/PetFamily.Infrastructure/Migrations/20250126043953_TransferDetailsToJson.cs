using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetFamily.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TransferDetailsToJson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "transfer_details_description",
                table: "volunteers");

            migrationBuilder.DropColumn(
                name: "transfer_details_name",
                table: "volunteers");

            migrationBuilder.DropColumn(
                name: "transfer_details_description",
                table: "pets");

            migrationBuilder.DropColumn(
                name: "transfer_details_name",
                table: "pets");

            migrationBuilder.AddColumn<string>(
                name: "TransferDetailsList",
                table: "volunteers",
                type: "jsonb",
                nullable: false,
                defaultValue: "{}");

            migrationBuilder.AddColumn<string>(
                name: "TransferDetailsList",
                table: "pets",
                type: "jsonb",
                nullable: false,
                defaultValue: "{}");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransferDetailsList",
                table: "volunteers");

            migrationBuilder.DropColumn(
                name: "TransferDetailsList",
                table: "pets");

            migrationBuilder.AddColumn<string>(
                name: "transfer_details_description",
                table: "volunteers",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "transfer_details_name",
                table: "volunteers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "transfer_details_description",
                table: "pets",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "transfer_details_name",
                table: "pets",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
