using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class Added_OldID_Column_In_School_Client_Offer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "oldId",
                table: "Schools",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "oldId",
                table: "Offers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "oldId",
                table: "Clients",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "oldId",
                table: "Schools");

            migrationBuilder.DropColumn(
                name: "oldId",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "oldId",
                table: "Clients");
        }
    }
}
