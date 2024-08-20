using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WomensWiki.Migrations
{
    /// <inheritdoc />
    public partial class ParentTagMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Tags_TagId",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_TagId",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "TagId",
                table: "Tags");

            migrationBuilder.CreateTable(
                name: "TagTag",
                columns: table => new
                {
                    ParentTagsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TagId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagTag", x => new { x.ParentTagsId, x.TagId });
                    table.ForeignKey(
                        name: "FK_TagTag_Tags_ParentTagsId",
                        column: x => x.ParentTagsId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TagTag_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TagTag_TagId",
                table: "TagTag",
                column: "TagId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TagTag");

            migrationBuilder.AddColumn<Guid>(
                name: "TagId",
                table: "Tags",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tags_TagId",
                table: "Tags",
                column: "TagId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Tags_TagId",
                table: "Tags",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id");
        }
    }
}
