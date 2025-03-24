using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetFamily.Accounts.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class dfscfd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "role_user",
                schema: "account");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_admin_accounts_asp_net_users_user_id1",
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

            migrationBuilder.CreateTable(
                name: "role_user",
                schema: "account",
                columns: table => new
                {
                    roles_id = table.Column<Guid>(type: "uuid", nullable: false),
                    users_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role_user", x => new { x.roles_id, x.users_id });
                    table.ForeignKey(
                        name: "fk_role_user_role_roles_id",
                        column: x => x.roles_id,
                        principalSchema: "account",
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_role_user_user_users_id",
                        column: x => x.users_id,
                        principalSchema: "account",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_role_user_users_id",
                schema: "account",
                table: "role_user",
                column: "users_id");
        }
    }
}
