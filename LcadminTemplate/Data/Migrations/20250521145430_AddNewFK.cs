using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNewFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_PingCaches_SourceId",
                table: "PingCaches",
                column: "SourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_PingCaches_Sources_SourceId",
                table: "PingCaches",
                column: "SourceId",
                principalTable: "Sources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PingCaches_Sources_SourceId",
                table: "PingCaches");

            migrationBuilder.DropIndex(
                name: "IX_PingCaches_SourceId",
                table: "PingCaches");
        }
    }
}
