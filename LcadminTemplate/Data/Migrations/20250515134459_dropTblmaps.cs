using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class dropTblmaps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CampusIdMap");

            migrationBuilder.DropTable(
                name: "OfferIdMap");

            migrationBuilder.DropTable(
                name: "PostalCodeIdMap");

            migrationBuilder.DropTable(
                name: "SchoolIdMap");

            migrationBuilder.DropTable(
                name: "SourceIdMap");

            migrationBuilder.DropTable(
                name: "StateIdMap");

            migrationBuilder.CreateTable(
                name: "ClientIdMap",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OldId = table.Column<int>(type: "int", nullable: false),
                    NewId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientIdMap", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientIdMap");

            migrationBuilder.CreateTable(
                name: "CampusIdMap",
                columns: table => new
                {
                    OldId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NewId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampusIdMap", x => x.OldId);
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
                name: "PostalCodeIdMap",
                columns: table => new
                {
                    OldId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NewId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostalCodeIdMap", x => x.OldId);
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

            migrationBuilder.CreateTable(
                name: "StateIdMap",
                columns: table => new
                {
                    OldId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NewId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StateIdMap", x => x.OldId);
                });
        }
    }
}
