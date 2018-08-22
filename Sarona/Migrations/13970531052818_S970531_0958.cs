using Microsoft.EntityFrameworkCore.Migrations;
using System.IO;

namespace Sarona.Migrations
{
    public partial class S970531_0958 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            using (var sr = new StreamReader(@"Scripts/CheckNetworkElement_Function.sql"))
            {
                migrationBuilder.Sql(sr.ReadToEnd());
            }
            using (var sr = new StreamReader(@"Scripts/AddCheckNetworkElement.sql"))
            {
                migrationBuilder.Sql(sr.ReadToEnd());
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE [dbo].[NetworkElements] DROP CONSTRAINT [CK_NetworkElements]");
            migrationBuilder.Sql("DROP FUNCTION [dbo].[CheckNetworkElement]");
        }
    }
}
