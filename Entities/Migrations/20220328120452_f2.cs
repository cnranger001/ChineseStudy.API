using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    public partial class f2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "NeedMoreRepetition",
                schema: "dbo",
                table: "StudyHistory",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                schema: "dbo",
                table: "StudyHistory",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NeedMoreRepetition",
                schema: "dbo",
                table: "StudyHistory");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "dbo",
                table: "StudyHistory");
        }
    }
}
