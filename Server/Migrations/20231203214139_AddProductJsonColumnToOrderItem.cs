using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineStore.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddProductJsonColumnToOrderItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Product",
                table: "OrdersItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Product",
                table: "OrdersItems");
        }
    }
}
