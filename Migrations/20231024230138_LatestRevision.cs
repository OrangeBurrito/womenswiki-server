using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WomensWikiServer.Migrations
{
    /// <inheritdoc />
    public partial class LatestRevision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "ArticleId",
                table: "Revisions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("46622167-4b20-46af-ba9f-22351e4fee25"),
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "LatestRevisionId",
                table: "Articles",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("bd839b9f-9fb2-45ca-aca2-ca5d55fbd3fe"));

            migrationBuilder.CreateIndex(
                name: "IX_Articles_LatestRevisionId",
                table: "Articles",
                column: "LatestRevisionId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Revisions_LatestRevisionId",
                table: "Articles",
                column: "LatestRevisionId",
                principalTable: "Revisions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Revisions_LatestRevisionId",
                table: "Articles");

            migrationBuilder.DropIndex(
                name: "IX_Articles_LatestRevisionId",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "LatestRevisionId",
                table: "Articles");

            migrationBuilder.AlterColumn<Guid>(
                name: "ArticleId",
                table: "Revisions",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValue: new Guid("46622167-4b20-46af-ba9f-22351e4fee25"));
        }
    }
}
