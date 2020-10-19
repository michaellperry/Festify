using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Festify.Promotion.Migrations
{
    public partial class AddShowRemoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShowRemoved",
                columns: table => new
                {
                    ShowRemovedId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShowId = table.Column<int>(nullable: false),
                    RemovedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShowRemoved", x => x.ShowRemovedId);
                    table.UniqueConstraint("AK_ShowRemoved_ShowId_RemovedDate", x => new { x.ShowId, x.RemovedDate });
                    table.ForeignKey(
                        name: "FK_ShowRemoved_Show_ShowId",
                        column: x => x.ShowId,
                        principalTable: "Show",
                        principalColumn: "ShowId",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShowRemoved");
        }
    }
}
