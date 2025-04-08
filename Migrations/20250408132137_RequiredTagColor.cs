using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WomensWiki.Migrations
{
    /// <inheritdoc />
    public partial class RequiredTagColor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Colors_ColorId",
                table: "Tags");

            migrationBuilder.AlterColumn<Guid>(
                name: "ColorId",
                table: "Tags",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Colors_ColorId",
                table: "Tags",
                column: "ColorId",
                principalTable: "Colors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Colors_ColorId",
                table: "Tags");

            migrationBuilder.AlterColumn<Guid>(
                name: "ColorId",
                table: "Tags",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Colors_ColorId",
                table: "Tags",
                column: "ColorId",
                principalTable: "Colors",
                principalColumn: "Id");
        }
    }
}
