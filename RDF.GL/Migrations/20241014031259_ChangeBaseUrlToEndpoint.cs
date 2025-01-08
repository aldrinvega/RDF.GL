using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RDF.GL.Migrations
{
    /// <inheritdoc />
    public partial class ChangeBaseUrlToEndpoint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "base_url",
                table: "systems",
                newName: "endpoint");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "endpoint",
                table: "systems",
                newName: "base_url");

            migrationBuilder.UpdateData(
                table: "user_roles",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2024, 10, 10, 6, 54, 25, 39, DateTimeKind.Utc).AddTicks(3928), new DateTime(2024, 10, 10, 6, 54, 25, 39, DateTimeKind.Utc).AddTicks(3933) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "password", "updated_at" },
                values: new object[] { new DateTime(2024, 10, 10, 14, 54, 25, 39, DateTimeKind.Local).AddTicks(3198), "$2a$11$kx4tm9IaKO2pmIgu8q9tueuALM4DC41mbojssx4RA/PK3VfmvbTMu", new DateTime(2024, 10, 10, 6, 54, 24, 889, DateTimeKind.Utc).AddTicks(1246) });
        }
    }
}
