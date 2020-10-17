using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Festify.Promotion.Migrations
{
    public partial class AddShowDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShowDescription",
                columns: table => new
                {
                    ShowDescriptionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShowId = table.Column<int>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(maxLength: 100, nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    City = table.Column<string>(maxLength: 50, nullable: true),
                    Venue = table.Column<string>(maxLength: 50, nullable: true),
                    ImageHash = table.Column<string>(maxLength: 88, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShowDescription", x => x.ShowDescriptionId);
                    table.UniqueConstraint("AK_ShowDescription_ShowId_ModifiedDate", x => new { x.ShowId, x.ModifiedDate });
                    table.ForeignKey(
                        name: "FK_ShowDescription_Show_ShowId",
                        column: x => x.ShowId,
                        principalTable: "Show",
                        principalColumn: "ShowId",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShowDescription");
        }
    }
}
