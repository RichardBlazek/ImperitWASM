using Microsoft.EntityFrameworkCore.Migrations;

namespace ImperitWASM.Server.Migrations
{
    public partial class IM6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_Action_Players_PlayerName1", table: "Action");
            migrationBuilder.DropForeignKey(name: "FK_Action_Provinces_ProvinceId", table: "Action");

            migrationBuilder.DropIndex(name: "IX_Action_PlayerName1", table: "Action");
            migrationBuilder.DropColumn(name: "PlayerName1", table: "Action");

            migrationBuilder.AddForeignKey(name: "FK_Action_Provinces_ProvinceId",
                table: "Action", column: "ProvinceId", principalTable: "Provinces",
                principalColumn: "Id", onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Action_Provinces_ProvinceId",
                table: "Action");

            migrationBuilder.AddColumn<string>(
                name: "PlayerName1",
                table: "Action",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Action_PlayerName1",
                table: "Action",
                column: "PlayerName1");

            migrationBuilder.AddForeignKey(
                name: "FK_Action_Players_PlayerName1",
                table: "Action",
                column: "PlayerName1",
                principalTable: "Players",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Action_Provinces_ProvinceId",
                table: "Action",
                column: "ProvinceId",
                principalTable: "Provinces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
