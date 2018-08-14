using Microsoft.EntityFrameworkCore.Migrations;

namespace Sarona.Migrations
{
    public partial class s3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NeType",
                table: "NetworkElements",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NeType",
                table: "NetworkElements");
        }
    }
}
