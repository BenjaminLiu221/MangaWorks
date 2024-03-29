﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MangaWorks.DataAccess.Migrations
{
    public partial class chapterViews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Views",
                table: "Chapters",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Views",
                table: "Chapters");
        }
    }
}
