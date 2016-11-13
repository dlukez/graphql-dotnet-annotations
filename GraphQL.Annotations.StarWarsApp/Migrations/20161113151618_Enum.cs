using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GraphQL.Annotations.StarWarsApp.Migrations
{
    public partial class Enum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DroidAppearances",
                columns: table => new
                {
                    DroidAppearanceId = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    DroidId = table.Column<int>(nullable: false),
                    Episode = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DroidAppearances", x => x.DroidAppearanceId);
                    table.ForeignKey(
                        name: "FK_DroidAppearances_Droids_DroidId",
                        column: x => x.DroidId,
                        principalTable: "Droids",
                        principalColumn: "DroidId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HumanAppearances",
                columns: table => new
                {
                    HumanAppearanceId = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    Episode = table.Column<int>(nullable: false),
                    HumanId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HumanAppearances", x => x.HumanAppearanceId);
                    table.ForeignKey(
                        name: "FK_HumanAppearances_Humans_HumanId",
                        column: x => x.HumanId,
                        principalTable: "Humans",
                        principalColumn: "HumanId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DroidAppearances_DroidId",
                table: "DroidAppearances",
                column: "DroidId");

            migrationBuilder.CreateIndex(
                name: "IX_HumanAppearances_HumanId",
                table: "HumanAppearances",
                column: "HumanId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DroidAppearances");

            migrationBuilder.DropTable(
                name: "HumanAppearances");
        }
    }
}
