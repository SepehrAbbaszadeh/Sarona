using Microsoft.EntityFrameworkCore.Migrations;

namespace Sarona.Migrations
{
    public partial class S970525_1813 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Owner",
                table: "NetworkElements",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Owner",
                table: "NetworkElements");
        }
    }
}
