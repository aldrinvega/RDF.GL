using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RDF.GL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "user_roles",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_role_name = table.Column<string>(type: "text", nullable: false),
                    permissions = table.Column<string[]>(type: "text[]", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    added_by = table.Column<int>(type: "integer", nullable: false),
                    modified_by = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fullname = table.Column<string>(type: "text", nullable: true),
                    username = table.Column<string>(type: "text", nullable: true),
                    password = table.Column<string>(type: "text", nullable: true),
                    user_role_id = table.Column<int>(type: "integer", nullable: true)
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
                name: "users");

            migrationBuilder.DropTable(
                name: "user_roles");
        }
    }
}
