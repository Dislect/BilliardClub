using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BilliardClub.Migrations
{
    public partial class newTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.RenameColumn(
                name: "numberHours",
                table: "OrderFoodItems",
                newName: "quantity");

            migrationBuilder.CreateTable(
                name: "CartFoodItems",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FoodItemid = table.Column<int>(type: "int", nullable: true),
                    quantity = table.Column<long>(type: "bigint", nullable: false),
                    cartId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartFoodItems", x => x.id);
                    table.ForeignKey(
                        name: "FK_CartFoodItems_FoodItems_FoodItemid",
                        column: x => x.FoodItemid,
                        principalTable: "FoodItems",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CartPoolTables",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PoolTableid = table.Column<int>(type: "int", nullable: true),
                    numberHours = table.Column<long>(type: "bigint", nullable: false),
                    reservationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    cartId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartPoolTables", x => x.id);
                    table.ForeignKey(
                        name: "FK_CartPoolTables_PoolTables_PoolTableid",
                        column: x => x.PoolTableid,
                        principalTable: "PoolTables",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartFoodItems_FoodItemid",
                table: "CartFoodItems",
                column: "FoodItemid");

            migrationBuilder.CreateIndex(
                name: "IX_CartPoolTables_PoolTableid",
                table: "CartPoolTables",
                column: "PoolTableid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartFoodItems");

            migrationBuilder.DropTable(
                name: "CartPoolTables");

            migrationBuilder.RenameColumn(
                name: "quantity",
                table: "OrderFoodItems",
                newName: "numberHours");

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FoodItemid = table.Column<int>(type: "int", nullable: true),
                    PoolTableid = table.Column<int>(type: "int", nullable: true),
                    cartId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    numberHours = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.id);
                    table.ForeignKey(
                        name: "FK_CartItems_FoodItems_FoodItemid",
                        column: x => x.FoodItemid,
                        principalTable: "FoodItems",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CartItems_PoolTables_PoolTableid",
                        column: x => x.PoolTableid,
                        principalTable: "PoolTables",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_FoodItemid",
                table: "CartItems",
                column: "FoodItemid");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_PoolTableid",
                table: "CartItems",
                column: "PoolTableid");
        }
    }
}
