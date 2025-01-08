using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RDF.GL.Migrations
{
    /// <inheritdoc />
    public partial class AddSeeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_general_ledger_users_added_by",
                table: "general_ledger");

            migrationBuilder.DropForeignKey(
                name: "fk_user_roles_users_added_by",
                table: "user_roles");

            migrationBuilder.DropForeignKey(
                name: "fk_user_roles_users_modified_by",
                table: "user_roles");

            migrationBuilder.DropPrimaryKey(
                name: "pk_general_ledger",
                table: "general_ledger");

            migrationBuilder.RenameTable(
                name: "general_ledger",
                newName: "general_ledgers");

            migrationBuilder.RenameIndex(
                name: "ix_general_ledger_added_by",
                table: "general_ledgers",
                newName: "ix_general_ledgers_added_by");

            migrationBuilder.AlterColumn<int>(
                name: "modified_by",
                table: "user_roles",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "added_by",
                table: "user_roles",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "pk_general_ledgers",
                table: "general_ledgers",
                column: "id");

            migrationBuilder.InsertData(
                table: "user_roles",
                columns: new[] { "id", "added_by", "created_at", "is_active", "modified_by", "permissions", "updated_at", "user_role_name" },
                values: new object[] { 1, null, new DateTime(2024, 9, 20, 3, 40, 0, 537, DateTimeKind.Utc).AddTicks(2959), true, null, "[\"User Management\"]", new DateTime(2024, 9, 20, 3, 40, 0, 537, DateTimeKind.Utc).AddTicks(2965), "Admin" });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "created_at", "first_name", "id_number", "id_prefix", "is_active", "last_name", "middle_name", "password", "sex", "updated_at", "user_role_id", "username" },
                values: new object[] { 1, new DateTime(2024, 9, 20, 11, 40, 0, 537, DateTimeKind.Local).AddTicks(1984), "Admin", null, null, true, null, null, "$2a$11$8.6uD4qL1jUNjr.V1PvFreyODLX9cezwSTK.X4i5I.083XdVj6A/q", null, new DateTime(2024, 9, 20, 3, 40, 0, 381, DateTimeKind.Utc).AddTicks(9179), 1, "admin" });

            migrationBuilder.AddForeignKey(
                name: "fk_general_ledgers_users_added_by",
                table: "general_ledgers",
                column: "added_by",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "fk_user_roles_users_added_by",
                table: "user_roles",
                column: "added_by",
                principalTable: "users",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_user_roles_users_modified_by",
                table: "user_roles",
                column: "modified_by",
                principalTable: "users",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_general_ledgers_users_added_by",
                table: "general_ledgers");

            migrationBuilder.DropForeignKey(
                name: "fk_user_roles_users_added_by",
                table: "user_roles");

            migrationBuilder.DropForeignKey(
                name: "fk_user_roles_users_modified_by",
                table: "user_roles");

            migrationBuilder.DropPrimaryKey(
                name: "pk_general_ledgers",
                table: "general_ledgers");

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "user_roles",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.RenameTable(
                name: "general_ledgers",
                newName: "general_ledger");

            migrationBuilder.RenameIndex(
                name: "ix_general_ledgers_added_by",
                table: "general_ledger",
                newName: "ix_general_ledger_added_by");

            migrationBuilder.AlterColumn<int>(
                name: "modified_by",
                table: "user_roles",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "added_by",
                table: "user_roles",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "pk_general_ledger",
                table: "general_ledger",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_general_ledger_users_added_by",
                table: "general_ledger",
                column: "added_by",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "fk_user_roles_users_added_by",
                table: "user_roles",
                column: "added_by",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "fk_user_roles_users_modified_by",
                table: "user_roles",
                column: "modified_by",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
