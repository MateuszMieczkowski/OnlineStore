using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SneakersBase.Server.Migrations
{
    public partial class WihoutImagesTest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThumbnailPath",
                table: "Products");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ThumbnailPath",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
