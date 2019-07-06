using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace EDScenicRouteCore.Migrations
{
    public partial class Create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GalacticPOIs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Type = table.Column<int>(nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(200) UNIQUE", maxLength: 200, nullable: true),
                    GalMapSearch = table.Column<string>(maxLength: 600, nullable: true),
                    GalMapUrl = table.Column<string>(maxLength: 1000, nullable: true),
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
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(type: "VARCHAR(200) UNIQUE", maxLength: 200, nullable: true),
                    Coordinates_X = table.Column<float>(nullable: false),
                    Coordinates_Y = table.Column<float>(nullable: false),
                    Coordinates_Z = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GalacticSystems", x => x.Id);
                });
            
            migrationBuilder.Sql("CREATE INDEX \"TRGM_IX_GalacticSystems_Name\" ON \"public\".\"GalacticSystems\" USING gin (\"Name\" gin_trgm_ops);");
            migrationBuilder.Sql("CREATE INDEX \"TRGM_IX_GalacticPOIs_Name\" ON \"public\".\"GalacticPOIs\" USING gin (\"Name\" gin_trgm_ops);");
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
