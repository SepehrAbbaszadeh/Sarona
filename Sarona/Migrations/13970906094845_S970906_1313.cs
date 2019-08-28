using Microsoft.EntityFrameworkCore.Migrations;

namespace Sarona.Migrations
{
    public partial class S970906_1313 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NumberingPoolNetworkElement_NetworkElements_NetworkElementId",
                table: "NumberingPoolNetworkElement");

            migrationBuilder.DropForeignKey(
                name: "FK_NumberingPoolNetworkElement_NumberingPools_NumberingId",
                table: "NumberingPoolNetworkElement");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NumberingPoolNetworkElement",
                table: "NumberingPoolNetworkElement");

            migrationBuilder.RenameTable(
                name: "NumberingPoolNetworkElement",
                newName: "NumberingPoolNetworkElements");

            migrationBuilder.RenameIndex(
                name: "IX_NumberingPoolNetworkElement_NumberingId",
                table: "NumberingPoolNetworkElements",
                newName: "IX_NumberingPoolNetworkElements_NumberingId");

            migrationBuilder.RenameIndex(
                name: "IX_NumberingPoolNetworkElement_NetworkElementId",
                table: "NumberingPoolNetworkElements",
                newName: "IX_NumberingPoolNetworkElements_NetworkElementId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NumberingPoolNetworkElements",
                table: "NumberingPoolNetworkElements",
                columns: new[] { "NumberingLocalId", "NetworkElementId" });

            migrationBuilder.AddForeignKey(
                name: "FK_NumberingPoolNetworkElements_NetworkElements_NetworkElementId",
                table: "NumberingPoolNetworkElements",
                column: "NetworkElementId",
                principalTable: "NetworkElements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NumberingPoolNetworkElements_NumberingPools_NumberingId",
                table: "NumberingPoolNetworkElements",
                column: "NumberingId",
                principalTable: "NumberingPools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NumberingPoolNetworkElements_NetworkElements_NetworkElementId",
                table: "NumberingPoolNetworkElements");

            migrationBuilder.DropForeignKey(
                name: "FK_NumberingPoolNetworkElements_NumberingPools_NumberingId",
                table: "NumberingPoolNetworkElements");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NumberingPoolNetworkElements",
                table: "NumberingPoolNetworkElements");

            migrationBuilder.RenameTable(
                name: "NumberingPoolNetworkElements",
                newName: "NumberingPoolNetworkElement");

            migrationBuilder.RenameIndex(
                name: "IX_NumberingPoolNetworkElements_NumberingId",
                table: "NumberingPoolNetworkElement",
                newName: "IX_NumberingPoolNetworkElement_NumberingId");

            migrationBuilder.RenameIndex(
                name: "IX_NumberingPoolNetworkElements_NetworkElementId",
                table: "NumberingPoolNetworkElement",
                newName: "IX_NumberingPoolNetworkElement_NetworkElementId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NumberingPoolNetworkElement",
                table: "NumberingPoolNetworkElement",
                columns: new[] { "NumberingLocalId", "NetworkElementId" });

            migrationBuilder.AddForeignKey(
                name: "FK_NumberingPoolNetworkElement_NetworkElements_NetworkElementId",
                table: "NumberingPoolNetworkElement",
                column: "NetworkElementId",
                principalTable: "NetworkElements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NumberingPoolNetworkElement_NumberingPools_NumberingId",
                table: "NumberingPoolNetworkElement",
                column: "NumberingId",
                principalTable: "NumberingPools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
