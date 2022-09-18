using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    public partial class article : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_Poem",
            //    table: "Poem");

            //migrationBuilder.RenameTable(
            //    name: "Poem",
            //    newName: "Poems");

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_Poems",
            //    table: "Poems",
            //    column: "Id");

            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Poems",
                table: "Poems");

            migrationBuilder.RenameTable(
                name: "Poems",
                newName: "Poem");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Poem",
                table: "Poem",
                column: "Id");
        }
    }
}
