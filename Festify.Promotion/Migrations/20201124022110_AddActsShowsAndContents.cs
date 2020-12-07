using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Festify.Promotion.Migrations
{
    public partial class AddActsShowsAndContents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Act",
                columns: table => new
                {
                    ActId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Act", x => x.ActId);
                    table.UniqueConstraint("AK_Act_ActGuid", x => x.ActGuid);
                });

            migrationBuilder.CreateTable(
                name: "Content",
                columns: table => new
                {
                    Hash = table.Column<string>(type: "nvarchar(88)", maxLength: 88, nullable: false),
                    Binary = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Content", x => x.Hash);
                });

            migrationBuilder.CreateTable(
                name: "ActDescription",
                columns: table => new
                {
                    ActDescriptionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActId = table.Column<int>(type: "int", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ImageHash = table.Column<string>(type: "nvarchar(88)", maxLength: 88, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActDescription", x => x.ActDescriptionId);
                    table.UniqueConstraint("AK_ActDescription_ActId_ModifiedDate", x => new { x.ActId, x.ModifiedDate });
                    table.ForeignKey(
                        name: "FK_ActDescription_Act_ActId",
                        column: x => x.ActId,
                        principalTable: "Act",
                        principalColumn: "ActId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActRemoved",
                columns: table => new
                {
                    ActRemovedId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActId = table.Column<int>(type: "int", nullable: false),
                    RemovedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActRemoved", x => x.ActRemovedId);
                    table.UniqueConstraint("AK_ActRemoved_ActId_RemovedDate", x => new { x.ActId, x.RemovedDate });
                    table.ForeignKey(
                        name: "FK_ActRemoved_Act_ActId",
                        column: x => x.ActId,
                        principalTable: "Act",
                        principalColumn: "ActId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Show",
                columns: table => new
                {
                    ShowId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActId = table.Column<int>(type: "int", nullable: false),
                    VenueId = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Show", x => x.ShowId);
                    table.UniqueConstraint("AK_Show_ActId_VenueId_StartTime", x => new { x.ActId, x.VenueId, x.StartTime });
                    table.ForeignKey(
                        name: "FK_Show_Act_ActId",
                        column: x => x.ActId,
                        principalTable: "Act",
                        principalColumn: "ActId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Show_Venue_VenueId",
                        column: x => x.VenueId,
                        principalTable: "Venue",
                        principalColumn: "VenueId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShowCancelled",
                columns: table => new
                {
                    ShowCancelledId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShowId = table.Column<int>(type: "int", nullable: false),
                    CancelledDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShowCancelled", x => x.ShowCancelledId);
                    table.UniqueConstraint("AK_ShowCancelled_ShowId_CancelledDate", x => new { x.ShowId, x.CancelledDate });
                    table.ForeignKey(
                        name: "FK_ShowCancelled_Show_ShowId",
                        column: x => x.ShowId,
                        principalTable: "Show",
                        principalColumn: "ShowId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Show_VenueId",
                table: "Show",
                column: "VenueId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActDescription");

            migrationBuilder.DropTable(
                name: "ActRemoved");

            migrationBuilder.DropTable(
                name: "Content");

            migrationBuilder.DropTable(
                name: "ShowCancelled");

            migrationBuilder.DropTable(
                name: "Show");

            migrationBuilder.DropTable(
                name: "Act");
        }
    }
}
