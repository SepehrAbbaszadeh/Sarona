using Microsoft.EntityFrameworkCore.Migrations;

namespace Sarona.Migrations
{
    public partial class s4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NeType",
                table: "NetworkElements");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "NetworkElements",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NetworkType",
                table: "NetworkElements",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NetworkType",
                table: "NetworkElements");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "NetworkElements",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "NeType",
                table: "NetworkElements",
                nullable: false,
                defaultValue: 0);
        }
    }
}
