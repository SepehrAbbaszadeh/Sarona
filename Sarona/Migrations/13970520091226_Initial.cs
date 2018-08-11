using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sarona.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "LinkSequence",
                incrementBy: 2);

            migrationBuilder.CreateTable(
                name: "Abbreviations",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Abb = table.Column<string>(nullable: false),
                    Area = table.Column<int>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Abbreviations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NumberingPools",
                columns: table => new
                {
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    Dgsb = table.Column<int>(nullable: false),
                    ExpireDate = table.Column<DateTime>(nullable: false),
                    From = table.Column<long>(nullable: false),
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Max = table.Column<byte>(nullable: false),
                    Min = table.Column<byte>(nullable: false),
                    To = table.Column<long>(nullable: false),
                    NumberType = table.Column<string>(nullable: true),
                    ChargingCase = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    Remark = table.Column<string>(nullable: true),
                    RoutingType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NumberingPools", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NetworkElements",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Manufacturer = table.Column<string>(nullable: true),
                    Model = table.Column<string>(nullable: true),
                    AbbreviationId = table.Column<long>(nullable: false),
                    InstalledCapacity = table.Column<int>(nullable: false),
                    UsedCapacity = table.Column<int>(nullable: false),
                    ParentId = table.Column<long>(nullable: true),
                    Remark = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NetworkElements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NetworkElements_Abbreviations_AbbreviationId",
                        column: x => x.AbbreviationId,
                        principalTable: "Abbreviations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NetworkElements_NetworkElements_ParentId",
                        column: x => x.ParentId,
                        principalTable: "NetworkElements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Links",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<int>(nullable: false),
                    ChannelNo = table.Column<int>(nullable: false),
                    Direction = table.Column<int>(nullable: false),
                    End1Id = table.Column<long>(nullable: false),
                    End2Id = table.Column<long>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    Remark = table.Column<string>(nullable: true),
                    OtherLinkId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Links", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Links_NetworkElements_End1Id",
                        column: x => x.End1Id,
                        principalTable: "NetworkElements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Links_NetworkElements_End2Id",
                        column: x => x.End2Id,
                        principalTable: "NetworkElements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Links_Links_OtherLinkId",
                        column: x => x.OtherLinkId,
                        principalTable: "Links",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NumberingPoolNetworkElement",
                columns: table => new
                {
                    NumberingPoolId = table.Column<long>(nullable: false),
                    NetworkElementId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NumberingPoolNetworkElement", x => new { x.NumberingPoolId, x.NetworkElementId });
                    table.ForeignKey(
                        name: "FK_NumberingPoolNetworkElement_NetworkElements_NetworkElementId",
                        column: x => x.NetworkElementId,
                        principalTable: "NetworkElements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NumberingPoolNetworkElement_NumberingPools_NumberingPoolId",
                        column: x => x.NumberingPoolId,
                        principalTable: "NumberingPools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Links_End1Id",
                table: "Links",
                column: "End1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Links_End2Id",
                table: "Links",
                column: "End2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Links_OtherLinkId",
                table: "Links",
                column: "OtherLinkId");

            migrationBuilder.CreateIndex(
                name: "IX_NetworkElements_AbbreviationId",
                table: "NetworkElements",
                column: "AbbreviationId");

            migrationBuilder.CreateIndex(
                name: "IX_NetworkElements_ParentId",
                table: "NetworkElements",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_NumberingPoolNetworkElement_NetworkElementId",
                table: "NumberingPoolNetworkElement",
                column: "NetworkElementId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Links");

            migrationBuilder.DropTable(
                name: "NumberingPoolNetworkElement");

            migrationBuilder.DropTable(
                name: "NetworkElements");

            migrationBuilder.DropTable(
                name: "NumberingPools");

            migrationBuilder.DropTable(
                name: "Abbreviations");

            migrationBuilder.DropSequence(
                name: "LinkSequence");
        }
    }
}
