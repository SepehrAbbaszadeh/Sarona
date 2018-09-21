using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sarona.Migrations
{
    public partial class S970613_2130 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "NumberingPools",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "LinkHistories",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<int>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    Remark = table.Column<string>(nullable: true),
                    Channels = table.Column<int>(nullable: false),
                    LinkId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinkHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LinkHistories_Links_LinkId",
                        column: x => x.LinkId,
                        principalTable: "Links",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LinkHistories_LinkId",
                table: "LinkHistories",
                column: "LinkId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LinkHistories");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "NumberingPools");
        }
    }
}
