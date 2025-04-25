using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetFamily.Discussion.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class dfgdfg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "text_value",
                schema: "discussion",
                table: "messages",
                newName: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "text",
                schema: "discussion",
                table: "messages",
                newName: "text_value");
        }
    }
}
