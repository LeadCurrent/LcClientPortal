using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class Removing_CompanyId_from_CampusesTBL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Campuses_Company_CompanyId",
                table: "Campuses");

            migrationBuilder.DropIndex(
                name: "IX_Campuses_CompanyId",
                table: "Campuses");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Campuses");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Campuses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Campuses_CompanyId",
                table: "Campuses",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Campuses_Company_CompanyId",
                table: "Campuses",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id");
        }
    }
}
