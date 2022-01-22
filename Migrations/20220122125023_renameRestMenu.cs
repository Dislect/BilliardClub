using Microsoft.EntityFrameworkCore.Migrations;

namespace BilliardClub.Migrations
{
    public partial class renameRestMenu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_RestaurantMenus_RestaurantMenuid",
                table: "CartItems");

            migrationBuilder.DropTable(
                name: "OrderRestaurantMenu");

            migrationBuilder.RenameColumn(
                name: "RestaurantMenuid",
                table: "CartItems",
                newName: "FoodItemid");

            migrationBuilder.RenameIndex(
                name: "IX_CartItems_RestaurantMenuid",
                table: "CartItems",
                newName: "IX_CartItems_FoodItemid");

            migrationBuilder.AddColumn<string>(
                name: "picturePath",
                table: "RestaurantMenus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FoodItemOrder",
                columns: table => new
                {
                    ordersid = table.Column<int>(type: "int", nullable: false),
                    restaurantMenuid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodItemOrder", x => new { x.ordersid, x.restaurantMenuid });
                    table.ForeignKey(
                        name: "FK_FoodItemOrder_Orders_ordersid",
                        column: x => x.ordersid,
                        principalTable: "Orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FoodItemOrder_RestaurantMenus_restaurantMenuid",
                        column: x => x.restaurantMenuid,
                        principalTable: "RestaurantMenus",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FoodItemOrder_restaurantMenuid",
                table: "FoodItemOrder",
                column: "restaurantMenuid");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_RestaurantMenus_FoodItemid",
                table: "CartItems",
                column: "FoodItemid",
                principalTable: "RestaurantMenus",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_RestaurantMenus_FoodItemid",
                table: "CartItems");

            migrationBuilder.DropTable(
                name: "FoodItemOrder");

            migrationBuilder.DropColumn(
                name: "picturePath",
                table: "RestaurantMenus");

            migrationBuilder.RenameColumn(
                name: "FoodItemid",
                table: "CartItems",
                newName: "RestaurantMenuid");

            migrationBuilder.RenameIndex(
                name: "IX_CartItems_FoodItemid",
                table: "CartItems",
                newName: "IX_CartItems_RestaurantMenuid");

            migrationBuilder.CreateTable(
                name: "OrderRestaurantMenu",
                columns: table => new
                {
                    ordersid = table.Column<int>(type: "int", nullable: false),
                    restaurantMenuid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderRestaurantMenu", x => new { x.ordersid, x.restaurantMenuid });
                    table.ForeignKey(
                        name: "FK_OrderRestaurantMenu_Orders_ordersid",
                        column: x => x.ordersid,
                        principalTable: "Orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderRestaurantMenu_RestaurantMenus_restaurantMenuid",
                        column: x => x.restaurantMenuid,
                        principalTable: "RestaurantMenus",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderRestaurantMenu_restaurantMenuid",
                table: "OrderRestaurantMenu",
                column: "restaurantMenuid");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_RestaurantMenus_RestaurantMenuid",
                table: "CartItems",
                column: "RestaurantMenuid",
                principalTable: "RestaurantMenus",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
