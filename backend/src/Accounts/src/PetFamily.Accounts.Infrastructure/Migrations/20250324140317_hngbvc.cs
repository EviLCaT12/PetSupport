using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetFamily.Accounts.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class hngbvc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_volunteer_accounts_user_user_id",
                schema: "account",
                table: "volunteer_accounts");

            migrationBuilder.AlterColumn<Guid>(
                name: "user_id",
                schema: "account",
                table: "volunteer_accounts",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "requisites",
                schema: "account",
                table: "volunteer_accounts",
                type: "jsonb",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "jsonb");

            migrationBuilder.AddForeignKey(
                name: "fk_volunteer_accounts_users_user_id",
                schema: "account",
                table: "volunteer_accounts",
                column: "user_id",
                principalSchema: "account",
                principalTable: "users",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_volunteer_accounts_users_user_id",
                schema: "account",
                table: "volunteer_accounts");

            migrationBuilder.AlterColumn<Guid>(
                name: "user_id",
                schema: "account",
                table: "volunteer_accounts",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "requisites",
                schema: "account",
                table: "volunteer_accounts",
                type: "jsonb",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "jsonb",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_volunteer_accounts_user_user_id",
                schema: "account",
                table: "volunteer_accounts",
                column: "user_id",
                principalSchema: "account",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
