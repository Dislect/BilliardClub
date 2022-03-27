using Microsoft.EntityFrameworkCore.Migrations;

namespace BilliardClub.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderPoolTables_Orders_orderid",
                table: "OrderPoolTables");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderPoolTables_PoolTables_poolTableid",
                table: "OrderPoolTables");

            migrationBuilder.AlterColumn<int>(
                name: "poolTableid",
                table: "OrderPoolTables",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "orderid",
                table: "OrderPoolTables",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderPoolTables_Orders_orderid",
                table: "OrderPoolTables",
                column: "orderid",
                principalTable: "Orders",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderPoolTables_PoolTables_poolTableid",
                table: "OrderPoolTables",
                column: "poolTableid",
                principalTable: "PoolTables",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderPoolTables_Orders_orderid",
                table: "OrderPoolTables");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderPoolTables_PoolTables_poolTableid",
                table: "OrderPoolTables");

            migrationBuilder.AlterColumn<int>(
                name: "poolTableid",
                table: "OrderPoolTables",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "orderid",
                table: "OrderPoolTables",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderPoolTables_Orders_orderid",
                table: "OrderPoolTables",
                column: "orderid",
                principalTable: "Orders",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderPoolTables_PoolTables_poolTableid",
                table: "OrderPoolTables",
                column: "poolTableid",
                principalTable: "PoolTables",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
