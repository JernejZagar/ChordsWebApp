using Microsoft.EntityFrameworkCore.Migrations;

namespace ChordsWebApp.Migrations
{
    public partial class Initialize4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Uploader",
                table: "Chords",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Uploader",
                table: "Chords");
        }
    }
}
