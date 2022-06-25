using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MangaWorks.DataAccess.Migrations
{
    public partial class mangamodelNumberOfRatings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberOfRatings",
                table: "Mangas",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfRatings",
                table: "Mangas");
        }
    }
}
