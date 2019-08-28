using Microsoft.EntityFrameworkCore.Migrations;

namespace Sarona.Migrations
{
    public partial class S970910_1358 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_NumberingPoolNetworkElements",
                table: "NumberingPoolNetworkElements");

            migrationBuilder.DropIndex(
                name: "IX_NumberingPoolNetworkElements_NumberingId",
                table: "NumberingPoolNetworkElements");

            migrationBuilder.DropColumn(
                name: "NumberingLocalId",
                table: "NumberingPoolNetworkElements");

            migrationBuilder.AlterColumn<long>(
                name: "NumberingId",
                table: "NumberingPoolNetworkElements",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_NumberingPoolNetworkElements",
                table: "NumberingPoolNetworkElements",
                columns: new[] { "NumberingId", "NetworkElementId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_NumberingPoolNetworkElements",
                table: "NumberingPoolNetworkElements");

            migrationBuilder.AlterColumn<long>(
                name: "NumberingId",
                table: "NumberingPoolNetworkElements",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<long>(
                name: "NumberingLocalId",
                table: "NumberingPoolNetworkElements",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(
                name: "PK_NumberingPoolNetworkElements",
                table: "NumberingPoolNetworkElements",
                columns: new[] { "NumberingLocalId", "NetworkElementId" });

            migrationBuilder.CreateIndex(
                name: "IX_NumberingPoolNetworkElements_NumberingId",
                table: "NumberingPoolNetworkElements",
                column: "NumberingId");
        }
    }
}
