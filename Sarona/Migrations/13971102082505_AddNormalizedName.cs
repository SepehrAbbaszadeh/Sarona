using Microsoft.EntityFrameworkCore.Migrations;
using System.IO;

namespace Sarona.Migrations
{
    public partial class AddNormalizedName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NormalizedSubscriberName",
                table: "NumberingPools",
                nullable: true);

            using (var sr = new StreamReader(@"Scripts/UpdateNormalizedNames.sql"))
            {
                migrationBuilder.Sql(sr.ReadToEnd());
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NormalizedSubscriberName",
                table: "NumberingPools");
        }
    }
}
