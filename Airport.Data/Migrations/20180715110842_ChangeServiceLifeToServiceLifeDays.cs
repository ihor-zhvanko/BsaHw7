using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Airport.Data.Migrations
{
    public partial class ChangeServiceLifeToServiceLifeDays : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServiceLife",
                table: "Plane");

            migrationBuilder.AddColumn<long>(
                name: "ServiceLifeDays",
                table: "Plane",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServiceLifeDays",
                table: "Plane");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ServiceLife",
                table: "Plane",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }
    }
}
