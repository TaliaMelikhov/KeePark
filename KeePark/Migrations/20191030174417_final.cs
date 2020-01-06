using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KeePark.Migrations
{
    public partial class final : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ParkingSpot",
                columns: table => new
                {
                    ParkingSpotID = table.Column<int>(nullable: false),
                    SpotName = table.Column<string>(nullable: false),
                    OwnerID = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: false),
                    Price = table.Column<int>(nullable: false),
                    NunOfOrders = table.Column<int>(nullable: false),
                    filePath = table.Column<string>(nullable: true),
                    SpotDescription = table.Column<string>(nullable: true),
                    SiteType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkingSpot", x => x.ParkingSpotID);
                });

            migrationBuilder.CreateTable(
                name: "ReserveSpot",
                columns: table => new
                {
                    ReserveSpotID = table.Column<Guid>(nullable: false),
                    UserID = table.Column<string>(nullable: true),
                    SpotID = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ReservationDate = table.Column<DateTime>(nullable: false),
                    ReservationHour = table.Column<int>(nullable: false),
                    Duration = table.Column<int>(nullable: false),
                    carNumber = table.Column<string>(maxLength: 8, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReserveSpot", x => x.ReserveSpotID);
                    table.ForeignKey(
                        name: "FK_ReserveSpot_ParkingSpot_SpotID",
                        column: x => x.SpotID,
                        principalTable: "ParkingSpot",
                        principalColumn: "ParkingSpotID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReserveSpot_SpotID",
                table: "ReserveSpot",
                column: "SpotID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReserveSpot");

            migrationBuilder.DropTable(
                name: "ParkingSpot");
        }
    }
}
