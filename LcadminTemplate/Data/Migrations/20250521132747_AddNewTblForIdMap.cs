using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNewTblForIdMap : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Offertargetings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "oldId",
                table: "Offertargetings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Leadposts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "oldId",
                table: "Leadposts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Extrarequirededucations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "oldId",
                table: "Extrarequirededucations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LeadpostsIdMap",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OldId = table.Column<int>(type: "int", nullable: false),
                    NewId = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeadpostsIdMap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeadpostsIdMap_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OffertargetingIdMap",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OldId = table.Column<int>(type: "int", nullable: false),
                    NewId = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OffertargetingIdMap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OffertargetingIdMap_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ping_cacheIdMap",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OldId = table.Column<int>(type: "int", nullable: false),
                    NewId = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ping_cacheIdMap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ping_cacheIdMap_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Offertargetings_CompanyId",
                table: "Offertargetings",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Leadposts_CompanyId",
                table: "Leadposts",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Extrarequirededucations_CompanyId",
                table: "Extrarequirededucations",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadpostsIdMap_CompanyId",
                table: "LeadpostsIdMap",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_OffertargetingIdMap_CompanyId",
                table: "OffertargetingIdMap",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Ping_cacheIdMap_CompanyId",
                table: "Ping_cacheIdMap",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Extrarequirededucations_Company_CompanyId",
                table: "Extrarequirededucations",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Leadposts_Company_CompanyId",
                table: "Leadposts",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Offertargetings_Company_CompanyId",
                table: "Offertargetings",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Extrarequirededucations_Company_CompanyId",
                table: "Extrarequirededucations");

            migrationBuilder.DropForeignKey(
                name: "FK_Leadposts_Company_CompanyId",
                table: "Leadposts");

            migrationBuilder.DropForeignKey(
                name: "FK_Offertargetings_Company_CompanyId",
                table: "Offertargetings");

            migrationBuilder.DropTable(
                name: "LeadpostsIdMap");

            migrationBuilder.DropTable(
                name: "OffertargetingIdMap");

            migrationBuilder.DropTable(
                name: "Ping_cacheIdMap");

            migrationBuilder.DropIndex(
                name: "IX_Offertargetings_CompanyId",
                table: "Offertargetings");

            migrationBuilder.DropIndex(
                name: "IX_Leadposts_CompanyId",
                table: "Leadposts");

            migrationBuilder.DropIndex(
                name: "IX_Extrarequirededucations_CompanyId",
                table: "Extrarequirededucations");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Offertargetings");

            migrationBuilder.DropColumn(
                name: "oldId",
                table: "Offertargetings");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Leadposts");

            migrationBuilder.DropColumn(
                name: "oldId",
                table: "Leadposts");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Extrarequirededucations");

            migrationBuilder.DropColumn(
                name: "oldId",
                table: "Extrarequirededucations");
        }
    }
}
