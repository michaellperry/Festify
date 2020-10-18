using Microsoft.EntityFrameworkCore.Migrations;

namespace Festify.Promotion.Migrations
{
    public partial class AddContentType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "Content",
                maxLength: 20,
                nullable: false,
                defaultValue: "image/jpg");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "Content");
        }
    }
}
