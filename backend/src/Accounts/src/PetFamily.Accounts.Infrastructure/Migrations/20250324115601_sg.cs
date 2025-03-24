using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetFamily.Accounts.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class sg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_admin_accounts_asp_net_users_user_id1",
                schema: "account",
                table: "admin_accounts");

            migrationBuilder.DropForeignKey(
                name: "fk_admin_accounts_user_user_id",
                schema: "account",
                table: "admin_accounts");

            migrationBuilder.DropIndex(
                name: "ix_admin_accounts_user_id1",
                schema: "account",
                table: "admin_accounts");

            migrationBuilder.DropColumn(
                name: "user_id1",
                schema: "account",
                table: "admin_accounts");

            migrationBuilder.AlterColumn<Guid>(
                name: "user_id",
                schema: "account",
                table: "admin_accounts",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "fk_admin_accounts_users_user_id",
                schema: "account",
                table: "admin_accounts",
                column: "user_id",
                principalSchema: "account",
                principalTable: "users",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_admin_accounts_users_user_id",
                schema: "account",
                table: "admin_accounts");

            migrationBuilder.AlterColumn<Guid>(
                name: "user_id",
                schema: "account",
                table: "admin_accounts",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "user_id1",
                schema: "account",
                table: "admin_accounts",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_admin_accounts_user_id1",
                schema: "account",
                table: "admin_accounts",
                column: "user_id1",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_admin_accounts_asp_net_users_user_id1",
                schema: "account",
                table: "admin_accounts",
                column: "user_id1",
                principalSchema: "account",
                principalTable: "users",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_admin_accounts_user_user_id",
                schema: "account",
                table: "admin_accounts",
                column: "user_id",
                principalSchema: "account",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
