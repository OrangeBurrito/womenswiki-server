using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WomensWiki.Migrations
{
    /// <inheritdoc />
    public partial class SpecifySummary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Summary",
                table: "Revisions",
                type: "nvarchar(72)",
                maxLength: 72,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Summary",
                table: "Revisions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(72)",
                oldMaxLength: 72,
                oldNullable: true);
        }
    }
}
