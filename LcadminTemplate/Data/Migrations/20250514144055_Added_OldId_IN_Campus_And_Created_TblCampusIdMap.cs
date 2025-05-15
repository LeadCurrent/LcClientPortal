using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class Added_OldId_IN_Campus_And_Created_TblCampusIdMap : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "oldId",
                table: "Campuses",
                type: "int",
                nullable: true);

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CampusIdMap");

            migrationBuilder.DropColumn(
                name: "oldId",
                table: "Campuses");
        }
    }
}
