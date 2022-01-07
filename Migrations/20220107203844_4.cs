using Microsoft.EntityFrameworkCore.Migrations;

namespace BilliardClub.Migrations
{
    public partial class _4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cartItemId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.id);
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PoolTables_CartItems_CartItemid",
                table: "PoolTables");

            migrationBuilder.DropForeignKey(
                name: "FK_RestaurantMenus_CartItems_CartItemid",
                table: "RestaurantMenus");

            migrationBuilder.DropTable(
                name: "CartItems");

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
        }
    }
}
