using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RDF.GL.Migrations
{
    /// <inheritdoc />
    public partial class AdjustGeneralLedger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "mark2",
                table: "general_ledgers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "user_roles",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2024, 9, 23, 8, 41, 31, 675, DateTimeKind.Utc).AddTicks(8222), new DateTime(2024, 9, 23, 8, 41, 31, 675, DateTimeKind.Utc).AddTicks(8229) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "password", "updated_at" },
                values: new object[] { new DateTime(2024, 9, 23, 16, 41, 31, 675, DateTimeKind.Local).AddTicks(7252), "$2a$11$EWzGumCMvOuxwC3u..MVXuOaAtnueFhBjM9b5m.1TybZkNcx4nCoy", new DateTime(2024, 9, 23, 8, 41, 31, 518, DateTimeKind.Utc).AddTicks(9483) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "mark2",
                table: "general_ledgers");

            migrationBuilder.UpdateData(
                table: "user_roles",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2024, 9, 20, 3, 40, 0, 537, DateTimeKind.Utc).AddTicks(2959), new DateTime(2024, 9, 20, 3, 40, 0, 537, DateTimeKind.Utc).AddTicks(2965) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "password", "updated_at" },
                values: new object[] { new DateTime(2024, 9, 20, 11, 40, 0, 537, DateTimeKind.Local).AddTicks(1984), "$2a$11$8.6uD4qL1jUNjr.V1PvFreyODLX9cezwSTK.X4i5I.083XdVj6A/q", new DateTime(2024, 9, 20, 3, 40, 0, 381, DateTimeKind.Utc).AddTicks(9179) });
        }
    }
}
