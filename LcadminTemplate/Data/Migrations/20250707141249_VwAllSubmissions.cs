using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class VwAllSubmissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var createViewSql = @"
                CREATE VIEW [dbo].[vwAllSubmissions] AS
                SELECT 'l' AS SubmissionType, OfferId, SourceId, CAST([Date] AS DATE) AS SubmissionDate
                FROM LeadPosts
                WHERE Success = 1
                UNION ALL
                SELECT 's' AS SubmissionType, OfferId, SourceId, CAST([Date] AS DATE) AS SubmissionDate
                FROM Submissions;
            ";

            migrationBuilder.Sql(createViewSql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS [dbo].[vwAllSubmissions];");
        }
    }
}
