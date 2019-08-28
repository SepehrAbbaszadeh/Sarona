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
                name: "AccessSequence");

            migrationBuilder.CreateSequence(
                name: "PbxSequence");

            migrationBuilder.CreateSequence(
                name: "RemoteSequence");

            migrationBuilder.CreateTable(
                name: "Abbreviations",
                columns: table => new
                {
                    Abb = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Abbreviations", x => x.Abb);
                });

            migrationBuilder.CreateTable(
                name: "Miscs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Miscs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Exchanges",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    Abb = table.Column<string>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    Providence = table.Column<string>(nullable: true),
                    Area = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exchanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Exchanges_Abbreviations_Abb",
                        column: x => x.Abb,
                        principalTable: "Abbreviations",
                        principalColumn: "Abb",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NumberingPools",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Prefix = table.Column<string>(nullable: false),
                    Min = table.Column<byte>(nullable: false),
                    Max = table.Column<byte>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    Remark = table.Column<string>(nullable: true),
                    ChargingCase = table.Column<string>(nullable: false),
                    Owner = table.Column<string>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    IsFloat = table.Column<bool>(nullable: false),
                    SecondaryMin = table.Column<byte>(nullable: false),
                    SecondaryMax = table.Column<byte>(nullable: false),
                    Abb = table.Column<string>(nullable: true),
                    SubscriberName = table.Column<string>(nullable: true),
                    IsKeshvari = table.Column<bool>(nullable: false),
                    ExpireDate = table.Column<DateTime>(nullable: false),
                    Link = table.Column<int>(nullable: true),
                    Area = table.Column<int>(nullable: true),
                    SecondaryArea = table.Column<int>(nullable: true),
                    Rond = table.Column<byte>(nullable: false),
                    NumberType = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NumberingPools", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NumberingPools_Abbreviations_Abb",
                        column: x => x.Abb,
                        principalTable: "Abbreviations",
                        principalColumn: "Abb",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NetworkElements",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    NetworkType = table.Column<int>(nullable: false),
                    Type = table.Column<string>(nullable: false),
                    Manufacturer = table.Column<string>(nullable: false),
                    Model = table.Column<string>(nullable: false),
                    ExchangeId = table.Column<long>(nullable: false),
                    InstalledCapacity = table.Column<int>(nullable: false),
                    UsedCapacity = table.Column<int>(nullable: false),
                    Owner = table.Column<string>(nullable: false),
                    ParentId = table.Column<long>(nullable: true),
                    Remark = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NetworkElements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NetworkElements_Exchanges_ExchangeId",
                        column: x => x.ExchangeId,
                        principalTable: "Exchanges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    Channels = table.Column<int>(nullable: false),
                    Direction = table.Column<int>(nullable: false),
                    End1Id = table.Column<long>(nullable: false),
                    End2Id = table.Column<long>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    Remark = table.Column<string>(nullable: true),
                    OtherLinkId = table.Column<long>(nullable: true)
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
                    NumberingLocalId = table.Column<long>(nullable: false),
                    NumberingId = table.Column<long>(nullable: true),
                    NetworkElementId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NumberingPoolNetworkElement", x => new { x.NumberingLocalId, x.NetworkElementId });
                    table.ForeignKey(
                        name: "FK_NumberingPoolNetworkElement_NetworkElements_NetworkElementId",
                        column: x => x.NetworkElementId,
                        principalTable: "NetworkElements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NumberingPoolNetworkElement_NumberingPools_NumberingId",
                        column: x => x.NumberingId,
                        principalTable: "NumberingPools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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
                    LinkId = table.Column<long>(nullable: false),
                    Direction = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinkHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LinkHistories_Links_LinkId",
                        column: x => x.LinkId,
                        principalTable: "Links",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Exchanges_Abb",
                table: "Exchanges",
                column: "Abb",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LinkHistories_LinkId",
                table: "LinkHistories",
                column: "LinkId");

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
                name: "IX_NetworkElements_ExchangeId",
                table: "NetworkElements",
                column: "ExchangeId");

            migrationBuilder.CreateIndex(
                name: "IX_NetworkElements_Name",
                table: "NetworkElements",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_NetworkElements_ParentId",
                table: "NetworkElements",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_NumberingPoolNetworkElement_NetworkElementId",
                table: "NumberingPoolNetworkElement",
                column: "NetworkElementId");

            migrationBuilder.CreateIndex(
                name: "IX_NumberingPoolNetworkElement_NumberingId",
                table: "NumberingPoolNetworkElement",
                column: "NumberingId");

            migrationBuilder.CreateIndex(
                name: "IX_NumberingPools_Abb",
                table: "NumberingPools",
                column: "Abb");

            migrationBuilder.CreateIndex(
                name: "IX_NumberingPools_Prefix",
                table: "NumberingPools",
                column: "Prefix",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LinkHistories");

            migrationBuilder.DropTable(
                name: "Miscs");

            migrationBuilder.DropTable(
                name: "NumberingPoolNetworkElement");

            migrationBuilder.DropTable(
                name: "Links");

            migrationBuilder.DropTable(
                name: "NumberingPools");

            migrationBuilder.DropTable(
                name: "NetworkElements");

            migrationBuilder.DropTable(
                name: "Exchanges");

            migrationBuilder.DropTable(
                name: "Abbreviations");

            migrationBuilder.DropSequence(
                name: "AccessSequence");

            migrationBuilder.DropSequence(
                name: "PbxSequence");

            migrationBuilder.DropSequence(
                name: "RemoteSequence");
        }
    }
}
