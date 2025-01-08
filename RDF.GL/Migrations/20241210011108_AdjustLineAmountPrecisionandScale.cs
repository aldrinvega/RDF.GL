using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RDF.GL.Migrations
{
    /// <inheritdoc />
    public partial class AdjustLineAmountPrecisionandScale : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "line_amount",
                table: "general_ledgers",
                type: "decimal(20,10)",
                precision: 20,
                scale: 10,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "user_roles",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2024, 12, 10, 1, 11, 7, 965, DateTimeKind.Utc).AddTicks(4771), new DateTime(2024, 12, 10, 1, 11, 7, 965, DateTimeKind.Utc).AddTicks(4779) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "password", "updated_at" },
                values: new object[] { new DateTime(2024, 12, 10, 9, 11, 7, 965, DateTimeKind.Local).AddTicks(3873), "$2a$11$M6/biOIKiFedaQXOwee6t.4vgTtHI3.O3.RxCsOxnnsWIHPwz2ixq", new DateTime(2024, 12, 10, 1, 11, 7, 813, DateTimeKind.Utc).AddTicks(3189) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "line_amount",
                table: "general_ledgers",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,10)",
                oldPrecision: 20,
                oldScale: 10,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "user_roles",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2024, 11, 2, 13, 12, 1, 878, DateTimeKind.Utc).AddTicks(7406), new DateTime(2024, 11, 2, 13, 12, 1, 878, DateTimeKind.Utc).AddTicks(7415) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "password", "updated_at" },
                values: new object[] { new DateTime(2024, 11, 2, 21, 12, 1, 878, DateTimeKind.Local).AddTicks(6555), "$2a$11$0dtXK8PdkfCsWQT9WgtsnucEYUWs77U.KTaTQ85.P6i6m1B3NbQWG", new DateTime(2024, 11, 2, 13, 12, 1, 723, DateTimeKind.Utc).AddTicks(8639) });
        }
    }
}
