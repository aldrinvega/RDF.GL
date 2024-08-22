using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RDF.GL.Migrations
{
    /// <inheritdoc />
    public partial class Adjustusername : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "user_name",
                table: "users",
                newName: "username");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "username",
                table: "users",
                newName: "user_name");
        }
    }
}
