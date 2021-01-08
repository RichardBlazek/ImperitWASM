using Microsoft.EntityFrameworkCore.Migrations;

namespace ImperitWASM.Server.Migrations
{
    public partial class IM3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Region_Soldiers_Id",
                table: "Region");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Region",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "SoldiersId",
                table: "Region",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Region_SoldiersId",
                table: "Region",
                column: "SoldiersId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Region_Soldiers_SoldiersId",
                table: "Region",
                column: "SoldiersId",
                principalTable: "Soldiers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Region_Soldiers_SoldiersId",
                table: "Region");

            migrationBuilder.DropIndex(
                name: "IX_Region_SoldiersId",
                table: "Region");

            migrationBuilder.DropColumn(
                name: "SoldiersId",
                table: "Region");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Region",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddForeignKey(
                name: "FK_Region_Soldiers_Id",
                table: "Region",
                column: "Id",
                principalTable: "Soldiers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
