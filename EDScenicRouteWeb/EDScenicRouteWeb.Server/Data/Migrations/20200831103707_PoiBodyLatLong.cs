using Microsoft.EntityFrameworkCore.Migrations;

namespace EDScenicRouteWeb.Server.Data.Migrations
{
    public partial class PoiBodyLatLong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "GalacticPOIs",
                type: "VARCHAR(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(200) UNIQUE",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Body",
                table: "GalacticPOIs",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Latitude",
                table: "GalacticPOIs",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Longitude",
                table: "GalacticPOIs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Body",
                table: "GalacticPOIs");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "GalacticPOIs");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "GalacticPOIs");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "GalacticPOIs",
                type: "VARCHAR(200) UNIQUE",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(200)",
                oldMaxLength: 200,
                oldNullable: true);
        }
    }
}
