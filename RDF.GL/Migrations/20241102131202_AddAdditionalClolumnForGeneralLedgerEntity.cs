using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RDF.GL.Migrations
{
    /// <inheritdoc />
    public partial class AddAdditionalClolumnForGeneralLedgerEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "bank_name",
                table: "general_ledgers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "cheque_number",
                table: "general_ledgers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "cheque_voucher_number",
                table: "general_ledgers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "rr_number",
                table: "general_ledgers",
                type: "nvarchar(max)",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "bank_name",
                table: "general_ledgers");

            migrationBuilder.DropColumn(
                name: "cheque_number",
                table: "general_ledgers");

            migrationBuilder.DropColumn(
                name: "cheque_voucher_number",
                table: "general_ledgers");

            migrationBuilder.DropColumn(
                name: "rr_number",
                table: "general_ledgers");

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
    }
}
