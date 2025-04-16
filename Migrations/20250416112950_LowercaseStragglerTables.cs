using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WomensWiki.Migrations
{
    /// <inheritdoc />
    public partial class LowercaseStragglerTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "ChildTags",
                newName: "child_tags");

            migrationBuilder.RenameTable(
                name: "ArticleTags",
                newName: "article_tags");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "child_tags",
                newName: "ChildTags");

            migrationBuilder.RenameTable(
                name: "article_tags",
                newName: "ArticleTags");
        }
    }
}
