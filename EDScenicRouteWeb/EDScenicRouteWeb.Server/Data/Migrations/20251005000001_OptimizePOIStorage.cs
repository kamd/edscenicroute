using Microsoft.EntityFrameworkCore.Migrations;

namespace EDScenicRouteWeb.Server.Data.Migrations
{
    public partial class OptimizePOIStorage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Optimize GalacticPOIs text field sizes
            migrationBuilder.Sql(@"
                -- Drop the trigram index on Name temporarily
                DROP INDEX IF EXISTS ""TRGM_IX_GalacticPOIs_Name"";

                -- Optimize Name: VARCHAR(200) -> VARCHAR(100)
                ALTER TABLE ""GalacticPOIs""
                ALTER COLUMN ""Name"" TYPE VARCHAR(100);

                -- Optimize GalMapUrl: VARCHAR(1000) -> VARCHAR(100)
                ALTER TABLE ""GalacticPOIs""
                ALTER COLUMN ""GalMapUrl"" TYPE VARCHAR(100);

                -- Optimize GalMapSearch: VARCHAR(600) -> VARCHAR(50)
                ALTER TABLE ""GalacticPOIs""
                ALTER COLUMN ""GalMapSearch"" TYPE VARCHAR(50);

                -- Optimize Body: VARCHAR(150) -> VARCHAR(50)
                ALTER TABLE ""GalacticPOIs""
                ALTER COLUMN ""Body"" TYPE VARCHAR(50);

                -- Recreate the trigram index on Name
                CREATE INDEX ""TRGM_IX_GalacticPOIs_Name""
                ON ""public"".""GalacticPOIs""
                USING gin (""Name"" gin_trgm_ops);
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert GalacticPOIs optimizations
            migrationBuilder.Sql(@"
                -- Drop the trigram index
                DROP INDEX IF EXISTS ""TRGM_IX_GalacticPOIs_Name"";

                -- Revert Name: VARCHAR(100) -> VARCHAR(200)
                ALTER TABLE ""GalacticPOIs""
                ALTER COLUMN ""Name"" TYPE VARCHAR(200);

                -- Revert GalMapUrl: VARCHAR(100) -> VARCHAR(1000)
                ALTER TABLE ""GalacticPOIs""
                ALTER COLUMN ""GalMapUrl"" TYPE VARCHAR(1000);

                -- Revert GalMapSearch: VARCHAR(50) -> VARCHAR(600)
                ALTER TABLE ""GalacticPOIs""
                ALTER COLUMN ""GalMapSearch"" TYPE VARCHAR(600);

                -- Revert Body: VARCHAR(50) -> VARCHAR(150)
                ALTER TABLE ""GalacticPOIs""
                ALTER COLUMN ""Body"" TYPE VARCHAR(150);

                -- Recreate the trigram index
                CREATE INDEX ""TRGM_IX_GalacticPOIs_Name""
                ON ""public"".""GalacticPOIs""
                USING gin (""Name"" gin_trgm_ops);
            ");
        }
    }
}
