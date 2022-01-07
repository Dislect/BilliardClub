using Microsoft.EntityFrameworkCore.Migrations;

namespace BilliardClub.Migrations
{
    public partial class _5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PoolTables_CartItems_CartItemid",
                table: "PoolTables");

            migrationBuilder.DropForeignKey(
                name: "FK_RestaurantMenus_CartItems_CartItemid",
                table: "RestaurantMenus");

            migrationBuilder.DropIndex(
                name: "IX_RestaurantMenus_CartItemid",
                table: "RestaurantMenus");

            migrationBuilder.DropIndex(
                name: "IX_PoolTables_CartItemid",
                table: "PoolTables");

            migrationBuilder.DropColumn(
                name: "CartItemid",
                table: "RestaurantMenus");

            migrationBuilder.DropColumn(
                name: "CartItemid",
                table: "PoolTables");

            migrationBuilder.RenameColumn(
                name: "nane",
                table: "Status",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "nane",
                table: "PoolTables",
                newName: "name");

            migrationBuilder.AddColumn<int>(
                name: "PoolTableid",
                table: "CartItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RestaurantMenuid",
                table: "CartItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_PoolTableid",
                table: "CartItems",
                column: "PoolTableid");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_RestaurantMenuid",
                table: "CartItems",
                column: "RestaurantMenuid");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_PoolTables_PoolTableid",
                table: "CartItems",
                column: "PoolTableid",
                principalTable: "PoolTables",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_RestaurantMenus_RestaurantMenuid",
                table: "CartItems",
                column: "RestaurantMenuid",
                principalTable: "RestaurantMenus",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_PoolTables_PoolTableid",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_RestaurantMenus_RestaurantMenuid",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_PoolTableid",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_RestaurantMenuid",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "PoolTableid",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "RestaurantMenuid",
                table: "CartItems");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Status",
                newName: "nane");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "PoolTables",
                newName: "nane");

            migrationBuilder.AddColumn<int>(
                name: "CartItemid",
                table: "RestaurantMenus",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CartItemid",
                table: "PoolTables",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantMenus_CartItemid",
                table: "RestaurantMenus",
                column: "CartItemid");

            migrationBuilder.CreateIndex(
                name: "IX_PoolTables_CartItemid",
                table: "PoolTables",
                column: "CartItemid");

            migrationBuilder.AddForeignKey(
                name: "FK_PoolTables_CartItems_CartItemid",
                table: "PoolTables",
                column: "CartItemid",
                principalTable: "CartItems",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RestaurantMenus_CartItems_CartItemid",
                table: "RestaurantMenus",
                column: "CartItemid",
                principalTable: "CartItems",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
