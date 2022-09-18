using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    public partial class qiao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pronun",
                schema: "dbo",
                table: "Words");

            migrationBuilder.AddColumn<bool>(
                name: "IsQiao",
                schema: "dbo",
                table: "Words",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsQiao",
                schema: "dbo",
                table: "Words");

            migrationBuilder.AddColumn<string>(
                name: "Pronun",
                schema: "dbo",
                table: "Words",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
