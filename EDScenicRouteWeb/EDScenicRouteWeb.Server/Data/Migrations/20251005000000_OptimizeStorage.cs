using Microsoft.EntityFrameworkCore.Migrations;

namespace EDScenicRouteWeb.Server.Data.Migrations
{
    public partial class OptimizeStorage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Phase 1: Optimize GalacticSystems.Name VARCHAR(200) -> VARCHAR(50)
            migrationBuilder.Sql(@"
                -- Drop the trigram index temporarily
                DROP INDEX IF EXISTS ""TRGM_IX_GalacticSystems_Name"";

                -- Alter the Name column type
                ALTER TABLE ""GalacticSystems""
                ALTER COLUMN ""Name"" TYPE VARCHAR(50);

                -- Recreate the trigram index (this will be much smaller now)
                CREATE INDEX ""TRGM_IX_GalacticSystems_Name""
                ON ""public"".""GalacticSystems""
                USING gin (""Name"" gin_trgm_ops);
            ");

            // Phase 2: Optimize GalacticPOIs.Type INT -> SMALLINT
            migrationBuilder.AlterColumn<short>(
                name: "Type",
                table: "GalacticPOIs",
                type: "SMALLINT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert GalacticSystems.Name VARCHAR(50) -> VARCHAR(200)
            migrationBuilder.Sql(@"
                -- Drop the trigram index
                DROP INDEX IF EXISTS ""TRGM_IX_GalacticSystems_Name"";

                -- Alter the Name column type back
                ALTER TABLE ""GalacticSystems""
                ALTER COLUMN ""Name"" TYPE VARCHAR(200);

                -- Recreate the trigram index
                CREATE INDEX ""TRGM_IX_GalacticSystems_Name""
                ON ""public"".""GalacticSystems""
                USING gin (""Name"" gin_trgm_ops);
            ");

            // Revert GalacticPOIs.Type SMALLINT -> INT
            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "GalacticPOIs",
                type: "integer",
                nullable: false,
                oldClrType: typeof(short),
                oldType: "SMALLINT");
        }
    }
}
