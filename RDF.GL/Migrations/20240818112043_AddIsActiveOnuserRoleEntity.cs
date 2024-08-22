using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RDF.GL.Migrations
{
    /// <inheritdoc />
    public partial class AddIsActiveOnuserRoleEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "user_roles",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_active",
                table: "user_roles");
        }
    }
}
