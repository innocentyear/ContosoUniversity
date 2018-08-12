using Microsoft.EntityFrameworkCore.Migrations;

namespace ContosoUniversity.Migrations
{
    public partial class InheritanceLater : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "OldID", table: "Person");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
