using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImperitWASM.Server.Migrations
{
    public partial class UnifiedTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Current = table.Column<int>(type: "INTEGER", nullable: false),
                    StartTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FinishTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Powers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GameId = table.Column<int>(type: "INTEGER", nullable: false),
                    Order = table.Column<int>(type: "INTEGER", nullable: false),
                    Alive = table.Column<bool>(type: "INTEGER", nullable: false),
                    Final = table.Column<int>(type: "INTEGER", nullable: false),
                    Income = table.Column<int>(type: "INTEGER", nullable: false),
                    Money = table.Column<int>(type: "INTEGER", nullable: false),
                    Soldiers = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Powers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    CountdownSeconds = table.Column<int>(type: "INTEGER", nullable: false),
                    IntegerDefaultInstability = table.Column<int>(type: "INTEGER", nullable: false),
                    DebtLimit = table.Column<int>(type: "INTEGER", nullable: false),
                    FinalLandsCount = table.Column<int>(type: "INTEGER", nullable: false),
                    IntegerInterest = table.Column<int>(type: "INTEGER", nullable: false),
                    LandColor_R = table.Column<byte>(type: "INTEGER", nullable: false),
                    LandColor_G = table.Column<byte>(type: "INTEGER", nullable: false),
                    LandColor_B = table.Column<byte>(type: "INTEGER", nullable: false),
                    LandColor_A = table.Column<byte>(type: "INTEGER", nullable: false),
                    MountainsColor_R = table.Column<byte>(type: "INTEGER", nullable: false),
                    MountainsColor_G = table.Column<byte>(type: "INTEGER", nullable: false),
                    MountainsColor_B = table.Column<byte>(type: "INTEGER", nullable: false),
                    MountainsColor_A = table.Column<byte>(type: "INTEGER", nullable: false),
                    MountainsWidth = table.Column<float>(type: "REAL", nullable: false),
                    SeaColor_R = table.Column<byte>(type: "INTEGER", nullable: false),
                    SeaColor_G = table.Column<byte>(type: "INTEGER", nullable: false),
                    SeaColor_B = table.Column<byte>(type: "INTEGER", nullable: false),
                    SeaColor_A = table.Column<byte>(type: "INTEGER", nullable: false),
                    DefaultMoney = table.Column<int>(type: "INTEGER", nullable: false),
                    PlayerCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.CountdownSeconds);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    GameId = table.Column<int>(type: "INTEGER", nullable: false),
                    Order = table.Column<int>(type: "INTEGER", nullable: false),
                    Color_R = table.Column<byte>(type: "INTEGER", nullable: false),
                    Color_G = table.Column<byte>(type: "INTEGER", nullable: false),
                    Color_B = table.Column<byte>(type: "INTEGER", nullable: false),
                    Color_A = table.Column<byte>(type: "INTEGER", nullable: false),
                    Money = table.Column<int>(type: "INTEGER", nullable: false),
                    Alive = table.Column<bool>(type: "INTEGER", nullable: false),
                    SettingsCountdownSeconds = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    Discriminator = table.Column<string>(type: "TEXT", nullable: false),
                    StringPassword = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Name);
                    table.ForeignKey(
                        name: "FK_Players_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Players_Settings_SettingsCountdownSeconds",
                        column: x => x.SettingsCountdownSeconds,
                        principalTable: "Settings",
                        principalColumn: "CountdownSeconds",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    Key = table.Column<string>(type: "TEXT", nullable: false),
                    P = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.Key);
                    table.ForeignKey(
                        name: "FK_Sessions_Players_P",
                        column: x => x.P,
                        principalTable: "Players",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Regiment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TypeSymbol = table.Column<string>(type: "TEXT", nullable: false),
                    Count = table.Column<int>(type: "INTEGER", nullable: false),
                    ManoeuvreId = table.Column<int>(type: "INTEGER", nullable: true),
                    ProvinceId = table.Column<int>(type: "INTEGER", nullable: true),
                    SoldiersId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regiment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Provinces",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GameId = table.Column<int>(type: "INTEGER", nullable: false),
                    RegionId = table.Column<int>(type: "INTEGER", nullable: false),
                    PlayerName = table.Column<string>(type: "TEXT", nullable: true),
                    SettingsCountdownSeconds = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provinces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Provinces_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Provinces_Players_PlayerName",
                        column: x => x.PlayerName,
                        principalTable: "Players",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Provinces_Settings_SettingsCountdownSeconds",
                        column: x => x.SettingsCountdownSeconds,
                        principalTable: "Settings",
                        principalColumn: "CountdownSeconds",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Action",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Discriminator = table.Column<string>(type: "TEXT", nullable: false),
                    PlayerName = table.Column<string>(type: "TEXT", nullable: false),
                    PlayerName1 = table.Column<string>(type: "TEXT", nullable: true),
                    Debt = table.Column<int>(type: "INTEGER", nullable: true),
                    ProvinceId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Action", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Action_Players_PlayerName",
                        column: x => x.PlayerName,
                        principalTable: "Players",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Action_Players_PlayerName1",
                        column: x => x.PlayerName1,
                        principalTable: "Players",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Action_Provinces_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Provinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Shape",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CenterId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shape", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Region",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ShapeId = table.Column<int>(type: "INTEGER", nullable: true),
                    SettingsCountdownSeconds1 = table.Column<int>(type: "INTEGER", nullable: false),
                    Discriminator = table.Column<string>(type: "TEXT", nullable: false),
                    SettingsCountdownSeconds = table.Column<int>(type: "INTEGER", nullable: false),
                    SettingsCountdownSeconds2 = table.Column<int>(type: "INTEGER", nullable: true),
                    Earnings = table.Column<int>(type: "INTEGER", nullable: true),
                    IsStart = table.Column<bool>(type: "INTEGER", nullable: true),
                    IsFinal = table.Column<bool>(type: "INTEGER", nullable: true),
                    HasPort = table.Column<bool>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Region", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Region_Settings_SettingsCountdownSeconds",
                        column: x => x.SettingsCountdownSeconds,
                        principalTable: "Settings",
                        principalColumn: "CountdownSeconds",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Region_Settings_SettingsCountdownSeconds1",
                        column: x => x.SettingsCountdownSeconds1,
                        principalTable: "Settings",
                        principalColumn: "CountdownSeconds",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Region_Settings_SettingsCountdownSeconds2",
                        column: x => x.SettingsCountdownSeconds2,
                        principalTable: "Settings",
                        principalColumn: "CountdownSeconds",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Region_Shape_ShapeId",
                        column: x => x.ShapeId,
                        principalTable: "Shape",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Point",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    X = table.Column<double>(type: "REAL", nullable: false),
                    Y = table.Column<double>(type: "REAL", nullable: false),
                    ProvinceId = table.Column<int>(type: "INTEGER", nullable: true),
                    RegionId = table.Column<int>(type: "INTEGER", nullable: true),
                    ShapeId = table.Column<int>(type: "INTEGER", nullable: true),
                    ShapeId1 = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Point", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Point_Provinces_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Provinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Point_Region_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Region",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Point_Shape_ShapeId",
                        column: x => x.ShapeId,
                        principalTable: "Shape",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Point_Shape_ShapeId1",
                        column: x => x.ShapeId1,
                        principalTable: "Shape",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Soldiers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    RegionId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Soldiers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Soldiers_Provinces_Id",
                        column: x => x.Id,
                        principalTable: "Provinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Soldiers_Region_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Region",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SoldierType",
                columns: table => new
                {
                    Symbol = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Text = table.Column<string>(type: "TEXT", nullable: false),
                    AttackPower = table.Column<int>(type: "INTEGER", nullable: false),
                    DefensePower = table.Column<int>(type: "INTEGER", nullable: false),
                    Weight = table.Column<int>(type: "INTEGER", nullable: false),
                    Price = table.Column<int>(type: "INTEGER", nullable: false),
                    Discriminator = table.Column<string>(type: "TEXT", nullable: false),
                    SettingsCountdownSeconds = table.Column<int>(type: "INTEGER", nullable: true),
                    SettingsCountdownSeconds1 = table.Column<int>(type: "INTEGER", nullable: false),
                    SoldiersId = table.Column<int>(type: "INTEGER", nullable: true),
                    Elephant_Capacity = table.Column<int>(type: "INTEGER", nullable: true),
                    Elephant_Speed = table.Column<int>(type: "INTEGER", nullable: true),
                    Capacity = table.Column<int>(type: "INTEGER", nullable: true),
                    Speed = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoldierType", x => x.Symbol);
                    table.ForeignKey(
                        name: "FK_SoldierType_Settings_SettingsCountdownSeconds",
                        column: x => x.SettingsCountdownSeconds,
                        principalTable: "Settings",
                        principalColumn: "CountdownSeconds",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SoldierType_Settings_SettingsCountdownSeconds1",
                        column: x => x.SettingsCountdownSeconds1,
                        principalTable: "Settings",
                        principalColumn: "CountdownSeconds",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SoldierType_Soldiers_SoldiersId",
                        column: x => x.SoldiersId,
                        principalTable: "Soldiers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProvinceSoldierType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SoldierTypeSymbol = table.Column<string>(type: "TEXT", nullable: false),
                    RegionId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProvinceSoldierType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProvinceSoldierType_Region_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Region",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProvinceSoldierType_SoldierType_SoldierTypeSymbol",
                        column: x => x.SoldierTypeSymbol,
                        principalTable: "SoldierType",
                        principalColumn: "Symbol",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Action_PlayerName",
                table: "Action",
                column: "PlayerName");

            migrationBuilder.CreateIndex(
                name: "IX_Action_PlayerName1",
                table: "Action",
                column: "PlayerName1");

            migrationBuilder.CreateIndex(
                name: "IX_Action_ProvinceId",
                table: "Action",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_GameId",
                table: "Players",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_SettingsCountdownSeconds",
                table: "Players",
                column: "SettingsCountdownSeconds");

            migrationBuilder.CreateIndex(
                name: "IX_Point_ProvinceId",
                table: "Point",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_Point_RegionId",
                table: "Point",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Point_ShapeId",
                table: "Point",
                column: "ShapeId");

            migrationBuilder.CreateIndex(
                name: "IX_Point_ShapeId1",
                table: "Point",
                column: "ShapeId1");

            migrationBuilder.CreateIndex(
                name: "IX_Provinces_GameId",
                table: "Provinces",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Provinces_PlayerName",
                table: "Provinces",
                column: "PlayerName");

            migrationBuilder.CreateIndex(
                name: "IX_Provinces_RegionId",
                table: "Provinces",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Provinces_SettingsCountdownSeconds",
                table: "Provinces",
                column: "SettingsCountdownSeconds");

            migrationBuilder.CreateIndex(
                name: "IX_ProvinceSoldierType_RegionId",
                table: "ProvinceSoldierType",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProvinceSoldierType_SoldierTypeSymbol",
                table: "ProvinceSoldierType",
                column: "SoldierTypeSymbol");

            migrationBuilder.CreateIndex(
                name: "IX_Regiment_ManoeuvreId",
                table: "Regiment",
                column: "ManoeuvreId");

            migrationBuilder.CreateIndex(
                name: "IX_Regiment_ProvinceId",
                table: "Regiment",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_Regiment_SoldiersId",
                table: "Regiment",
                column: "SoldiersId");

            migrationBuilder.CreateIndex(
                name: "IX_Regiment_TypeSymbol",
                table: "Regiment",
                column: "TypeSymbol");

            migrationBuilder.CreateIndex(
                name: "IX_Region_SettingsCountdownSeconds",
                table: "Region",
                column: "SettingsCountdownSeconds");

            migrationBuilder.CreateIndex(
                name: "IX_Region_SettingsCountdownSeconds1",
                table: "Region",
                column: "SettingsCountdownSeconds1");

            migrationBuilder.CreateIndex(
                name: "IX_Region_SettingsCountdownSeconds2",
                table: "Region",
                column: "SettingsCountdownSeconds2");

            migrationBuilder.CreateIndex(
                name: "IX_Region_ShapeId",
                table: "Region",
                column: "ShapeId");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_P",
                table: "Sessions",
                column: "P");

            migrationBuilder.CreateIndex(
                name: "IX_Shape_CenterId",
                table: "Shape",
                column: "CenterId");

            migrationBuilder.CreateIndex(
                name: "IX_Soldiers_RegionId",
                table: "Soldiers",
                column: "RegionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SoldierType_SettingsCountdownSeconds",
                table: "SoldierType",
                column: "SettingsCountdownSeconds");

            migrationBuilder.CreateIndex(
                name: "IX_SoldierType_SettingsCountdownSeconds1",
                table: "SoldierType",
                column: "SettingsCountdownSeconds1");

            migrationBuilder.CreateIndex(
                name: "IX_SoldierType_SoldiersId",
                table: "SoldierType",
                column: "SoldiersId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Regiment_Soldiers_SoldiersId",
                table: "Regiment",
                column: "SoldiersId",
                principalTable: "Soldiers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Regiment_SoldierType_TypeSymbol",
                table: "Regiment",
                column: "TypeSymbol",
                principalTable: "SoldierType",
                principalColumn: "Symbol",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Provinces_Region_RegionId",
                table: "Provinces",
                column: "RegionId",
                principalTable: "Region",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Shape_Point_CenterId",
                table: "Shape",
                column: "CenterId",
                principalTable: "Point",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Provinces_Players_PlayerName",
                table: "Provinces");

            migrationBuilder.DropForeignKey(
                name: "FK_Point_Provinces_ProvinceId",
                table: "Point");

            migrationBuilder.DropForeignKey(
                name: "FK_Region_Settings_SettingsCountdownSeconds",
                table: "Region");

            migrationBuilder.DropForeignKey(
                name: "FK_Region_Settings_SettingsCountdownSeconds1",
                table: "Region");

            migrationBuilder.DropForeignKey(
                name: "FK_Region_Settings_SettingsCountdownSeconds2",
                table: "Region");

            migrationBuilder.DropForeignKey(
                name: "FK_Point_Region_RegionId",
                table: "Point");

            migrationBuilder.DropForeignKey(
                name: "FK_Point_Shape_ShapeId",
                table: "Point");

            migrationBuilder.DropForeignKey(
                name: "FK_Point_Shape_ShapeId1",
                table: "Point");

            migrationBuilder.DropTable(
                name: "Powers");

            migrationBuilder.DropTable(
                name: "ProvinceSoldierType");

            migrationBuilder.DropTable(
                name: "Regiment");

            migrationBuilder.DropTable(
                name: "Sessions");

            migrationBuilder.DropTable(
                name: "Action");

            migrationBuilder.DropTable(
                name: "SoldierType");

            migrationBuilder.DropTable(
                name: "Soldiers");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Provinces");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "Region");

            migrationBuilder.DropTable(
                name: "Shape");

            migrationBuilder.DropTable(
                name: "Point");
        }
    }
}
