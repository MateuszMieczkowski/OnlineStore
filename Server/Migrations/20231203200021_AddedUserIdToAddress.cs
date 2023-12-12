using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineStore.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserIdToAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "OrdersAddresses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrdersAddresses_UserId",
                table: "OrdersAddresses",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrdersAddresses_Users_UserId",
                table: "OrdersAddresses",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdersAddresses_Users_UserId",
                table: "OrdersAddresses");

            migrationBuilder.DropIndex(
                name: "IX_OrdersAddresses_UserId",
                table: "OrdersAddresses");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "OrdersAddresses");
        }
    }
}
