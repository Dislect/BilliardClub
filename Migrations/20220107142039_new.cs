using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BilliardClub.Migrations
{
    public partial class @new : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RentalHistories");

            migrationBuilder.DropTable(
                name: "RestaurantOrderHistories");

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    orderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    cheque = table.Column<double>(type: "float", nullable: false),
                    userId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.id);
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_userId",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderPoolTable",
                columns: table => new
                {
                    ordersid = table.Column<int>(type: "int", nullable: false),
                    poolTablesid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderPoolTable", x => new { x.ordersid, x.poolTablesid });
                    table.ForeignKey(
                        name: "FK_OrderPoolTable_Orders_ordersid",
                        column: x => x.ordersid,
                        principalTable: "Orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderPoolTable_PoolTables_poolTablesid",
                        column: x => x.poolTablesid,
                        principalTable: "PoolTables",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "IX_OrderPoolTable_poolTablesid",
                table: "OrderPoolTable",
                column: "poolTablesid");

            migrationBuilder.CreateIndex(
                name: "IX_OrderRestaurantMenu_restaurantMenuid",
                table: "OrderRestaurantMenu",
                column: "restaurantMenuid");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_userId",
                table: "Orders",
                column: "userId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderPoolTable");

            migrationBuilder.DropTable(
                name: "OrderRestaurantMenu");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.CreateTable(
                name: "RentalHistories",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    poolTableid = table.Column<int>(type: "int", nullable: true),
                    rentalTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    userId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentalHistories", x => x.id);
                    table.ForeignKey(
                        name: "FK_RentalHistories_AspNetUsers_userId",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RentalHistories_PoolTables_poolTableid",
                        column: x => x.poolTableid,
                        principalTable: "PoolTables",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RestaurantOrderHistories",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    dateOrder = table.Column<DateTime>(type: "datetime2", nullable: false),
                    restaurantMenuItemid = table.Column<int>(type: "int", nullable: true),
                    userId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestaurantOrderHistories", x => x.id);
                    table.ForeignKey(
                        name: "FK_RestaurantOrderHistories_AspNetUsers_userId",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RestaurantOrderHistories_RestaurantMenus_restaurantMenuItemid",
                        column: x => x.restaurantMenuItemid,
                        principalTable: "RestaurantMenus",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RentalHistories_poolTableid",
                table: "RentalHistories",
                column: "poolTableid");

            migrationBuilder.CreateIndex(
                name: "IX_RentalHistories_userId",
                table: "RentalHistories",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantOrderHistories_restaurantMenuItemid",
                table: "RestaurantOrderHistories",
                column: "restaurantMenuItemid");

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantOrderHistories_userId",
                table: "RestaurantOrderHistories",
                column: "userId");
        }
    }
}
