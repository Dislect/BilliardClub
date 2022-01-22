using Microsoft.EntityFrameworkCore.Migrations;

namespace BilliardClub.Migrations
{
    public partial class renameTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_RestaurantMenus_FoodItemid",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_FoodItemOrder_RestaurantMenus_restaurantMenuid",
                table: "FoodItemOrder");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RestaurantMenus",
                table: "RestaurantMenus");

            migrationBuilder.RenameTable(
                name: "RestaurantMenus",
                newName: "FoodItems");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FoodItems",
                table: "FoodItems",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_FoodItems_FoodItemid",
                table: "CartItems",
                column: "FoodItemid",
                principalTable: "FoodItems",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FoodItemOrder_FoodItems_restaurantMenuid",
                table: "FoodItemOrder",
                column: "restaurantMenuid",
                principalTable: "FoodItems",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_FoodItems_FoodItemid",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_FoodItemOrder_FoodItems_restaurantMenuid",
                table: "FoodItemOrder");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FoodItems",
                table: "FoodItems");

            migrationBuilder.RenameTable(
                name: "FoodItems",
                newName: "RestaurantMenus");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RestaurantMenus",
                table: "RestaurantMenus",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_RestaurantMenus_FoodItemid",
                table: "CartItems",
                column: "FoodItemid",
                principalTable: "RestaurantMenus",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FoodItemOrder_RestaurantMenus_restaurantMenuid",
                table: "FoodItemOrder",
                column: "restaurantMenuid",
                principalTable: "RestaurantMenus",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
