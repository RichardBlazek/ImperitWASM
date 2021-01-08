using Microsoft.EntityFrameworkCore.Migrations;

namespace ImperitWASM.Server.Migrations
{
    public partial class IM2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropForeignKey(name: "FK_SoldierType_Soldiers_SoldiersId",table: "SoldierType");
            _ = migrationBuilder.DropIndex(name: "IX_SoldierType_SoldiersId", table: "SoldierType");
            _ = migrationBuilder.DropColumn(name: "SoldiersId", table: "SoldierType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SoldiersId",
                table: "SoldierType",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SoldierType_SoldiersId",
                table: "SoldierType",
                column: "SoldiersId");

            migrationBuilder.AddForeignKey(
                name: "FK_SoldierType_Soldiers_SoldiersId",
                table: "SoldierType",
                column: "SoldiersId",
                principalTable: "Soldiers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
