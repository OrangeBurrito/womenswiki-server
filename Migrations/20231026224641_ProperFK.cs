using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WomensWikiServer.Migrations
{
    /// <inheritdoc />
    public partial class ProperFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Revisions_Articles_ArticleId",
                table: "Revisions");

            migrationBuilder.DropIndex(
                name: "IX_Revisions_ArticleId",
                table: "Revisions");

            migrationBuilder.DropColumn(
                name: "ArticleId",
                table: "Revisions");

            migrationBuilder.AlterColumn<Guid>(
                name: "LatestRevisionId",
                table: "Articles",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldDefaultValue: new Guid("06a23ca5-a20e-4484-80f6-59b72255622f"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ArticleId",
                table: "Revisions",
                type: "uuid",
                nullable: true,
                defaultValue: new Guid("8e64f879-b48d-4ef8-a3b2-de512a02e90b"));

            migrationBuilder.AlterColumn<Guid>(
                name: "LatestRevisionId",
                table: "Articles",
                type: "uuid",
                nullable: true,
                defaultValue: new Guid("06a23ca5-a20e-4484-80f6-59b72255622f"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Revisions_ArticleId",
                table: "Revisions",
                column: "ArticleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Revisions_Articles_ArticleId",
                table: "Revisions",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id");
        }
    }
}
