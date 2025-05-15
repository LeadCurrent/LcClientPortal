using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class Added_CompanyId_In_TblCampuses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
