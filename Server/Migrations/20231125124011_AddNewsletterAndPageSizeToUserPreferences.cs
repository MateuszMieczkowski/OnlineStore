using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineStore.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddNewsletterAndPageSizeToUserPreferences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSubscribedToNewsletter",
                table: "Users");

            migrationBuilder.AddColumn<bool>(
                name: "IsSubscribedToNewsLetter",
                table: "UserPreferences",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PageSize",
                table: "UserPreferences",
                type: "int",
                nullable: false,
                defaultValue: 20);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSubscribedToNewsLetter",
                table: "UserPreferences");

            migrationBuilder.DropColumn(
                name: "PageSize",
                table: "UserPreferences");

            migrationBuilder.AddColumn<bool>(
                name: "IsSubscribedToNewsletter",
                table: "Users",
                type: "bit",
                nullable: true);
        }
    }
}
