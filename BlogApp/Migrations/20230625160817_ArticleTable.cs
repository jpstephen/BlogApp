using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogApp.Migrations
{
    public partial class ArticleTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AllArticles",
                columns: table => new
                {
                    ArticleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArticleTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ArticleCreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ArticleContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ArticleCategory = table.Column<int>(type: "int", nullable: false),
                    ArticleThumbNail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsArticleDeleted = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ArticleDeletedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ShowInHome = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllArticles", x => x.ArticleId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AllArticles");
        }
    }
}
