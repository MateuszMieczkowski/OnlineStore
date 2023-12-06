using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineStore.Server.Migrations
{
    /// <inheritdoc />
    public partial class RemoveProductFromOrderItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdersItems_Products_ProductId",
                table: "OrdersItems");

            migrationBuilder.DropIndex(
                name: "IX_OrdersItems_ProductId",
                table: "OrdersItems");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "OrdersItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "OrdersItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OrdersItems_ProductId",
                table: "OrdersItems",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrdersItems_Products_ProductId",
                table: "OrdersItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
