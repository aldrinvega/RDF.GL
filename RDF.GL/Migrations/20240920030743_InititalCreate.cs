using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RDF.GL.Migrations
{
    /// <inheritdoc />
    public partial class InititalCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "general_ledger",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sync_id = table.Column<int>(type: "int", nullable: false),
                    mark1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    asset_cip = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    accounting_tag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    transaction_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    client_supplier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    account_title_code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    account_title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    company_code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    company = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    division_code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    division = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    department_code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    department = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    unit_code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    unit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sub_unit_code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sub_unit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    location_code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    po_number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    reference_no = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    item_code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    item_description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    uom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    unit_price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    line_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    voucher_journal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    account_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    drcp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    asset_code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    asset = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    service_provider_code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    service_provider = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    boa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    allocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    account_group = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    account_sub_group = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    financial_statement = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    unit_responsible = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    batch = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    payroll_period = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    position = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    payroll_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    payroll_type2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    depreciation_description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    remaining_depreciation_value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    useful_life = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    month = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    year = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    particulars = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    month2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    farm_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    jean_remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    from = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    change_to = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    checking_remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    boa2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    system = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    books = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    added_by = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_general_ledger", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_roles",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_role_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    permissions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    added_by = table.Column<int>(type: "int", nullable: false),
                    modified_by = table.Column<int>(type: "int", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_prefix = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    id_number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    first_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    middle_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    last_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    user_role_id = table.Column<int>(type: "int", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                    table.ForeignKey(
                        name: "fk_users_user_roles_user_role_id",
                        column: x => x.user_role_id,
                        principalTable: "user_roles",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_general_ledger_added_by",
                table: "general_ledger",
                column: "added_by");

            migrationBuilder.CreateIndex(
                name: "ix_user_roles_added_by",
                table: "user_roles",
                column: "added_by");

            migrationBuilder.CreateIndex(
                name: "ix_user_roles_modified_by",
                table: "user_roles",
                column: "modified_by");

            migrationBuilder.CreateIndex(
                name: "ix_users_user_role_id",
                table: "users",
                column: "user_role_id");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_user_roles_users_added_by",
                table: "user_roles");

            migrationBuilder.DropForeignKey(
                name: "fk_user_roles_users_modified_by",
                table: "user_roles");

            migrationBuilder.DropTable(
                name: "general_ledger");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "user_roles");
        }
    }
}
