using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class addCompanyIdtoTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "StateIdMap",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "SourceIdMap",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "SchoolIdMap",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "PostalCodeIdMap",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "OfferIdMap",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "ClientIdMap",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "CampusIdMap",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_StateIdMap_CompanyId",
                table: "StateIdMap",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_SourceIdMap_CompanyId",
                table: "SourceIdMap",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolIdMap_CompanyId",
                table: "SchoolIdMap",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_PostalCodeIdMap_CompanyId",
                table: "PostalCodeIdMap",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_OfferIdMap_CompanyId",
                table: "OfferIdMap",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientIdMap_CompanyId",
                table: "ClientIdMap",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CampusIdMap_CompanyId",
                table: "CampusIdMap",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_CampusIdMap_Company_CompanyId",
                table: "CampusIdMap",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientIdMap_Company_CompanyId",
                table: "ClientIdMap",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OfferIdMap_Company_CompanyId",
                table: "OfferIdMap",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostalCodeIdMap_Company_CompanyId",
                table: "PostalCodeIdMap",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolIdMap_Company_CompanyId",
                table: "SchoolIdMap",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SourceIdMap_Company_CompanyId",
                table: "SourceIdMap",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StateIdMap_Company_CompanyId",
                table: "StateIdMap",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CampusIdMap_Company_CompanyId",
                table: "CampusIdMap");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientIdMap_Company_CompanyId",
                table: "ClientIdMap");

            migrationBuilder.DropForeignKey(
                name: "FK_OfferIdMap_Company_CompanyId",
                table: "OfferIdMap");

            migrationBuilder.DropForeignKey(
                name: "FK_PostalCodeIdMap_Company_CompanyId",
                table: "PostalCodeIdMap");

            migrationBuilder.DropForeignKey(
                name: "FK_SchoolIdMap_Company_CompanyId",
                table: "SchoolIdMap");

            migrationBuilder.DropForeignKey(
                name: "FK_SourceIdMap_Company_CompanyId",
                table: "SourceIdMap");

            migrationBuilder.DropForeignKey(
                name: "FK_StateIdMap_Company_CompanyId",
                table: "StateIdMap");

            migrationBuilder.DropIndex(
                name: "IX_StateIdMap_CompanyId",
                table: "StateIdMap");

            migrationBuilder.DropIndex(
                name: "IX_SourceIdMap_CompanyId",
                table: "SourceIdMap");

            migrationBuilder.DropIndex(
                name: "IX_SchoolIdMap_CompanyId",
                table: "SchoolIdMap");

            migrationBuilder.DropIndex(
                name: "IX_PostalCodeIdMap_CompanyId",
                table: "PostalCodeIdMap");

            migrationBuilder.DropIndex(
                name: "IX_OfferIdMap_CompanyId",
                table: "OfferIdMap");

            migrationBuilder.DropIndex(
                name: "IX_ClientIdMap_CompanyId",
                table: "ClientIdMap");

            migrationBuilder.DropIndex(
                name: "IX_CampusIdMap_CompanyId",
                table: "CampusIdMap");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "StateIdMap");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "SourceIdMap");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "SchoolIdMap");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "PostalCodeIdMap");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "OfferIdMap");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "ClientIdMap");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "CampusIdMap");
        }
    }
}
