using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RDF.GL.Migrations
{
    /// <inheritdoc />
    public partial class AdjustGeneralLedgerReportsSyncIdtoString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "sync_id",
                table: "general_ledgers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "sync_id",
                table: "general_ledgers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "user_roles",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2024, 9, 25, 1, 9, 28, 636, DateTimeKind.Utc).AddTicks(4176), new DateTime(2024, 9, 25, 1, 9, 28, 636, DateTimeKind.Utc).AddTicks(4182) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "password", "updated_at" },
                values: new object[] { new DateTime(2024, 9, 25, 9, 9, 28, 636, DateTimeKind.Local).AddTicks(3036), "$2a$11$tQRdT7sMfdaASs5uw1uFH.8TFWsMP/Nq98AjUQVmnblWAiiEwmR/y", new DateTime(2024, 9, 25, 1, 9, 28, 478, DateTimeKind.Utc).AddTicks(3882) });
        }
    }
}
