using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RDF.GL.Migrations
{
    /// <inheritdoc />
    public partial class AdjustDecimalNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "unit_price",
                table: "general_ledgers",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "quantity",
                table: "general_ledgers",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "line_amount",
                table: "general_ledgers",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "unit_price",
                table: "general_ledgers",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "quantity",
                table: "general_ledgers",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "line_amount",
                table: "general_ledgers",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

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
    }
}
