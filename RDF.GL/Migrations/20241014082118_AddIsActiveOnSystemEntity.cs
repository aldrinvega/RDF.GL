using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RDF.GL.Migrations
{
    /// <inheritdoc />
    public partial class AddIsActiveOnSystemEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "systems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "user_roles",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2024, 10, 14, 8, 21, 17, 847, DateTimeKind.Utc).AddTicks(3847), new DateTime(2024, 10, 14, 8, 21, 17, 847, DateTimeKind.Utc).AddTicks(3853) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "password", "updated_at" },
                values: new object[] { new DateTime(2024, 10, 14, 16, 21, 17, 847, DateTimeKind.Local).AddTicks(3076), "$2a$11$5CAcB4t/hpD4ygfaWzE6R.hfctSVPIgbaGvbgzp5fdgXMlFu717MO", new DateTime(2024, 10, 14, 8, 21, 17, 693, DateTimeKind.Utc).AddTicks(7281) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_active",
                table: "systems");

            migrationBuilder.UpdateData(
                table: "user_roles",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2024, 10, 14, 3, 12, 59, 219, DateTimeKind.Utc).AddTicks(2820), new DateTime(2024, 10, 14, 3, 12, 59, 219, DateTimeKind.Utc).AddTicks(2825) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "password", "updated_at" },
                values: new object[] { new DateTime(2024, 10, 14, 11, 12, 59, 219, DateTimeKind.Local).AddTicks(2003), "$2a$11$p5IaoDbDC8Wf8Dcd7BNCN.34VN1eqs.PmK0KavxuukQfTR/ZGjYpi", new DateTime(2024, 10, 14, 3, 12, 59, 71, DateTimeKind.Utc).AddTicks(5006) });
        }
    }
}
