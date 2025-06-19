using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class Removed_CompanyId_From_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Areas_Company_CompanyId",
                table: "Areas");

            migrationBuilder.DropForeignKey(
                name: "FK_AreasIdMap_Company_CompanyId",
                table: "AreasIdMap");

            migrationBuilder.DropForeignKey(
                name: "FK_Interests_Company_CompanyId",
                table: "Interests");

            migrationBuilder.DropForeignKey(
                name: "FK_InterestsIdMap_Company_CompanyId",
                table: "InterestsIdMap");

            migrationBuilder.DropForeignKey(
                name: "FK_Master_school_mappingsIdMap_Company_CompanyId",
                table: "Master_school_mappingsIdMap");

            migrationBuilder.DropForeignKey(
                name: "FK_Master_schoolsIdMap_Company_CompanyId",
                table: "Master_schoolsIdMap");

            migrationBuilder.DropForeignKey(
                name: "FK_MasterSchoolMappings_Company_CompanyId",
                table: "MasterSchoolMappings");

            migrationBuilder.DropForeignKey(
                name: "FK_MasterSchools_Company_CompanyId",
                table: "MasterSchools");

            migrationBuilder.DropForeignKey(
                name: "FK_Programareas_Company_CompanyId",
                table: "Programareas");

            migrationBuilder.DropForeignKey(
                name: "FK_ProgramareasIdMap_Company_CompanyId",
                table: "ProgramareasIdMap");

            migrationBuilder.DropForeignKey(
                name: "FK_Programinterests_Company_CompanyId",
                table: "Programinterests");

            migrationBuilder.DropForeignKey(
                name: "FK_PrograminterestsIdMap_Company_CompanyId",
                table: "PrograminterestsIdMap");

            migrationBuilder.DropIndex(
                name: "IX_PrograminterestsIdMap_CompanyId",
                table: "PrograminterestsIdMap");

            migrationBuilder.DropIndex(
                name: "IX_Programinterests_CompanyId",
                table: "Programinterests");

            migrationBuilder.DropIndex(
                name: "IX_ProgramareasIdMap_CompanyId",
                table: "ProgramareasIdMap");

            migrationBuilder.DropIndex(
                name: "IX_Programareas_CompanyId",
                table: "Programareas");

            migrationBuilder.DropIndex(
                name: "IX_MasterSchools_CompanyId",
                table: "MasterSchools");

            migrationBuilder.DropIndex(
                name: "IX_MasterSchoolMappings_CompanyId",
                table: "MasterSchoolMappings");

            migrationBuilder.DropIndex(
                name: "IX_Master_schoolsIdMap_CompanyId",
                table: "Master_schoolsIdMap");

            migrationBuilder.DropIndex(
                name: "IX_Master_school_mappingsIdMap_CompanyId",
                table: "Master_school_mappingsIdMap");

            migrationBuilder.DropIndex(
                name: "IX_InterestsIdMap_CompanyId",
                table: "InterestsIdMap");

            migrationBuilder.DropIndex(
                name: "IX_Interests_CompanyId",
                table: "Interests");

            migrationBuilder.DropIndex(
                name: "IX_AreasIdMap_CompanyId",
                table: "AreasIdMap");

            migrationBuilder.DropIndex(
                name: "IX_Areas_CompanyId",
                table: "Areas");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "PrograminterestsIdMap");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Programinterests");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "ProgramareasIdMap");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Programareas");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "MasterSchools");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "MasterSchoolMappings");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Master_schoolsIdMap");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Master_school_mappingsIdMap");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "InterestsIdMap");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Interests");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "AreasIdMap");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Areas");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "PrograminterestsIdMap",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Programinterests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "ProgramareasIdMap",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Programareas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "MasterSchools",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "MasterSchoolMappings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Master_schoolsIdMap",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Master_school_mappingsIdMap",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "InterestsIdMap",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Interests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "AreasIdMap",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Areas",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PrograminterestsIdMap_CompanyId",
                table: "PrograminterestsIdMap",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Programinterests_CompanyId",
                table: "Programinterests",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramareasIdMap_CompanyId",
                table: "ProgramareasIdMap",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Programareas_CompanyId",
                table: "Programareas",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_MasterSchools_CompanyId",
                table: "MasterSchools",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_MasterSchoolMappings_CompanyId",
                table: "MasterSchoolMappings",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Master_schoolsIdMap_CompanyId",
                table: "Master_schoolsIdMap",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Master_school_mappingsIdMap_CompanyId",
                table: "Master_school_mappingsIdMap",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_InterestsIdMap_CompanyId",
                table: "InterestsIdMap",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Interests_CompanyId",
                table: "Interests",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_AreasIdMap_CompanyId",
                table: "AreasIdMap",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Areas_CompanyId",
                table: "Areas",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Areas_Company_CompanyId",
                table: "Areas",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AreasIdMap_Company_CompanyId",
                table: "AreasIdMap",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Interests_Company_CompanyId",
                table: "Interests",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InterestsIdMap_Company_CompanyId",
                table: "InterestsIdMap",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Master_school_mappingsIdMap_Company_CompanyId",
                table: "Master_school_mappingsIdMap",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Master_schoolsIdMap_Company_CompanyId",
                table: "Master_schoolsIdMap",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MasterSchoolMappings_Company_CompanyId",
                table: "MasterSchoolMappings",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MasterSchools_Company_CompanyId",
                table: "MasterSchools",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Programareas_Company_CompanyId",
                table: "Programareas",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProgramareasIdMap_Company_CompanyId",
                table: "ProgramareasIdMap",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Programinterests_Company_CompanyId",
                table: "Programinterests",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PrograminterestsIdMap_Company_CompanyId",
                table: "PrograminterestsIdMap",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
