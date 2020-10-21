using Microsoft.EntityFrameworkCore.Migrations;

namespace Festify.Promotion.Migrations
{
    public partial class AddShowDescriptionIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE NONCLUSTERED INDEX IX_ShowDescription_ShowId_ModifiedDate ON ShowDescription
            (
                ShowId ASC,
                ModifiedDate DESC
            )
            INCLUDE(Title,Date,City,Venue,ImageHash)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP INDEX IX_ShowDescription_ShowId_ModifiedDate ON ShowDescription");
        }
    }
}
