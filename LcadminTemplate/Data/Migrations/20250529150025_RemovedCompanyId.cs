using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovedCompanyId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Levels_Company_CompanyId",
                table: "Levels");

            migrationBuilder.DropForeignKey(
                name: "FK_LevelsIdMap_Company_CompanyId",
                table: "LevelsIdMap");

            migrationBuilder.DropForeignKey(
                name: "FK_Programs_Company_CompanyId",
                table: "Programs");

            migrationBuilder.DropForeignKey(
                name: "FK_ProgramsIdMap_Company_CompanyId",
                table: "ProgramsIdMap");

            migrationBuilder.DropIndex(
                name: "IX_ProgramsIdMap_CompanyId",
                table: "ProgramsIdMap");

            migrationBuilder.DropIndex(
                name: "IX_Programs_CompanyId",
                table: "Programs");

            migrationBuilder.DropIndex(
                name: "IX_LevelsIdMap_CompanyId",
                table: "LevelsIdMap");

            migrationBuilder.DropIndex(
                name: "IX_Levels_CompanyId",
                table: "Levels");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "ProgramsIdMap");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Programs");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "LevelsIdMap");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Levels");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "ProgramsIdMap",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Programs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "LevelsIdMap",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Levels",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProgramsIdMap_CompanyId",
                table: "ProgramsIdMap",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Programs_CompanyId",
                table: "Programs",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_LevelsIdMap_CompanyId",
                table: "LevelsIdMap",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Levels_CompanyId",
                table: "Levels",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Levels_Company_CompanyId",
                table: "Levels",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LevelsIdMap_Company_CompanyId",
                table: "LevelsIdMap",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Programs_Company_CompanyId",
                table: "Programs",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProgramsIdMap_Company_CompanyId",
                table: "ProgramsIdMap",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
