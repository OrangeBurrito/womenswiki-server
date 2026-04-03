using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WomensWiki.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class TempFixUserRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "roles_id",
                table: "user_roles",
                newName: "role_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "role_id",
                table: "user_roles",
                newName: "roles_id");
        }
    }
}
