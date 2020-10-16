using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Festify.Promotion.Migrations
{
    public partial class AddShow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Show",
                columns: table => new
                {
                    ShowId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShowGuid = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Show", x => x.ShowId);
                    table.UniqueConstraint("AK_Show_ShowGuid", x => x.ShowGuid);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Show");
        }
    }
}
