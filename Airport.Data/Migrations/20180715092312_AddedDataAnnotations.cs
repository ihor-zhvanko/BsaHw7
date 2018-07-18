using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Airport.Data.Migrations
{
  public partial class AddedDataAnnotations : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AlterColumn<string>(
          name: "LastName",
          table: "Pilot",
          maxLength: 32,
          nullable: false,
          oldClrType: typeof(string),
          oldNullable: true);

      migrationBuilder.AlterColumn<string>(
          name: "FirstName",
          table: "Pilot",
          maxLength: 32,
          nullable: false,
          oldClrType: typeof(string),
          oldNullable: true);

      migrationBuilder.AlterColumn<string>(
          name: "Number",
          table: "Flight",
          maxLength: 6,
          nullable: true,
          oldClrType: typeof(string),
          oldNullable: true);

      migrationBuilder.AlterColumn<string>(
          name: "DeparturePlace",
          table: "Flight",
          maxLength: 32,
          nullable: true,
          oldClrType: typeof(string),
          oldNullable: true);

      migrationBuilder.AlterColumn<string>(
          name: "ArrivalPlace",
          table: "Flight",
          maxLength: 32,
          nullable: true,
          oldClrType: typeof(string),
          oldNullable: true);

      migrationBuilder.AlterColumn<DateTime>(
          name: "Date",
          table: "Departure",
          type: "date",
          nullable: false,
          oldClrType: typeof(DateTime));

      migrationBuilder.AlterColumn<string>(
          name: "LastName",
          table: "Airhostess",
          maxLength: 32,
          nullable: true,
          oldClrType: typeof(string),
          oldNullable: true);

      migrationBuilder.AlterColumn<string>(
          name: "FirstName",
          table: "Airhostess",
          maxLength: 32,
          nullable: true,
          oldClrType: typeof(string),
          oldNullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AlterColumn<string>(
          name: "LastName",
          table: "Pilot",
          nullable: true,
          oldClrType: typeof(string),
          oldMaxLength: 32,
          oldNullable: true);

      migrationBuilder.AlterColumn<string>(
          name: "FirstName",
          table: "Pilot",
          nullable: true,
          oldClrType: typeof(string),
          oldMaxLength: 32,
          oldNullable: true);

      migrationBuilder.AlterColumn<string>(
          name: "Number",
          table: "Flight",
          nullable: true,
          oldClrType: typeof(string),
          oldMaxLength: 6,
          oldNullable: true);

      migrationBuilder.AlterColumn<string>(
          name: "DeparturePlace",
          table: "Flight",
          nullable: true,
          oldClrType: typeof(string),
          oldMaxLength: 32,
          oldNullable: true);

      migrationBuilder.AlterColumn<string>(
          name: "ArrivalPlace",
          table: "Flight",
          nullable: true,
          oldClrType: typeof(string),
          oldMaxLength: 32,
          oldNullable: true);

      migrationBuilder.AlterColumn<DateTime>(
          name: "Date",
          table: "Departure",
          nullable: false,
          oldClrType: typeof(DateTime),
          oldType: "date");

      migrationBuilder.AlterColumn<string>(
          name: "LastName",
          table: "Airhostess",
          nullable: true,
          oldClrType: typeof(string),
          oldMaxLength: 32,
          oldNullable: true);

      migrationBuilder.AlterColumn<string>(
          name: "FirstName",
          table: "Airhostess",
          nullable: true,
          oldClrType: typeof(string),
          oldMaxLength: 32,
          oldNullable: true);
    }
  }
}
