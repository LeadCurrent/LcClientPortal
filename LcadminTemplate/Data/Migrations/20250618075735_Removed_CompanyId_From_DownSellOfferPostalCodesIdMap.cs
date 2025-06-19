using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class Removed_CompanyId_From_DownSellOfferPostalCodesIdMap : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DownSellOfferPostalCodes_Company_CompanyId",
                table: "DownSellOfferPostalCodes");

            migrationBuilder.DropForeignKey(
                name: "FK_DownSellOfferPostalCodesIdMap_Company_CompanyId",
                table: "DownSellOfferPostalCodesIdMap");

            migrationBuilder.DropIndex(
                name: "IX_DownSellOfferPostalCodesIdMap_CompanyId",
                table: "DownSellOfferPostalCodesIdMap");

            migrationBuilder.DropIndex(
                name: "IX_DownSellOfferPostalCodes_CompanyId",
                table: "DownSellOfferPostalCodes");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "DownSellOfferPostalCodesIdMap");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "DownSellOfferPostalCodes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "DownSellOfferPostalCodesIdMap",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "DownSellOfferPostalCodes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DownSellOfferPostalCodesIdMap_CompanyId",
                table: "DownSellOfferPostalCodesIdMap",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_DownSellOfferPostalCodes_CompanyId",
                table: "DownSellOfferPostalCodes",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_DownSellOfferPostalCodes_Company_CompanyId",
                table: "DownSellOfferPostalCodes",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DownSellOfferPostalCodesIdMap_Company_CompanyId",
                table: "DownSellOfferPostalCodesIdMap",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
