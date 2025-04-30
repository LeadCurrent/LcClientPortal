using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class Removed_FieldsFrom_TblCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JobId",
                table: "Document");

            migrationBuilder.DropColumn(
                name: "LeadId",
                table: "Document");

            migrationBuilder.DropColumn(
                name: "VendorId",
                table: "Document");

            migrationBuilder.DropColumn(
                name: "BillingAddress",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "BillingCity",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "BillingCompanyName",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "BillingEmail",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "BillingFirstName",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "BillingLastName",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "BillingPhone",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "BillingState",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "BillingZip",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "ExternalCustomerId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "LeadId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "QuickBooksCustomerId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "TaxRate",
                table: "Customer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "JobId",
                table: "Document",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LeadId",
                table: "Document",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VendorId",
                table: "Document",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BillingAddress",
                table: "Customer",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BillingCity",
                table: "Customer",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BillingCompanyName",
                table: "Customer",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BillingEmail",
                table: "Customer",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BillingFirstName",
                table: "Customer",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BillingLastName",
                table: "Customer",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BillingPhone",
                table: "Customer",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BillingState",
                table: "Customer",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BillingZip",
                table: "Customer",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExternalCustomerId",
                table: "Customer",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LeadId",
                table: "Customer",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "QuickBooksCustomerId",
                table: "Customer",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TaxRate",
                table: "Customer",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
