using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class Created_table_SchoolIdMap_ClientIdMap_OfferIdMap_SourceIdMap : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "oldId",
                table: "Sources",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ClientIdMap",
                columns: table => new
                {
                    OldId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NewId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientIdMap", x => x.OldId);
                });

            migrationBuilder.CreateTable(
                name: "OfferIdMap",
                columns: table => new
                {
                    OldId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NewId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfferIdMap", x => x.OldId);
                });

            migrationBuilder.CreateTable(
                name: "SchoolIdMap",
                columns: table => new
                {
                    OldId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NewId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolIdMap", x => x.OldId);
                });

            migrationBuilder.CreateTable(
                name: "SourceIdMap",
                columns: table => new
                {
                    OldId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NewId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SourceIdMap", x => x.OldId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientIdMap");

            migrationBuilder.DropTable(
                name: "OfferIdMap");

            migrationBuilder.DropTable(
                name: "SchoolIdMap");

            migrationBuilder.DropTable(
                name: "SourceIdMap");

            migrationBuilder.DropColumn(
                name: "oldId",
                table: "Sources");
        }
    }
}
