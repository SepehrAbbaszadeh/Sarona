using Microsoft.EntityFrameworkCore.Migrations;

namespace Sarona.Migrations
{
    public partial class S970629_1145 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Prefix",
                table: "NumberingPools",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 2);

            migrationBuilder.CreateIndex(
                name: "IX_NumberingPools_Prefix",
                table: "NumberingPools",
                column: "Prefix",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_NumberingPools_Prefix",
                table: "NumberingPools");

            migrationBuilder.AlterColumn<string>(
                name: "Prefix",
                table: "NumberingPools",
                maxLength: 2,
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
