using Microsoft.EntityFrameworkCore.Migrations;

namespace ChessInfo.Repository.Migrations
{
    public partial class OpeningClassification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OpeningClassification",
                table: "Games",
                maxLength: 3,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OpeningClassification",
                table: "Games");
        }
    }
}
