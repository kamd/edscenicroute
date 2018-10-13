using Microsoft.EntityFrameworkCore.Migrations;

namespace EDScenicRouteCore.Migrations
{
    public partial class NamesCollateNocase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Custom migration because SQLite doesn't support AlterColumn.

            // Drop old indices
            migrationBuilder.DropIndex(
                name: "IX_GalacticSystems_Name",
                table: "GalacticSystems");

            migrationBuilder.DropIndex(
                name: "IX_GalacticPOIs_Name",
                table: "GalacticPOIs");

            // Create new tables with case-insensitive Names
            migrationBuilder.CreateTable(
                name: "GalacticPOIs_migration",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<int>(nullable: false),
                    Name = table.Column<string>(
                        nullable: true,
                        type: "TEXT COLLATE NOCASE"),
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
                name: "GalacticSystems_migration",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(
                        nullable: true,
                        type: "TEXT COLLATE NOCASE"),
                    Coordinates_X = table.Column<float>(nullable: false),
                    Coordinates_Y = table.Column<float>(nullable: false),
                    Coordinates_Z = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GalacticSystems", x => x.Id);
                });

            // copy data
            migrationBuilder.Sql(
                    "insert into GalacticPOIs_migration select * from GalacticPOIs;");

            migrationBuilder.Sql(
                    "insert into GalacticSystems_migration select * from GalacticSystems;");

            //drop old tables
            migrationBuilder.DropTable(
                name: "GalacticPOIs");

            migrationBuilder.DropTable(
                name: "GalacticSystems");


            //rename new table
            migrationBuilder.RenameTable(
                name: "GalacticPOIs_migration",
                newName: "GalacticPOIs");

            migrationBuilder.RenameTable(
                name: "GalacticSystems_migration",
                newName: "GalacticSystems");

            // recreate indices
            migrationBuilder.CreateIndex(
                name: "IX_GalacticSystems_Name",
                table: "GalacticSystems",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_GalacticPOIs_Name",
                table: "GalacticPOIs",
                column: "Name");

            // Auto generated migration left in comment form to demonstrate the intent of this migration:

            //migrationBuilder.AlterColumn<string>(
                //name: "Name",
                //table: "GalacticSystems",
                //type: "TEXT COLLATE NOCASE",
                //nullable: true,
                //oldClrType: typeof(string),
                //oldNullable: true);

            //migrationBuilder.AlterColumn<string>(
                //name: "Name",
                //table: "GalacticPOIs",
                //type: "TEXT COLLATE NOCASE",
                //nullable: true,
                //oldClrType: typeof(string),
                //oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            // Custom migration because SQLite doesn't support AlterColumn.

            // Drop old indices
            migrationBuilder.DropIndex(
                name: "IX_GalacticSystems_Name",
                table: "GalacticSystems");

            migrationBuilder.DropIndex(
                name: "IX_GalacticPOIs_Name",
                table: "GalacticPOIs");

            // Create new tables with case-insensitive Names
            migrationBuilder.CreateTable(
                name: "GalacticPOIs_migration",
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
                name: "GalacticSystems_migration",
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

            // copy data
            migrationBuilder.Sql(
                    "insert into GalacticPOIs_migration select * from GalacticPOIs;");

            migrationBuilder.Sql(
                    "insert into GalacticSystems_migration select * from GalacticSystems;");

            //drop old tables
            migrationBuilder.DropTable(
                name: "GalacticPOIs");

            migrationBuilder.DropTable(
                name: "GalacticSystems");


            //rename new table
            migrationBuilder.RenameTable(
                name: "GalacticPOIs_migration",
                newName: "GalacticPOIs");

            migrationBuilder.RenameTable(
                name: "GalacticSystems_migration",
                newName: "GalacticSystems");

            // recreate indices
            migrationBuilder.CreateIndex(
                name: "IX_GalacticSystems_Name",
                table: "GalacticSystems",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_GalacticPOIs_Name",
                table: "GalacticPOIs",
                column: "Name");

            // Auto generated migration left in comment form to demonstrate the intent of this migration:

            //migrationBuilder.AlterColumn<string>(
            //    name: "Name",
            //    table: "GalacticSystems",
            //    nullable: true,
            //    oldClrType: typeof(string),
            //    oldType: "TEXT COLLATE NOCASE",
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<string>(
            //    name: "Name",
            //    table: "GalacticPOIs",
            //    nullable: true,
            //    oldClrType: typeof(string),
            //    oldType: "TEXT COLLATE NOCASE",
            //    oldNullable: true);
        }
    }
}
