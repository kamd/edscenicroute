using Microsoft.EntityFrameworkCore.Migrations;

namespace EDScenicRouteCore.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GalacticPOIs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    GalMapSearch = table.Column<string>(nullable: true),
                    GalMapUrl = table.Column<string>(nullable: true),
                    Coordinates_X = table.Column<float>(nullable: false),
                    Coordinates_Y = table.Column<float>(nullable: false),
                    Coordinates_Z = table.Column<float>(nullable: false),
                    DistanceFromSol = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GalacticPOIs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GalacticSystems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Coordinates_X = table.Column<float>(nullable: false),
                    Coordinates_Y = table.Column<float>(nullable: false),
                    Coordinates_Z = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GalacticSystems", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GalacticPOIs");

            migrationBuilder.DropTable(
                name: "GalacticSystems");
        }
    }
}
