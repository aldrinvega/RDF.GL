using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RDF.GL.Migrations
{
    /// <inheritdoc />
    public partial class AdjustUserEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "username",
                table: "users",
                newName: "user_name");

            migrationBuilder.RenameColumn(
                name: "fullname",
                table: "users",
                newName: "sex");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "first_name",
                table: "users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "id_number",
                table: "users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "id_prefix",
                table: "users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "last_name",
                table: "users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "middle_name",
                table: "users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "users",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "created_at",
                table: "users");

            migrationBuilder.DropColumn(
                name: "first_name",
                table: "users");

            migrationBuilder.DropColumn(
                name: "id_number",
                table: "users");

            migrationBuilder.DropColumn(
                name: "id_prefix",
                table: "users");

            migrationBuilder.DropColumn(
                name: "last_name",
                table: "users");

            migrationBuilder.DropColumn(
                name: "middle_name",
                table: "users");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "users");

            migrationBuilder.RenameColumn(
                name: "user_name",
                table: "users",
                newName: "username");

            migrationBuilder.RenameColumn(
                name: "sex",
                table: "users",
                newName: "fullname");
        }
    }
}
