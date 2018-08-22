using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sarona.Migrations
{
    public partial class S970531_0944 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "NetworkElements",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "NetworkElements",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "NetworkElements",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "Links",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Links",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "Abbreviations",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Abbreviations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "NetworkElements");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "NetworkElements");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "NetworkElements");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Links");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Links");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Abbreviations");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Abbreviations");
        }
    }
}
