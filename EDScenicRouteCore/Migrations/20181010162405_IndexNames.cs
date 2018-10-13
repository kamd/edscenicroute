using Microsoft.EntityFrameworkCore.Migrations;

namespace EDScenicRouteCore.Migrations
{
    public partial class IndexNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_GalacticSystems_Name",
                table: "GalacticSystems",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_GalacticPOIs_Name",
                table: "GalacticPOIs",
                column: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GalacticSystems_Name",
                table: "GalacticSystems");

            migrationBuilder.DropIndex(
                name: "IX_GalacticPOIs_Name",
                table: "GalacticPOIs");
        }
    }
}
