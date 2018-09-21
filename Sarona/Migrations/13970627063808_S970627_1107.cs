using Microsoft.EntityFrameworkCore.Migrations;

namespace Sarona.Migrations
{
    public partial class S970627_1107 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE [dbo].[NumberingPools] DROP CONSTRAINT [CK_NumberingPools]");

            migrationBuilder.Sql("DROP FUNCTION [dbo].[CheckNumberingRange]");

            migrationBuilder.DropColumn(
                name: "From",
                table: "NumberingPools");

            migrationBuilder.DropColumn(
                name: "To",
                table: "NumberingPools");

            migrationBuilder.AddColumn<string>(
                name: "Prefix",
                table: "NumberingPools",
                maxLength: 2,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Prefix",
                table: "NumberingPools");

            migrationBuilder.AddColumn<long>(
                name: "From",
                table: "NumberingPools",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "To",
                table: "NumberingPools",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
