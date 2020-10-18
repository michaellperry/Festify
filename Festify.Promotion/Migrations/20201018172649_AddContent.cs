using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Festify.Promotion.Migrations
{
    public partial class AddContent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Content",
                columns: table => new
                {
                    Hash = table.Column<string>(maxLength: 88, nullable: false),
                    Binary = table.Column<byte[]>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Content", x => x.Hash);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Content");
        }
    }
}
