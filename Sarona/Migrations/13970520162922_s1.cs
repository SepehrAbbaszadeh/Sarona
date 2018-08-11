using Microsoft.EntityFrameworkCore.Migrations;

namespace Sarona.Migrations
{
    public partial class s1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Abb",
                table: "Abbreviations",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.CreateIndex(
                name: "IX_Abbreviations_Abb",
                table: "Abbreviations",
                column: "Abb",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Abbreviations_Abb",
                table: "Abbreviations");

            migrationBuilder.AlterColumn<string>(
                name: "Abb",
                table: "Abbreviations",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
