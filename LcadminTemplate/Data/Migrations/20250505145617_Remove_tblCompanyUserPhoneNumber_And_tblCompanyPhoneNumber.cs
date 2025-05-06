using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class Remove_tblCompanyUserPhoneNumber_And_tblCompanyPhoneNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyUserPhoneNumber");

            migrationBuilder.DropTable(
                name: "CompanyPhoneNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompanyPhoneNumber",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    AllStaffAccess = table.Column<bool>(type: "bit", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyPhoneNumber", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyPhoneNumber_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanyUserPhoneNumber",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyPhoneNumberId = table.Column<int>(type: "int", nullable: false),
                    CompanyUserId = table.Column<int>(type: "int", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyUserPhoneNumber", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyUserPhoneNumber_CompanyPhoneNumber_CompanyPhoneNumberId",
                        column: x => x.CompanyPhoneNumberId,
                        principalTable: "CompanyPhoneNumber",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyUserPhoneNumber_CompanyUser_CompanyUserId",
                        column: x => x.CompanyUserId,
                        principalTable: "CompanyUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyPhoneNumber_CompanyId",
                table: "CompanyPhoneNumber",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyUserPhoneNumber_CompanyPhoneNumberId",
                table: "CompanyUserPhoneNumber",
                column: "CompanyPhoneNumberId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyUserPhoneNumber_CompanyUserId",
                table: "CompanyUserPhoneNumber",
                column: "CompanyUserId");
        }
    }
}
