using Microsoft.EntityFrameworkCore.Migrations;

namespace ImperitWASM.Server.Migrations
{
    public partial class IM8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Settings_SettingsId",
                table: "Players");

            migrationBuilder.DropForeignKey(
                name: "FK_Provinces_Settings_SettingsId",
                table: "Provinces");

            migrationBuilder.DropForeignKey(
                name: "FK_Region_Settings_SettingsId",
                table: "Region");

            migrationBuilder.DropForeignKey(
                name: "FK_SoldierType_Settings_SettingsId",
                table: "SoldierType");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropIndex(
                name: "IX_SoldierType_SettingsId",
                table: "SoldierType");

            migrationBuilder.DropIndex(
                name: "IX_Region_SettingsId",
                table: "Region");

            migrationBuilder.DropIndex(
                name: "IX_Provinces_SettingsId",
                table: "Provinces");

            migrationBuilder.DropIndex(
                name: "IX_Players_SettingsId",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "SettingsId",
                table: "SoldierType");

            migrationBuilder.DropColumn(
                name: "SettingsId",
                table: "Provinces");

            migrationBuilder.DropColumn(
                name: "Color_A",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Color_B",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Color_G",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Color_R",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Players");

            migrationBuilder.RenameColumn(
                name: "SettingsId",
                table: "Region",
                newName: "Color_R");

            migrationBuilder.RenameColumn(
                name: "SettingsId",
                table: "Players",
                newName: "IsHuman");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Region",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<byte>(
                name: "Color_A",
                table: "Region",
                type: "INTEGER",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "Color_B",
                table: "Region",
                type: "INTEGER",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "Color_G",
                table: "Region",
                type: "INTEGER",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<int>(
                name: "DefaultInstabilityInt",
                table: "Region",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "StrokeWidth",
                table: "Region",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<string>(
                name: "StringPassword",
                table: "Players",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color_A",
                table: "Region");

            migrationBuilder.DropColumn(
                name: "Color_B",
                table: "Region");

            migrationBuilder.DropColumn(
                name: "Color_G",
                table: "Region");

            migrationBuilder.DropColumn(
                name: "DefaultInstabilityInt",
                table: "Region");

            migrationBuilder.DropColumn(
                name: "StrokeWidth",
                table: "Region");

            migrationBuilder.RenameColumn(
                name: "Color_R",
                table: "Region",
                newName: "SettingsId");

            migrationBuilder.RenameColumn(
                name: "IsHuman",
                table: "Players",
                newName: "SettingsId");

            migrationBuilder.AddColumn<int>(
                name: "SettingsId",
                table: "SoldierType",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Region",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "SettingsId",
                table: "Provinces",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "StringPassword",
                table: "Players",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<byte>(
                name: "Color_A",
                table: "Players",
                type: "INTEGER",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "Color_B",
                table: "Players",
                type: "INTEGER",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "Color_G",
                table: "Players",
                type: "INTEGER",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "Color_R",
                table: "Players",
                type: "INTEGER",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Players",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CountdownSeconds = table.Column<int>(type: "INTEGER", nullable: false),
                    DebtLimit = table.Column<int>(type: "INTEGER", nullable: false),
                    DefaultMoney = table.Column<int>(type: "INTEGER", nullable: false),
                    FinalLandsCount = table.Column<int>(type: "INTEGER", nullable: false),
                    IntegerDefaultInstability = table.Column<int>(type: "INTEGER", nullable: false),
                    IntegerInterest = table.Column<int>(type: "INTEGER", nullable: false),
                    MountainsWidth = table.Column<float>(type: "REAL", nullable: false),
                    PlayerCount = table.Column<int>(type: "INTEGER", nullable: false),
                    LandColor_A = table.Column<byte>(type: "INTEGER", nullable: false),
                    LandColor_B = table.Column<byte>(type: "INTEGER", nullable: false),
                    LandColor_G = table.Column<byte>(type: "INTEGER", nullable: false),
                    LandColor_R = table.Column<byte>(type: "INTEGER", nullable: false),
                    MountainsColor_A = table.Column<byte>(type: "INTEGER", nullable: false),
                    MountainsColor_B = table.Column<byte>(type: "INTEGER", nullable: false),
                    MountainsColor_G = table.Column<byte>(type: "INTEGER", nullable: false),
                    MountainsColor_R = table.Column<byte>(type: "INTEGER", nullable: false),
                    SeaColor_A = table.Column<byte>(type: "INTEGER", nullable: false),
                    SeaColor_B = table.Column<byte>(type: "INTEGER", nullable: false),
                    SeaColor_G = table.Column<byte>(type: "INTEGER", nullable: false),
                    SeaColor_R = table.Column<byte>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SoldierType_SettingsId",
                table: "SoldierType",
                column: "SettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_Region_SettingsId",
                table: "Region",
                column: "SettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_Provinces_SettingsId",
                table: "Provinces",
                column: "SettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_SettingsId",
                table: "Players",
                column: "SettingsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Settings_SettingsId",
                table: "Players",
                column: "SettingsId",
                principalTable: "Settings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Provinces_Settings_SettingsId",
                table: "Provinces",
                column: "SettingsId",
                principalTable: "Settings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Region_Settings_SettingsId",
                table: "Region",
                column: "SettingsId",
                principalTable: "Settings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SoldierType_Settings_SettingsId",
                table: "SoldierType",
                column: "SettingsId",
                principalTable: "Settings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
