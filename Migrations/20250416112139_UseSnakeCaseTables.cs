using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WomensWiki.Migrations
{
    /// <inheritdoc />
    public partial class UseSnakeCaseTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticleTag_Articles_ArticlesId",
                table: "ArticleTag");

            migrationBuilder.DropForeignKey(
                name: "FK_ArticleTag_Tags_TagsId",
                table: "ArticleTag");

            migrationBuilder.DropForeignKey(
                name: "FK_ChildTags_Tags_ParentTagsId",
                table: "ChildTags");

            migrationBuilder.DropForeignKey(
                name: "FK_ChildTags_Tags_TagId",
                table: "ChildTags");

            migrationBuilder.DropForeignKey(
                name: "FK_Revisions_Articles_ArticleId",
                table: "Revisions");

            migrationBuilder.DropForeignKey(
                name: "FK_Revisions_Users_AuthorId",
                table: "Revisions");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Colors_ColorId",
                table: "Tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tags",
                table: "Tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Revisions",
                table: "Revisions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Colors",
                table: "Colors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChildTags",
                table: "ChildTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Articles",
                table: "Articles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ArticleTag",
                table: "ArticleTag");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "users");

            migrationBuilder.RenameTable(
                name: "Tags",
                newName: "tags");

            migrationBuilder.RenameTable(
                name: "Revisions",
                newName: "revisions");

            migrationBuilder.RenameTable(
                name: "Colors",
                newName: "colors");

            migrationBuilder.RenameTable(
                name: "Articles",
                newName: "articles");

            migrationBuilder.RenameTable(
                name: "ArticleTag",
                newName: "ArticleTags");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "users",
                newName: "username");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "users",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "users",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "users",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "tags",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "tags",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "tags",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "ColorId",
                table: "tags",
                newName: "color_id");

            migrationBuilder.RenameIndex(
                name: "IX_Tags_ColorId",
                table: "tags",
                newName: "ix_tags_color_id");

            migrationBuilder.RenameColumn(
                name: "Summary",
                table: "revisions",
                newName: "summary");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "revisions",
                newName: "content");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "revisions",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "revisions",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "revisions",
                newName: "author_id");

            migrationBuilder.RenameColumn(
                name: "ArticleId",
                table: "revisions",
                newName: "article_id");

            migrationBuilder.RenameIndex(
                name: "IX_Revisions_AuthorId",
                table: "revisions",
                newName: "ix_revisions_author_id");

            migrationBuilder.RenameIndex(
                name: "IX_Revisions_ArticleId",
                table: "revisions",
                newName: "ix_revisions_article_id");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "colors",
                newName: "value");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "colors",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "colors",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "colors",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "TagId",
                table: "ChildTags",
                newName: "tag_id");

            migrationBuilder.RenameColumn(
                name: "ParentTagsId",
                table: "ChildTags",
                newName: "parent_tags_id");

            migrationBuilder.RenameIndex(
                name: "IX_ChildTags_TagId",
                table: "ChildTags",
                newName: "ix_child_tags_tag_id");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "articles",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "articles",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "articles",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "LatestVersion",
                table: "articles",
                newName: "latest_version");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "articles",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "TagsId",
                table: "ArticleTags",
                newName: "tags_id");

            migrationBuilder.RenameColumn(
                name: "ArticlesId",
                table: "ArticleTags",
                newName: "articles_id");

            migrationBuilder.RenameIndex(
                name: "IX_ArticleTag_TagsId",
                table: "ArticleTags",
                newName: "ix_article_tags_tags_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_users",
                table: "users",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_tags",
                table: "tags",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_revisions",
                table: "revisions",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_colors",
                table: "colors",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_child_tags",
                table: "ChildTags",
                columns: new[] { "parent_tags_id", "tag_id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_articles",
                table: "articles",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_article_tags",
                table: "ArticleTags",
                columns: new[] { "articles_id", "tags_id" });

            migrationBuilder.AddForeignKey(
                name: "fk_article_tags_articles_articles_id",
                table: "ArticleTags",
                column: "articles_id",
                principalTable: "articles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_article_tags_tags_tags_id",
                table: "ArticleTags",
                column: "tags_id",
                principalTable: "tags",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_child_tags_tags_parent_tags_id",
                table: "ChildTags",
                column: "parent_tags_id",
                principalTable: "tags",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_child_tags_tags_tag_id",
                table: "ChildTags",
                column: "tag_id",
                principalTable: "tags",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_revisions_articles_article_id",
                table: "revisions",
                column: "article_id",
                principalTable: "articles",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_revisions_users_author_id",
                table: "revisions",
                column: "author_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_tags_colors_color_id",
                table: "tags",
                column: "color_id",
                principalTable: "colors",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_article_tags_articles_articles_id",
                table: "ArticleTags");

            migrationBuilder.DropForeignKey(
                name: "fk_article_tags_tags_tags_id",
                table: "ArticleTags");

            migrationBuilder.DropForeignKey(
                name: "fk_child_tags_tags_parent_tags_id",
                table: "ChildTags");

            migrationBuilder.DropForeignKey(
                name: "fk_child_tags_tags_tag_id",
                table: "ChildTags");

            migrationBuilder.DropForeignKey(
                name: "fk_revisions_articles_article_id",
                table: "revisions");

            migrationBuilder.DropForeignKey(
                name: "fk_revisions_users_author_id",
                table: "revisions");

            migrationBuilder.DropForeignKey(
                name: "fk_tags_colors_color_id",
                table: "tags");

            migrationBuilder.DropPrimaryKey(
                name: "pk_users",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "pk_tags",
                table: "tags");

            migrationBuilder.DropPrimaryKey(
                name: "pk_revisions",
                table: "revisions");

            migrationBuilder.DropPrimaryKey(
                name: "pk_colors",
                table: "colors");

            migrationBuilder.DropPrimaryKey(
                name: "pk_child_tags",
                table: "ChildTags");

            migrationBuilder.DropPrimaryKey(
                name: "pk_articles",
                table: "articles");

            migrationBuilder.DropPrimaryKey(
                name: "pk_article_tags",
                table: "ArticleTags");

            migrationBuilder.RenameTable(
                name: "users",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "tags",
                newName: "Tags");

            migrationBuilder.RenameTable(
                name: "revisions",
                newName: "Revisions");

            migrationBuilder.RenameTable(
                name: "colors",
                newName: "Colors");

            migrationBuilder.RenameTable(
                name: "articles",
                newName: "Articles");

            migrationBuilder.RenameTable(
                name: "ArticleTags",
                newName: "ArticleTag");

            migrationBuilder.RenameColumn(
                name: "username",
                table: "Users",
                newName: "Username");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Users",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Users",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Users",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Tags",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Tags",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Tags",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "color_id",
                table: "Tags",
                newName: "ColorId");

            migrationBuilder.RenameIndex(
                name: "ix_tags_color_id",
                table: "Tags",
                newName: "IX_Tags_ColorId");

            migrationBuilder.RenameColumn(
                name: "summary",
                table: "Revisions",
                newName: "Summary");

            migrationBuilder.RenameColumn(
                name: "content",
                table: "Revisions",
                newName: "Content");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Revisions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Revisions",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "author_id",
                table: "Revisions",
                newName: "AuthorId");

            migrationBuilder.RenameColumn(
                name: "article_id",
                table: "Revisions",
                newName: "ArticleId");

            migrationBuilder.RenameIndex(
                name: "ix_revisions_author_id",
                table: "Revisions",
                newName: "IX_Revisions_AuthorId");

            migrationBuilder.RenameIndex(
                name: "ix_revisions_article_id",
                table: "Revisions",
                newName: "IX_Revisions_ArticleId");

            migrationBuilder.RenameColumn(
                name: "value",
                table: "Colors",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Colors",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Colors",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Colors",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "tag_id",
                table: "ChildTags",
                newName: "TagId");

            migrationBuilder.RenameColumn(
                name: "parent_tags_id",
                table: "ChildTags",
                newName: "ParentTagsId");

            migrationBuilder.RenameIndex(
                name: "ix_child_tags_tag_id",
                table: "ChildTags",
                newName: "IX_ChildTags_TagId");

            migrationBuilder.RenameColumn(
                name: "title",
                table: "Articles",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Articles",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "Articles",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "latest_version",
                table: "Articles",
                newName: "LatestVersion");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Articles",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "tags_id",
                table: "ArticleTag",
                newName: "TagsId");

            migrationBuilder.RenameColumn(
                name: "articles_id",
                table: "ArticleTag",
                newName: "ArticlesId");

            migrationBuilder.RenameIndex(
                name: "ix_article_tags_tags_id",
                table: "ArticleTag",
                newName: "IX_ArticleTag_TagsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tags",
                table: "Tags",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Revisions",
                table: "Revisions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Colors",
                table: "Colors",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChildTags",
                table: "ChildTags",
                columns: new[] { "ParentTagsId", "TagId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Articles",
                table: "Articles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ArticleTag",
                table: "ArticleTag",
                columns: new[] { "ArticlesId", "TagsId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleTag_Articles_ArticlesId",
                table: "ArticleTag",
                column: "ArticlesId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleTag_Tags_TagsId",
                table: "ArticleTag",
                column: "TagsId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChildTags_Tags_ParentTagsId",
                table: "ChildTags",
                column: "ParentTagsId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChildTags_Tags_TagId",
                table: "ChildTags",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Revisions_Articles_ArticleId",
                table: "Revisions",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Revisions_Users_AuthorId",
                table: "Revisions",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Colors_ColorId",
                table: "Tags",
                column: "ColorId",
                principalTable: "Colors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
