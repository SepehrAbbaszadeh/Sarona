using Microsoft.EntityFrameworkCore.Migrations;
using System.IO;

namespace Sarona.Migrations
{
    public partial class S970909_2303 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            using (var sr = new StreamReader(@"Scripts/GetRondType_Function.sql"))
            {
                migrationBuilder.Sql(sr.ReadToEnd());
            }
            migrationBuilder.AlterColumn<byte>(
                name: "Rond",
                table: "NumberingPools",
                nullable: false,
                computedColumnSql: "([dbo].[GetRondType]([Prefix]))",
                oldClrType: typeof(byte));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "Rond",
                table: "NumberingPools",
                nullable: false,
                oldClrType: typeof(byte),
                oldComputedColumnSql: "([dbo].[GetRondType]([Prefix]))");
        }
    }
}
