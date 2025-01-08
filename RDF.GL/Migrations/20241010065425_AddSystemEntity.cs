using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RDF.GL.Migrations
{
    /// <inheritdoc />
    public partial class AddSystemEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "systems",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    system_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    base_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    token = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_systems", x => x.id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "systems");

            migrationBuilder.UpdateData(
                table: "user_roles",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2024, 10, 4, 7, 0, 2, 825, DateTimeKind.Utc).AddTicks(7155), new DateTime(2024, 10, 4, 7, 0, 2, 825, DateTimeKind.Utc).AddTicks(7162) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "password", "updated_at" },
                values: new object[] { new DateTime(2024, 10, 4, 15, 0, 2, 825, DateTimeKind.Local).AddTicks(6134), "$2a$11$czTBznSjC/By6DAUFeJK/ODQJU41I9LYSI3BNqIUDB45zPvRGTBw6", new DateTime(2024, 10, 4, 7, 0, 2, 667, DateTimeKind.Utc).AddTicks(8680) });
        }
    }
}
