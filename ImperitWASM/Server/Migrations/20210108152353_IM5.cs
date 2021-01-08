using Microsoft.EntityFrameworkCore.Migrations;

namespace ImperitWASM.Server.Migrations
{
    public partial class IM5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Elephant_Capacity",
                table: "SoldierType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Elephant_Capacity",
                table: "SoldierType",
                type: "INTEGER",
                nullable: true);
        }
    }
}
