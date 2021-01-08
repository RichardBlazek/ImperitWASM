using Microsoft.EntityFrameworkCore.Migrations;

namespace ImperitWASM.Server.Migrations
{
    public partial class IM4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Regiment_Action_ManoeuvreId",
                table: "Regiment");

            migrationBuilder.DropForeignKey(
                name: "FK_Regiment_Provinces_ProvinceId",
                table: "Regiment");

            migrationBuilder.DropIndex(
                name: "IX_Regiment_ManoeuvreId",
                table: "Regiment");

            migrationBuilder.DropIndex(
                name: "IX_Regiment_ProvinceId",
                table: "Regiment");

            migrationBuilder.DropColumn(
                name: "ManoeuvreId",
                table: "Regiment");

            migrationBuilder.DropColumn(
                name: "ProvinceId",
                table: "Regiment");

            migrationBuilder.AddColumn<int>(
                name: "SoldiersId",
                table: "Action",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Action_SoldiersId",
                table: "Action",
                column: "SoldiersId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Action_Soldiers_SoldiersId",
                table: "Action",
                column: "SoldiersId",
                principalTable: "Soldiers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Action_Soldiers_SoldiersId",
                table: "Action");

            migrationBuilder.DropIndex(
                name: "IX_Action_SoldiersId",
                table: "Action");

            migrationBuilder.DropColumn(
                name: "SoldiersId",
                table: "Action");

            migrationBuilder.AddColumn<int>(
                name: "ManoeuvreId",
                table: "Regiment",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProvinceId",
                table: "Regiment",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Regiment_ManoeuvreId",
                table: "Regiment",
                column: "ManoeuvreId");

            migrationBuilder.CreateIndex(
                name: "IX_Regiment_ProvinceId",
                table: "Regiment",
                column: "ProvinceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Regiment_Action_ManoeuvreId",
                table: "Regiment",
                column: "ManoeuvreId",
                principalTable: "Action",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Regiment_Provinces_ProvinceId",
                table: "Regiment",
                column: "ProvinceId",
                principalTable: "Provinces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
