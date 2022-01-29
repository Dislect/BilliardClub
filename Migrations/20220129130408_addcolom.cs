using Microsoft.EntityFrameworkCore.Migrations;

namespace BilliardClub.Migrations
{
    public partial class addcolom : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "idOrder",
                table: "PoolTables",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "idStatus",
                table: "PoolTables",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "idOrder",
                table: "PoolTables");

            migrationBuilder.DropColumn(
                name: "idStatus",
                table: "PoolTables");
        }
    }
}
