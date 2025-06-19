using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class Removed_CompanyId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DownSellOffersIdMap_Company_CompanyId",
                table: "DownSellOffersIdMap");

            migrationBuilder.DropIndex(
                name: "IX_DownSellOffersIdMap_CompanyId",
                table: "DownSellOffersIdMap");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "DownSellOffersIdMap");

            //migrationBuilder.AlterColumn<int>(
            //    name: "Sourceid",
            //    table: "VwAllSubmissions",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0,
            //    oldClrType: typeof(int),
            //    oldType: "int",
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<int>(
            //    name: "Offerid",
            //    table: "VwAllSubmissions",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0,
            //    oldClrType: typeof(int),
            //    oldType: "int",
            //    oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AlterColumn<int>(
            //    name: "Sourceid",
            //    table: "VwAllSubmissions",
            //    type: "int",
            //    nullable: true,
            //    oldClrType: typeof(int),
            //    oldType: "int");

            //migrationBuilder.AlterColumn<int>(
            //    name: "Offerid",
            //    table: "VwAllSubmissions",
            //    type: "int",
            //    nullable: true,
            //    oldClrType: typeof(int),
            //    oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "DownSellOffersIdMap",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DownSellOffersIdMap_CompanyId",
                table: "DownSellOffersIdMap",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_DownSellOffersIdMap_Company_CompanyId",
                table: "DownSellOffersIdMap",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
