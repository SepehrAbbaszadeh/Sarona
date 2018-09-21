using Microsoft.EntityFrameworkCore.Migrations;
using System.IO;

namespace Sarona.Migrations
{
    public partial class Scripts970624_1216 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            using (var sr = new StreamReader(@"Scripts/CheckNumberingRange_Function.sql"))
            {
                migrationBuilder.Sql(sr.ReadToEnd());
            }
            using (var sr = new StreamReader(@"Scripts/AddCheckNumberingPools.sql"))
            {
                migrationBuilder.Sql(sr.ReadToEnd());
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE [dbo].[NumberingPools] DROP CONSTRAINT [CK_NumberingPools]");
            migrationBuilder.Sql("DROP FUNCTION [dbo].[CheckNumberingRange]");
        }
    }
}
