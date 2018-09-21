using Microsoft.EntityFrameworkCore.Migrations;

namespace Sarona.Migrations
{
    public partial class S970629_1127 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "NumberingPools",
                newName: "Username");

            migrationBuilder.AddColumn<string>(
                name: "Owner",
                table: "NumberingPools",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Owner",
                table: "NumberingPools");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "NumberingPools",
                newName: "UserName");
        }
    }
}
