using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WomensWiki.Migrations
{
    /// <inheritdoc />
    public partial class MultipleParentTags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Tags_ParentTagId",
                table: "Tags");

            migrationBuilder.RenameColumn(
                name: "ParentTagId",
                table: "Tags",
                newName: "TagId");

            migrationBuilder.RenameIndex(
                name: "IX_Tags_ParentTagId",
                table: "Tags",
                newName: "IX_Tags_TagId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Tags_TagId",
                table: "Tags",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Tags_TagId",
                table: "Tags");

            migrationBuilder.RenameColumn(
                name: "TagId",
                table: "Tags",
                newName: "ParentTagId");

            migrationBuilder.RenameIndex(
                name: "IX_Tags_TagId",
                table: "Tags",
                newName: "IX_Tags_ParentTagId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Tags_ParentTagId",
                table: "Tags",
                column: "ParentTagId",
                principalTable: "Tags",
                principalColumn: "Id");
        }
    }
}
