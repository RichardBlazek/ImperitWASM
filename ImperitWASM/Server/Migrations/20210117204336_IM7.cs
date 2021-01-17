using Microsoft.EntityFrameworkCore.Migrations;

namespace ImperitWASM.Server.Migrations
{
    public partial class IM7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_Action_Provinces_ProvinceId", table: "Action");
            migrationBuilder.DropForeignKey(name: "FK_Provinces_Soldiers_Id", table: "Provinces");

            migrationBuilder.DropPrimaryKey(name: "PK_Provinces", table: "Provinces");

            migrationBuilder.DropIndex(name: "IX_Provinces_GameId", table: "Provinces");
            migrationBuilder.DropIndex(name: "IX_Action_ProvinceId", table: "Action");

            migrationBuilder.RenameColumn(name: "Id", table: "Provinces", newName: "SoldiersId");
            migrationBuilder.RenameColumn(name: "ProvinceId", table: "Action", newName: "RegionId");

            migrationBuilder.AddPrimaryKey(name: "PK_Provinces", table: "Provinces", columns: new[] { "GameId", "RegionId" });

            migrationBuilder.CreateIndex(name: "IX_Provinces_SoldiersId", table: "Provinces", column: "SoldiersId", unique: true);

            migrationBuilder.AddForeignKey(name: "FK_Provinces_Soldiers_SoldiersId", table: "Provinces", column: "SoldiersId",
                principalTable: "Soldiers", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Provinces_Soldiers_SoldiersId",
                table: "Provinces");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Provinces",
                table: "Provinces");

            migrationBuilder.DropIndex(
                name: "IX_Provinces_SoldiersId",
                table: "Provinces");

            migrationBuilder.RenameColumn(
                name: "SoldiersId",
                table: "Provinces",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "RegionId",
                table: "Action",
                newName: "ProvinceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Provinces",
                table: "Provinces",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Provinces_GameId",
                table: "Provinces",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Action_ProvinceId",
                table: "Action",
                column: "ProvinceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Action_Provinces_ProvinceId",
                table: "Action",
                column: "ProvinceId",
                principalTable: "Provinces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Provinces_Soldiers_Id",
                table: "Provinces",
                column: "Id",
                principalTable: "Soldiers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
