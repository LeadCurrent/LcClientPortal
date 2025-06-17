using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class removeFKfromTBLTargetingPortal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Portaltargetings_Searchportals_Portalid",
                table: "Portaltargetings");

            migrationBuilder.DropIndex(
                name: "IX_Portaltargetings_Portalid",
                table: "Portaltargetings");

            migrationBuilder.AddColumn<int>(
                name: "SearchportalId",
                table: "Portaltargetings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Portaltargetings_SearchportalId",
                table: "Portaltargetings",
                column: "SearchportalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Portaltargetings_Searchportals_SearchportalId",
                table: "Portaltargetings",
                column: "SearchportalId",
                principalTable: "Searchportals",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Portaltargetings_Searchportals_SearchportalId",
                table: "Portaltargetings");

            migrationBuilder.DropIndex(
                name: "IX_Portaltargetings_SearchportalId",
                table: "Portaltargetings");

            migrationBuilder.DropColumn(
                name: "SearchportalId",
                table: "Portaltargetings");

            migrationBuilder.CreateIndex(
                name: "IX_Portaltargetings_Portalid",
                table: "Portaltargetings",
                column: "Portalid");

            migrationBuilder.AddForeignKey(
                name: "FK_Portaltargetings_Searchportals_Portalid",
                table: "Portaltargetings",
                column: "Portalid",
                principalTable: "Searchportals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
