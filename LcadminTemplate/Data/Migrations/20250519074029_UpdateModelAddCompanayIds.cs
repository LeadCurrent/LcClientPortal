using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModelAddCompanayIds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Campuses_PortalStates_PortalStatesId",
                table: "Campuses");

            migrationBuilder.RenameColumn(
                name: "PortalStatesId",
                table: "Campuses",
                newName: "PortalStatesid");

            migrationBuilder.RenameIndex(
                name: "IX_Campuses_PortalStatesId",
                table: "Campuses",
                newName: "IX_Campuses_PortalStatesid");

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Schoolgroups",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "oldId",
                table: "Schoolgroups",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Programs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "oldId",
                table: "Programs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Programinterests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "oldId",
                table: "Programinterests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Programareas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "oldId",
                table: "Programareas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "MasterSchools",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "oldId",
                table: "MasterSchools",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "MasterSchoolMappings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "oldId",
                table: "MasterSchoolMappings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Levels",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "oldId",
                table: "Levels",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Interests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "oldId",
                table: "Interests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Groups",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "oldId",
                table: "Groups",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "DownSellOffers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "oldId",
                table: "DownSellOffers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "DownSellOfferPostalCodes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "oldId",
                table: "DownSellOfferPostalCodes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Degreeprograms",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "oldId",
                table: "Degreeprograms",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Campuspostalcodes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "oldId",
                table: "Campuspostalcodes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Campusdegrees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "oldId",
                table: "Campusdegrees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Areas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "oldId",
                table: "Areas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Allocations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "oldId",
                table: "Allocations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AllocationsIdMap",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OldId = table.Column<int>(type: "int", nullable: false),
                    NewId = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllocationsIdMap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AllocationsIdMap_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AreasIdMap",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OldId = table.Column<int>(type: "int", nullable: false),
                    NewId = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AreasIdMap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AreasIdMap_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CampusdegreeIdMap",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OldId = table.Column<int>(type: "int", nullable: false),
                    NewId = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampusdegreeIdMap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CampusdegreeIdMap_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CampuspostalcodesIdMap",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OldId = table.Column<int>(type: "int", nullable: false),
                    NewId = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampuspostalcodesIdMap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CampuspostalcodesIdMap_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DegreeprogramsIdMap",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OldId = table.Column<int>(type: "int", nullable: false),
                    NewId = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DegreeprogramsIdMap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DegreeprogramsIdMap_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DownSellOfferPostalCodesIdMap",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OldId = table.Column<int>(type: "int", nullable: false),
                    NewId = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DownSellOfferPostalCodesIdMap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DownSellOfferPostalCodesIdMap_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DownSellOffersIdMap",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OldId = table.Column<int>(type: "int", nullable: false),
                    NewId = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DownSellOffersIdMap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DownSellOffersIdMap_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupIdMap",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OldId = table.Column<int>(type: "int", nullable: false),
                    NewId = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupIdMap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupIdMap_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InterestsIdMap",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OldId = table.Column<int>(type: "int", nullable: false),
                    NewId = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterestsIdMap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InterestsIdMap_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LevelsIdMap",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OldId = table.Column<int>(type: "int", nullable: false),
                    NewId = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LevelsIdMap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LevelsIdMap_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Master_school_mappingsIdMap",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OldId = table.Column<int>(type: "int", nullable: false),
                    NewId = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Master_school_mappingsIdMap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Master_school_mappingsIdMap_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Master_schoolsIdMap",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OldId = table.Column<int>(type: "int", nullable: false),
                    NewId = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Master_schoolsIdMap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Master_schoolsIdMap_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProgramareasIdMap",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OldId = table.Column<int>(type: "int", nullable: false),
                    NewId = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgramareasIdMap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProgramareasIdMap_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrograminterestsIdMap",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OldId = table.Column<int>(type: "int", nullable: false),
                    NewId = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrograminterestsIdMap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrograminterestsIdMap_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProgramsIdMap",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OldId = table.Column<int>(type: "int", nullable: false),
                    NewId = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgramsIdMap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProgramsIdMap_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SchoolGroupsIdMap",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OldId = table.Column<int>(type: "int", nullable: false),
                    NewId = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolGroupsIdMap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolGroupsIdMap_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Schoolgroups_CompanyId",
                table: "Schoolgroups",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Programs_CompanyId",
                table: "Programs",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Programinterests_CompanyId",
                table: "Programinterests",
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
                name: "IX_Levels_CompanyId",
                table: "Levels",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Interests_CompanyId",
                table: "Interests",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_CompanyId",
                table: "Groups",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_DownSellOffers_CompanyId",
                table: "DownSellOffers",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_DownSellOfferPostalCodes_CompanyId",
                table: "DownSellOfferPostalCodes",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Degreeprograms_CompanyId",
                table: "Degreeprograms",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Campuspostalcodes_CompanyId",
                table: "Campuspostalcodes",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Campusdegrees_CompanyId",
                table: "Campusdegrees",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Areas_CompanyId",
                table: "Areas",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Allocations_CompanyId",
                table: "Allocations",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_AllocationsIdMap_CompanyId",
                table: "AllocationsIdMap",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_AreasIdMap_CompanyId",
                table: "AreasIdMap",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CampusdegreeIdMap_CompanyId",
                table: "CampusdegreeIdMap",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CampuspostalcodesIdMap_CompanyId",
                table: "CampuspostalcodesIdMap",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_DegreeprogramsIdMap_CompanyId",
                table: "DegreeprogramsIdMap",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_DownSellOfferPostalCodesIdMap_CompanyId",
                table: "DownSellOfferPostalCodesIdMap",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_DownSellOffersIdMap_CompanyId",
                table: "DownSellOffersIdMap",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupIdMap_CompanyId",
                table: "GroupIdMap",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_InterestsIdMap_CompanyId",
                table: "InterestsIdMap",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_LevelsIdMap_CompanyId",
                table: "LevelsIdMap",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Master_school_mappingsIdMap_CompanyId",
                table: "Master_school_mappingsIdMap",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Master_schoolsIdMap_CompanyId",
                table: "Master_schoolsIdMap",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramareasIdMap_CompanyId",
                table: "ProgramareasIdMap",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_PrograminterestsIdMap_CompanyId",
                table: "PrograminterestsIdMap",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramsIdMap_CompanyId",
                table: "ProgramsIdMap",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolGroupsIdMap_CompanyId",
                table: "SchoolGroupsIdMap",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Allocations_Company_CompanyId",
                table: "Allocations",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Areas_Company_CompanyId",
                table: "Areas",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Campusdegrees_Company_CompanyId",
                table: "Campusdegrees",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Campuses_PortalStates_PortalStatesid",
                table: "Campuses",
                column: "PortalStatesid",
                principalTable: "PortalStates",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Campuspostalcodes_Company_CompanyId",
                table: "Campuspostalcodes",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Degreeprograms_Company_CompanyId",
                table: "Degreeprograms",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DownSellOfferPostalCodes_Company_CompanyId",
                table: "DownSellOfferPostalCodes",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DownSellOffers_Company_CompanyId",
                table: "DownSellOffers",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Company_CompanyId",
                table: "Groups",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Interests_Company_CompanyId",
                table: "Interests",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Levels_Company_CompanyId",
                table: "Levels",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id");

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
                name: "FK_Programinterests_Company_CompanyId",
                table: "Programinterests",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Programs_Company_CompanyId",
                table: "Programs",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Schoolgroups_Company_CompanyId",
                table: "Schoolgroups",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Allocations_Company_CompanyId",
                table: "Allocations");

            migrationBuilder.DropForeignKey(
                name: "FK_Areas_Company_CompanyId",
                table: "Areas");

            migrationBuilder.DropForeignKey(
                name: "FK_Campusdegrees_Company_CompanyId",
                table: "Campusdegrees");

            migrationBuilder.DropForeignKey(
                name: "FK_Campuses_PortalStates_PortalStatesid",
                table: "Campuses");

            migrationBuilder.DropForeignKey(
                name: "FK_Campuspostalcodes_Company_CompanyId",
                table: "Campuspostalcodes");

            migrationBuilder.DropForeignKey(
                name: "FK_Degreeprograms_Company_CompanyId",
                table: "Degreeprograms");

            migrationBuilder.DropForeignKey(
                name: "FK_DownSellOfferPostalCodes_Company_CompanyId",
                table: "DownSellOfferPostalCodes");

            migrationBuilder.DropForeignKey(
                name: "FK_DownSellOffers_Company_CompanyId",
                table: "DownSellOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Company_CompanyId",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Interests_Company_CompanyId",
                table: "Interests");

            migrationBuilder.DropForeignKey(
                name: "FK_Levels_Company_CompanyId",
                table: "Levels");

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
                name: "FK_Programinterests_Company_CompanyId",
                table: "Programinterests");

            migrationBuilder.DropForeignKey(
                name: "FK_Programs_Company_CompanyId",
                table: "Programs");

            migrationBuilder.DropForeignKey(
                name: "FK_Schoolgroups_Company_CompanyId",
                table: "Schoolgroups");

            migrationBuilder.DropTable(
                name: "AllocationsIdMap");

            migrationBuilder.DropTable(
                name: "AreasIdMap");

            migrationBuilder.DropTable(
                name: "CampusdegreeIdMap");

            migrationBuilder.DropTable(
                name: "CampuspostalcodesIdMap");

            migrationBuilder.DropTable(
                name: "DegreeprogramsIdMap");

            migrationBuilder.DropTable(
                name: "DownSellOfferPostalCodesIdMap");

            migrationBuilder.DropTable(
                name: "DownSellOffersIdMap");

            migrationBuilder.DropTable(
                name: "GroupIdMap");

            migrationBuilder.DropTable(
                name: "InterestsIdMap");

            migrationBuilder.DropTable(
                name: "LevelsIdMap");

            migrationBuilder.DropTable(
                name: "Master_school_mappingsIdMap");

            migrationBuilder.DropTable(
                name: "Master_schoolsIdMap");

            migrationBuilder.DropTable(
                name: "ProgramareasIdMap");

            migrationBuilder.DropTable(
                name: "PrograminterestsIdMap");

            migrationBuilder.DropTable(
                name: "ProgramsIdMap");

            migrationBuilder.DropTable(
                name: "SchoolGroupsIdMap");

            migrationBuilder.DropIndex(
                name: "IX_Schoolgroups_CompanyId",
                table: "Schoolgroups");

            migrationBuilder.DropIndex(
                name: "IX_Programs_CompanyId",
                table: "Programs");

            migrationBuilder.DropIndex(
                name: "IX_Programinterests_CompanyId",
                table: "Programinterests");

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
                name: "IX_Levels_CompanyId",
                table: "Levels");

            migrationBuilder.DropIndex(
                name: "IX_Interests_CompanyId",
                table: "Interests");

            migrationBuilder.DropIndex(
                name: "IX_Groups_CompanyId",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_DownSellOffers_CompanyId",
                table: "DownSellOffers");

            migrationBuilder.DropIndex(
                name: "IX_DownSellOfferPostalCodes_CompanyId",
                table: "DownSellOfferPostalCodes");

            migrationBuilder.DropIndex(
                name: "IX_Degreeprograms_CompanyId",
                table: "Degreeprograms");

            migrationBuilder.DropIndex(
                name: "IX_Campuspostalcodes_CompanyId",
                table: "Campuspostalcodes");

            migrationBuilder.DropIndex(
                name: "IX_Campusdegrees_CompanyId",
                table: "Campusdegrees");

            migrationBuilder.DropIndex(
                name: "IX_Areas_CompanyId",
                table: "Areas");

            migrationBuilder.DropIndex(
                name: "IX_Allocations_CompanyId",
                table: "Allocations");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Schoolgroups");

            migrationBuilder.DropColumn(
                name: "oldId",
                table: "Schoolgroups");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Programs");

            migrationBuilder.DropColumn(
                name: "oldId",
                table: "Programs");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Programinterests");

            migrationBuilder.DropColumn(
                name: "oldId",
                table: "Programinterests");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Programareas");

            migrationBuilder.DropColumn(
                name: "oldId",
                table: "Programareas");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "MasterSchools");

            migrationBuilder.DropColumn(
                name: "oldId",
                table: "MasterSchools");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "MasterSchoolMappings");

            migrationBuilder.DropColumn(
                name: "oldId",
                table: "MasterSchoolMappings");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Levels");

            migrationBuilder.DropColumn(
                name: "oldId",
                table: "Levels");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Interests");

            migrationBuilder.DropColumn(
                name: "oldId",
                table: "Interests");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "oldId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "DownSellOffers");

            migrationBuilder.DropColumn(
                name: "oldId",
                table: "DownSellOffers");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "DownSellOfferPostalCodes");

            migrationBuilder.DropColumn(
                name: "oldId",
                table: "DownSellOfferPostalCodes");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Degreeprograms");

            migrationBuilder.DropColumn(
                name: "oldId",
                table: "Degreeprograms");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Campuspostalcodes");

            migrationBuilder.DropColumn(
                name: "oldId",
                table: "Campuspostalcodes");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Campusdegrees");

            migrationBuilder.DropColumn(
                name: "oldId",
                table: "Campusdegrees");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Areas");

            migrationBuilder.DropColumn(
                name: "oldId",
                table: "Areas");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Allocations");

            migrationBuilder.DropColumn(
                name: "oldId",
                table: "Allocations");

            migrationBuilder.RenameColumn(
                name: "PortalStatesid",
                table: "Campuses",
                newName: "PortalStatesId");

            migrationBuilder.RenameIndex(
                name: "IX_Campuses_PortalStatesid",
                table: "Campuses",
                newName: "IX_Campuses_PortalStatesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Campuses_PortalStates_PortalStatesId",
                table: "Campuses",
                column: "PortalStatesId",
                principalTable: "PortalStates",
                principalColumn: "Id");
        }
    }
}
