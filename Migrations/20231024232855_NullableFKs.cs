using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WomensWikiServer.Migrations
{
    /// <inheritdoc />
    public partial class NullableFKs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "ArticleId",
                table: "Revisions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("d0ae039f-2938-4284-ba41-4d067e9714e8"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValue: new Guid("46622167-4b20-46af-ba9f-22351e4fee25"));

            migrationBuilder.AlterColumn<Guid>(
                name: "LatestRevisionId",
                table: "Articles",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("390abcd2-53e1-4737-843f-6a36076e42bd"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValue: new Guid("bd839b9f-9fb2-45ca-aca2-ca5d55fbd3fe"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "ArticleId",
                table: "Revisions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("46622167-4b20-46af-ba9f-22351e4fee25"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValue: new Guid("d0ae039f-2938-4284-ba41-4d067e9714e8"));

            migrationBuilder.AlterColumn<Guid>(
                name: "LatestRevisionId",
                table: "Articles",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("bd839b9f-9fb2-45ca-aca2-ca5d55fbd3fe"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValue: new Guid("390abcd2-53e1-4737-843f-6a36076e42bd"));
        }
    }
}
