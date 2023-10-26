using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WomensWikiServer.Migrations
{
    /// <inheritdoc />
    public partial class ModelNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Revisions_LatestRevisionId",
                table: "Articles");

            migrationBuilder.DropForeignKey(
                name: "FK_Revisions_Articles_ArticleId",
                table: "Revisions");

            migrationBuilder.AlterColumn<Guid>(
                name: "ArticleId",
                table: "Revisions",
                type: "uuid",
                nullable: true,
                defaultValue: new Guid("8e64f879-b48d-4ef8-a3b2-de512a02e90b"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValue: new Guid("d0ae039f-2938-4284-ba41-4d067e9714e8"));

            migrationBuilder.AlterColumn<Guid>(
                name: "LatestRevisionId",
                table: "Articles",
                type: "uuid",
                nullable: true,
                defaultValue: new Guid("06a23ca5-a20e-4484-80f6-59b72255622f"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValue: new Guid("390abcd2-53e1-4737-843f-6a36076e42bd"));

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Revisions_LatestRevisionId",
                table: "Articles",
                column: "LatestRevisionId",
                principalTable: "Revisions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Revisions_Articles_ArticleId",
                table: "Revisions",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Revisions_LatestRevisionId",
                table: "Articles");

            migrationBuilder.DropForeignKey(
                name: "FK_Revisions_Articles_ArticleId",
                table: "Revisions");

            migrationBuilder.AlterColumn<Guid>(
                name: "ArticleId",
                table: "Revisions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("d0ae039f-2938-4284-ba41-4d067e9714e8"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldDefaultValue: new Guid("8e64f879-b48d-4ef8-a3b2-de512a02e90b"));

            migrationBuilder.AlterColumn<Guid>(
                name: "LatestRevisionId",
                table: "Articles",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("390abcd2-53e1-4737-843f-6a36076e42bd"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldDefaultValue: new Guid("06a23ca5-a20e-4484-80f6-59b72255622f"));

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Revisions_LatestRevisionId",
                table: "Articles",
                column: "LatestRevisionId",
                principalTable: "Revisions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Revisions_Articles_ArticleId",
                table: "Revisions",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
