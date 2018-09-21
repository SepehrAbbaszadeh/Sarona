using Microsoft.EntityFrameworkCore.Migrations;

namespace Sarona.Migrations
{
    public partial class S970613_2149 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LinkHistories_Links_LinkId",
                table: "LinkHistories");

            migrationBuilder.AddForeignKey(
                name: "FK_LinkHistories_Links_LinkId",
                table: "LinkHistories",
                column: "LinkId",
                principalTable: "Links",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LinkHistories_Links_LinkId",
                table: "LinkHistories");

            migrationBuilder.AddForeignKey(
                name: "FK_LinkHistories_Links_LinkId",
                table: "LinkHistories",
                column: "LinkId",
                principalTable: "Links",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
