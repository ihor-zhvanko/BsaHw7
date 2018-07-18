using Microsoft.EntityFrameworkCore.Migrations;

namespace Airport.Data.Migrations
{
    public partial class AddedUniqueConstraint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Model",
                table: "PlaneType",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.CreateIndex(
                name: "IX_PlaneType_Model",
                table: "PlaneType",
                column: "Model",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Flight_Number",
                table: "Flight",
                column: "Number",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PlaneType_Model",
                table: "PlaneType");

            migrationBuilder.DropIndex(
                name: "IX_Flight_Number",
                table: "Flight");

            migrationBuilder.AlterColumn<string>(
                name: "Model",
                table: "PlaneType",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
