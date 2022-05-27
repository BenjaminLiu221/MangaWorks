using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MangaWorks.DataAccess.Migrations
{
    public partial class updatepagetable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChapterId",
                table: "Pages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Pages_ChapterId",
                table: "Pages",
                column: "ChapterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pages_Chapters_ChapterId",
                table: "Pages",
                column: "ChapterId",
                principalTable: "Chapters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pages_Chapters_ChapterId",
                table: "Pages");

            migrationBuilder.DropIndex(
                name: "IX_Pages_ChapterId",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "ChapterId",
                table: "Pages");
        }
    }
}
