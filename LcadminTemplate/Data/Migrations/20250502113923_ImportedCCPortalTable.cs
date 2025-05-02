using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class ImportedCCPortalTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Adminips",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ip = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adminips", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Apilogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Logtext = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Apilogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Areas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Copy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Direct = table.Column<bool>(type: "bit", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clients_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DownSellOfferPostalCodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DownSellOfferId = table.Column<int>(type: "int", nullable: false),
                    Postalcodeid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DownSellOfferPostalCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DownSellOffers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Clientid = table.Column<int>(type: "int", nullable: false),
                    Formurl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Dcap = table.Column<bool>(type: "bit", nullable: false),
                    Dcapamt = table.Column<int>(type: "int", nullable: false),
                    Mcap = table.Column<bool>(type: "bit", nullable: false),
                    Mcapamt = table.Column<int>(type: "int", nullable: false),
                    Wcap = table.Column<bool>(type: "bit", nullable: false),
                    Wcapamt = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Transferphone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IncludeUscitizens = table.Column<bool>(type: "bit", nullable: false),
                    IncludePermanentResidents = table.Column<bool>(type: "bit", nullable: false),
                    IncludeGreenCardHolders = table.Column<bool>(type: "bit", nullable: false),
                    IncludeNonCitizens = table.Column<bool>(type: "bit", nullable: false),
                    IncludeInternet = table.Column<bool>(type: "bit", nullable: false),
                    IncludeNoInternet = table.Column<bool>(type: "bit", nullable: false),
                    IncludeMilitary = table.Column<bool>(type: "bit", nullable: false),
                    IncludeNonMilitary = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MondayActive = table.Column<bool>(type: "bit", nullable: false),
                    MondayStartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MondayEndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TuesdayActive = table.Column<bool>(type: "bit", nullable: false),
                    TuesdayStartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TuesdayEndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WednesdayActive = table.Column<bool>(type: "bit", nullable: false),
                    WednesdayStartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WednesdayEndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ThursdayActive = table.Column<bool>(type: "bit", nullable: false),
                    ThursdayStartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ThursdayEndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FridayActive = table.Column<bool>(type: "bit", nullable: false),
                    FridayStartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FridayEndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SaturdayActive = table.Column<bool>(type: "bit", nullable: false),
                    SaturdayStartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SaturdayEndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SundayActive = table.Column<bool>(type: "bit", nullable: false),
                    SundayStartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SundayEndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Identifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Minage = table.Column<int>(type: "int", nullable: false),
                    Maxage = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DownSellOffers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EduspotsEduapiLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Xml = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Searchreturnid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Eduapiid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EduspotsEduapiLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Extrarequirededucations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Degreeid = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Campusid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Extrarequirededucations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Copy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Interests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Copy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Leadposts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Parameterstring = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Serverresponse = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Testflag = table.Column<bool>(type: "bit", nullable: true),
                    Testparameter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Success = table.Column<bool>(type: "bit", nullable: true),
                    Ipaddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Vamidentifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Offerid = table.Column<int>(type: "int", nullable: true),
                    Schoolid = table.Column<int>(type: "int", nullable: true),
                    Sourceid = table.Column<int>(type: "int", nullable: true),
                    Zip = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Campusid = table.Column<int>(type: "int", nullable: true),
                    Campusname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Programid = table.Column<int>(type: "int", nullable: true),
                    Programname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Clientname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Offername = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Agent = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leadposts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Levels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Copy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Levels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Csfile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Error = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Exceptionmsg = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Innerexception = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MasterSchools",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterSchools", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PingCaches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PingSignature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Allowed = table.Column<bool>(type: "bit", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PingResponse = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SourceId = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PingCaches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PingCaches_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Portalclicks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Sourceid = table.Column<int>(type: "int", nullable: false),
                    Ipaddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Agentemail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Portalid = table.Column<int>(type: "int", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Clicked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Portalclicks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PortalStates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Abbr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Timezone = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PortalStates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Postalcodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Stateid = table.Column<int>(type: "int", nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Postalcodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PrepingLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrepingName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Response = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PingDetail = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrepingLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Programs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Copy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Programs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Schools",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Abbr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Logo100 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Minage = table.Column<int>(type: "int", nullable: true),
                    Maxage = table.Column<int>(type: "int", nullable: true),
                    Minhs = table.Column<int>(type: "int", nullable: true),
                    Maxhs = table.Column<int>(type: "int", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Shortcopy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Targeting = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Accreditation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Highlights = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Alert = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Startdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Scoreadjustment = table.Column<int>(type: "int", nullable: false),
                    Militaryfriendly = table.Column<bool>(type: "bit", nullable: false),
                    Disclosure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Schoolgroup = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TcpaText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schools", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Schools_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Searchportals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Rank = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Transfers = table.Column<bool>(type: "bit", nullable: false),
                    Leads = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Searchportals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Apikey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lcsourceid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lcsiteid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Accesskey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sources_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Submissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Offerid = table.Column<int>(type: "int", nullable: false),
                    Sourceid = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Ipaddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Postalcode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Userconfidencelevel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Agentconfidencelevel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Partnerid = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Submissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TblConfigEducationLevels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Identifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblConfigEducationLevels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Eduapis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Clientid = table.Column<int>(type: "int", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dcap = table.Column<bool>(type: "bit", nullable: false),
                    Dcapamt = table.Column<int>(type: "int", nullable: false),
                    Wcap = table.Column<bool>(type: "bit", nullable: false),
                    Wcapamt = table.Column<int>(type: "int", nullable: false),
                    Mcap = table.Column<bool>(type: "bit", nullable: false),
                    Mcapamt = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eduapis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Eduapis_Clients_Clientid",
                        column: x => x.Clientid,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MasterSchoolMappings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MasterSchoolsId = table.Column<int>(type: "int", nullable: false),
                    Identifier = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterSchoolMappings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MasterSchoolMappings_MasterSchools_MasterSchoolsId",
                        column: x => x.MasterSchoolsId,
                        principalTable: "MasterSchools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Degreeprograms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Levelid = table.Column<int>(type: "int", nullable: false),
                    Programid = table.Column<int>(type: "int", nullable: false),
                    Copy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Degreeprograms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Degreeprograms_Levels_Levelid",
                        column: x => x.Levelid,
                        principalTable: "Levels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Degreeprograms_Programs_Programid",
                        column: x => x.Programid,
                        principalTable: "Programs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Programareas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Programid = table.Column<int>(type: "int", nullable: false),
                    Areaid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Programareas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Programareas_Areas_Areaid",
                        column: x => x.Areaid,
                        principalTable: "Areas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Programareas_Programs_Programid",
                        column: x => x.Programid,
                        principalTable: "Programs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Programinterests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Programid = table.Column<int>(type: "int", nullable: false),
                    Interestid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Programinterests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Programinterests_Interests_Interestid",
                        column: x => x.Interestid,
                        principalTable: "Interests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Programinterests_Programs_Programid",
                        column: x => x.Programid,
                        principalTable: "Programs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Campuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Schoolid = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Stateid = table.Column<int>(type: "int", nullable: true),
                    Postalcodeid = table.Column<int>(type: "int", nullable: true),
                    Campustype = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Copy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Clientid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PortalStatesId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Campuses_PortalStates_PortalStatesId",
                        column: x => x.PortalStatesId,
                        principalTable: "PortalStates",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Campuses_Postalcodes_Postalcodeid",
                        column: x => x.Postalcodeid,
                        principalTable: "Postalcodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Campuses_Schools_Schoolid",
                        column: x => x.Schoolid,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Offers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Schoolid = table.Column<int>(type: "int", nullable: false),
                    Clientid = table.Column<int>(type: "int", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Rpl = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Dcap = table.Column<bool>(type: "bit", nullable: false),
                    Dcapamt = table.Column<int>(type: "int", nullable: false),
                    Mcap = table.Column<bool>(type: "bit", nullable: false),
                    Mcapamt = table.Column<int>(type: "int", nullable: false),
                    Wcap = table.Column<bool>(type: "bit", nullable: false),
                    Wcapamt = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Militaryonly = table.Column<bool>(type: "bit", nullable: false),
                    Nomilitary = table.Column<bool>(type: "bit", nullable: false),
                    Transferphone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lccampaignid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Archive = table.Column<bool>(type: "bit", nullable: false),
                    EndClient = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CecRplA = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CecRplB = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CecRplC = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CecRplD = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CecRplE = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CecRplF = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CecRplG = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DeliveryIdentifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeliveryName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Offers_Clients_Clientid",
                        column: x => x.Clientid,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Offers_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Offers_Schools_Schoolid",
                        column: x => x.Schoolid,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Schoolgroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Groupid = table.Column<int>(type: "int", nullable: false),
                    Schoolid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schoolgroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Schoolgroups_Groups_Groupid",
                        column: x => x.Groupid,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Schoolgroups_Schools_Schoolid",
                        column: x => x.Schoolid,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Schoolhighlights",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Schoolid = table.Column<int>(type: "int", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schoolhighlights", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Schoolhighlights_Schools_Schoolid",
                        column: x => x.Schoolid,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Schoolstarts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Schoolid = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schoolstarts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Schoolstarts_Schools_Schoolid",
                        column: x => x.Schoolid,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Portaltargetings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Portalid = table.Column<int>(type: "int", nullable: false),
                    CitizenIncludeUscitizens = table.Column<bool>(type: "bit", nullable: false),
                    CitizenIncludePermanentResidents = table.Column<bool>(type: "bit", nullable: false),
                    CitizenIncludeGreenCardHolders = table.Column<bool>(type: "bit", nullable: false),
                    CitizenIncludeOther = table.Column<bool>(type: "bit", nullable: false),
                    InternetIncludeInternet = table.Column<bool>(type: "bit", nullable: false),
                    InternetIncludeNoInternet = table.Column<bool>(type: "bit", nullable: false),
                    MilitaryIncludeMilitary = table.Column<bool>(type: "bit", nullable: false),
                    MilitaryIncludeNonMilitary = table.Column<bool>(type: "bit", nullable: false),
                    StudentMinHighSchoolGradYear = table.Column<int>(type: "int", nullable: false),
                    StudentMaxHighSchoolGradYear = table.Column<int>(type: "int", nullable: false),
                    StudentMinAge = table.Column<int>(type: "int", nullable: false),
                    StudentMaxAge = table.Column<int>(type: "int", nullable: false),
                    LeadIpAddressRequired = table.Column<bool>(type: "bit", nullable: false),
                    MondayActive = table.Column<bool>(type: "bit", nullable: false),
                    MondayStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TuesdayStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WednesdayStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ThursdayStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FridayStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SaturdayStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SundayStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TuesdayActive = table.Column<bool>(type: "bit", nullable: false),
                    WednesdayActive = table.Column<bool>(type: "bit", nullable: false),
                    ThursdayActive = table.Column<bool>(type: "bit", nullable: false),
                    FridayActive = table.Column<bool>(type: "bit", nullable: false),
                    SaturdayActive = table.Column<bool>(type: "bit", nullable: false),
                    SundayActive = table.Column<bool>(type: "bit", nullable: false),
                    MondayEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TuesdayEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WednesdayEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ThursdayEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FridayEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SaturdayEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SundayEnd = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Portaltargetings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Portaltargetings_Searchportals_Portalid",
                        column: x => x.Portalid,
                        principalTable: "Searchportals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sourceips",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Sourceid = table.Column<int>(type: "int", nullable: false),
                    Ipaddress = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sourceips", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sourceips_Sources_Sourceid",
                        column: x => x.Sourceid,
                        principalTable: "Sources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Eduapitargetings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Eduapiid = table.Column<int>(type: "int", nullable: false),
                    CitizenIncludeUscitizens = table.Column<bool>(type: "bit", nullable: false),
                    CitizenIncludePermanentResidents = table.Column<bool>(type: "bit", nullable: false),
                    CitizenIncludeGreenCardHolders = table.Column<bool>(type: "bit", nullable: false),
                    CitizenIncludeOther = table.Column<bool>(type: "bit", nullable: false),
                    InternetIncludeInternet = table.Column<bool>(type: "bit", nullable: false),
                    InternetIncludeNoInternet = table.Column<bool>(type: "bit", nullable: false),
                    MilitaryIncludeMilitary = table.Column<bool>(type: "bit", nullable: false),
                    MilitaryIncludeNonMilitary = table.Column<bool>(type: "bit", nullable: false),
                    StudentMinHighSchoolGradYear = table.Column<int>(type: "int", nullable: false),
                    StudentMaxHighSchoolGradYear = table.Column<int>(type: "int", nullable: false),
                    StudentMinAge = table.Column<int>(type: "int", nullable: false),
                    StudentMaxAge = table.Column<int>(type: "int", nullable: false),
                    LeadIpAddressRequired = table.Column<bool>(type: "bit", nullable: false),
                    MondayActive = table.Column<bool>(type: "bit", nullable: false),
                    MondayStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TuesdayStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WednesdayStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ThursdayStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FridayStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SaturdayStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SundayStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TuesdayActive = table.Column<bool>(type: "bit", nullable: false),
                    WednesdayActive = table.Column<bool>(type: "bit", nullable: false),
                    ThursdayActive = table.Column<bool>(type: "bit", nullable: false),
                    FridayActive = table.Column<bool>(type: "bit", nullable: false),
                    SaturdayActive = table.Column<bool>(type: "bit", nullable: false),
                    SundayActive = table.Column<bool>(type: "bit", nullable: false),
                    MondayEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TuesdayEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WednesdayEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ThursdayEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FridayEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SaturdayEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SundayEnd = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eduapitargetings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Eduapitargetings_Eduapis_Eduapiid",
                        column: x => x.Eduapiid,
                        principalTable: "Eduapis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Campusdegrees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Campusid = table.Column<int>(type: "int", nullable: false),
                    Degreeid = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Copy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Clientid = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campusdegrees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Campusdegrees_Degreeprograms_Degreeid",
                        column: x => x.Degreeid,
                        principalTable: "Degreeprograms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Campuspostalcodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Campusid = table.Column<int>(type: "int", nullable: false),
                    Postalcodeid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campuspostalcodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Campuspostalcodes_Campuses_Campusid",
                        column: x => x.Campusid,
                        principalTable: "Campuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Campuspostalcodes_Postalcodes_Postalcodeid",
                        column: x => x.Postalcodeid,
                        principalTable: "Postalcodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Allocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Offerid = table.Column<int>(type: "int", nullable: false),
                    Sourceid = table.Column<int>(type: "int", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Identifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cpl = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Dcap = table.Column<bool>(type: "bit", nullable: false),
                    Dcapamt = table.Column<int>(type: "int", nullable: false),
                    Mcap = table.Column<bool>(type: "bit", nullable: false),
                    Mcapamt = table.Column<int>(type: "int", nullable: false),
                    Wcap = table.Column<bool>(type: "bit", nullable: false),
                    Wcapamt = table.Column<int>(type: "int", nullable: false),
                    Transferphone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CecIncludeA = table.Column<bool>(type: "bit", nullable: false),
                    CecIncludeB = table.Column<bool>(type: "bit", nullable: false),
                    CecIncludeC = table.Column<bool>(type: "bit", nullable: false),
                    CecIncludeD = table.Column<bool>(type: "bit", nullable: false),
                    CecIncludeE = table.Column<bool>(type: "bit", nullable: false),
                    CecIncludeF = table.Column<bool>(type: "bit", nullable: false),
                    CecIncludeG = table.Column<bool>(type: "bit", nullable: false),
                    CecCplA = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CecCplB = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CecCplC = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CecCplD = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CecCplE = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CecCplF = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CecCplG = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Allocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Allocations_Offers_Offerid",
                        column: x => x.Offerid,
                        principalTable: "Offers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Allocations_Sources_Sourceid",
                        column: x => x.Sourceid,
                        principalTable: "Sources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Offertargetings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Offerid = table.Column<int>(type: "int", nullable: false),
                    CitizenIncludeUscitizens = table.Column<bool>(type: "bit", nullable: false),
                    CitizenIncludePermanentResidents = table.Column<bool>(type: "bit", nullable: false),
                    CitizenIncludeGreenCardHolders = table.Column<bool>(type: "bit", nullable: false),
                    CitizenIncludeOther = table.Column<bool>(type: "bit", nullable: false),
                    InternetIncludeInternet = table.Column<bool>(type: "bit", nullable: false),
                    InternetIncludeNoInternet = table.Column<bool>(type: "bit", nullable: false),
                    MilitaryIncludeMilitary = table.Column<bool>(type: "bit", nullable: false),
                    MilitaryIncludeNonMilitary = table.Column<bool>(type: "bit", nullable: false),
                    StudentMinHighSchoolGradYear = table.Column<int>(type: "int", nullable: false),
                    StudentMaxHighSchoolGradYear = table.Column<int>(type: "int", nullable: false),
                    StudentMinAge = table.Column<int>(type: "int", nullable: false),
                    StudentMaxAge = table.Column<int>(type: "int", nullable: false),
                    LeadIpAddressRequired = table.Column<bool>(type: "bit", nullable: false),
                    MondayActive = table.Column<bool>(type: "bit", nullable: false),
                    MondayStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TuesdayStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WednesdayStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ThursdayStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FridayStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SaturdayStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SundayStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TuesdayActive = table.Column<bool>(type: "bit", nullable: false),
                    WednesdayActive = table.Column<bool>(type: "bit", nullable: false),
                    ThursdayActive = table.Column<bool>(type: "bit", nullable: false),
                    FridayActive = table.Column<bool>(type: "bit", nullable: false),
                    SaturdayActive = table.Column<bool>(type: "bit", nullable: false),
                    SundayActive = table.Column<bool>(type: "bit", nullable: false),
                    MondayEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TuesdayEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WednesdayEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ThursdayEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FridayEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SaturdayEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SundayEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CecIncludeA = table.Column<bool>(type: "bit", nullable: false),
                    CecIncludeB = table.Column<bool>(type: "bit", nullable: false),
                    CecIncludeC = table.Column<bool>(type: "bit", nullable: false),
                    CecIncludeD = table.Column<bool>(type: "bit", nullable: false),
                    CecIncludeE = table.Column<bool>(type: "bit", nullable: false),
                    CecIncludeF = table.Column<bool>(type: "bit", nullable: false),
                    CecIncludeG = table.Column<bool>(type: "bit", nullable: false),
                    CecCplA = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CecCplB = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CecCplC = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CecCplD = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CecCplE = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CecCplF = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CecCplG = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offertargetings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Offertargetings_Offers_Offerid",
                        column: x => x.Offerid,
                        principalTable: "Offers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Allocationcampusdegrees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Allocationid = table.Column<int>(type: "int", nullable: false),
                    Campusdegreeid = table.Column<int>(type: "int", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Dcap = table.Column<bool>(type: "bit", nullable: false),
                    Dcapamt = table.Column<int>(type: "int", nullable: false),
                    Wcap = table.Column<bool>(type: "bit", nullable: false),
                    Wcapamt = table.Column<int>(type: "int", nullable: false),
                    Mcap = table.Column<bool>(type: "bit", nullable: false),
                    Mcapamt = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Allocationcampusdegrees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Allocationcampusdegrees_Allocations_Allocationid",
                        column: x => x.Allocationid,
                        principalTable: "Allocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Allocationcampusdegrees_Campusdegrees_Campusdegreeid",
                        column: x => x.Campusdegreeid,
                        principalTable: "Campusdegrees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Allocationcampuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Allocationid = table.Column<int>(type: "int", nullable: false),
                    Campusid = table.Column<int>(type: "int", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Dcap = table.Column<bool>(type: "bit", nullable: false),
                    Dcapamt = table.Column<int>(type: "int", nullable: false),
                    Wcap = table.Column<bool>(type: "bit", nullable: false),
                    Wcapamt = table.Column<int>(type: "int", nullable: false),
                    Mcap = table.Column<bool>(type: "bit", nullable: false),
                    Mcapamt = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Allocationcampuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Allocationcampuses_Allocations_Allocationid",
                        column: x => x.Allocationid,
                        principalTable: "Allocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Allocationcampusdegrees_Allocationid",
                table: "Allocationcampusdegrees",
                column: "Allocationid");

            migrationBuilder.CreateIndex(
                name: "IX_Allocationcampusdegrees_Campusdegreeid",
                table: "Allocationcampusdegrees",
                column: "Campusdegreeid");

            migrationBuilder.CreateIndex(
                name: "IX_Allocationcampuses_Allocationid",
                table: "Allocationcampuses",
                column: "Allocationid");

            migrationBuilder.CreateIndex(
                name: "IX_Allocations_Offerid",
                table: "Allocations",
                column: "Offerid");

            migrationBuilder.CreateIndex(
                name: "IX_Allocations_Sourceid",
                table: "Allocations",
                column: "Sourceid");

            migrationBuilder.CreateIndex(
                name: "IX_Campusdegrees_Degreeid",
                table: "Campusdegrees",
                column: "Degreeid");

            migrationBuilder.CreateIndex(
                name: "IX_Campuses_PortalStatesId",
                table: "Campuses",
                column: "PortalStatesId");

            migrationBuilder.CreateIndex(
                name: "IX_Campuses_Postalcodeid",
                table: "Campuses",
                column: "Postalcodeid");

            migrationBuilder.CreateIndex(
                name: "IX_Campuses_Schoolid",
                table: "Campuses",
                column: "Schoolid");

            migrationBuilder.CreateIndex(
                name: "IX_Campuspostalcodes_Campusid",
                table: "Campuspostalcodes",
                column: "Campusid");

            migrationBuilder.CreateIndex(
                name: "IX_Campuspostalcodes_Postalcodeid",
                table: "Campuspostalcodes",
                column: "Postalcodeid");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_CompanyId",
                table: "Clients",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Degreeprograms_Levelid",
                table: "Degreeprograms",
                column: "Levelid");

            migrationBuilder.CreateIndex(
                name: "IX_Degreeprograms_Programid",
                table: "Degreeprograms",
                column: "Programid");

            migrationBuilder.CreateIndex(
                name: "IX_Eduapis_Clientid",
                table: "Eduapis",
                column: "Clientid");

            migrationBuilder.CreateIndex(
                name: "IX_Eduapitargetings_Eduapiid",
                table: "Eduapitargetings",
                column: "Eduapiid");

            migrationBuilder.CreateIndex(
                name: "IX_MasterSchoolMappings_MasterSchoolsId",
                table: "MasterSchoolMappings",
                column: "MasterSchoolsId");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_Clientid",
                table: "Offers",
                column: "Clientid");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_CompanyId",
                table: "Offers",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_Schoolid",
                table: "Offers",
                column: "Schoolid");

            migrationBuilder.CreateIndex(
                name: "IX_Offertargetings_Offerid",
                table: "Offertargetings",
                column: "Offerid");

            migrationBuilder.CreateIndex(
                name: "IX_PingCaches_CompanyId",
                table: "PingCaches",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Portaltargetings_Portalid",
                table: "Portaltargetings",
                column: "Portalid");

            migrationBuilder.CreateIndex(
                name: "IX_Programareas_Areaid",
                table: "Programareas",
                column: "Areaid");

            migrationBuilder.CreateIndex(
                name: "IX_Programareas_Programid",
                table: "Programareas",
                column: "Programid");

            migrationBuilder.CreateIndex(
                name: "IX_Programinterests_Interestid",
                table: "Programinterests",
                column: "Interestid");

            migrationBuilder.CreateIndex(
                name: "IX_Programinterests_Programid",
                table: "Programinterests",
                column: "Programid");

            migrationBuilder.CreateIndex(
                name: "IX_Schoolgroups_Groupid",
                table: "Schoolgroups",
                column: "Groupid");

            migrationBuilder.CreateIndex(
                name: "IX_Schoolgroups_Schoolid",
                table: "Schoolgroups",
                column: "Schoolid");

            migrationBuilder.CreateIndex(
                name: "IX_Schoolhighlights_Schoolid",
                table: "Schoolhighlights",
                column: "Schoolid");

            migrationBuilder.CreateIndex(
                name: "IX_Schools_CompanyId",
                table: "Schools",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Schoolstarts_Schoolid",
                table: "Schoolstarts",
                column: "Schoolid");

            migrationBuilder.CreateIndex(
                name: "IX_Sourceips_Sourceid",
                table: "Sourceips",
                column: "Sourceid");

            migrationBuilder.CreateIndex(
                name: "IX_Sources_CompanyId",
                table: "Sources",
                column: "CompanyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Adminips");

            migrationBuilder.DropTable(
                name: "Allocationcampusdegrees");

            migrationBuilder.DropTable(
                name: "Allocationcampuses");

            migrationBuilder.DropTable(
                name: "Apilogs");

            migrationBuilder.DropTable(
                name: "Campuspostalcodes");

            migrationBuilder.DropTable(
                name: "DownSellOfferPostalCodes");

            migrationBuilder.DropTable(
                name: "DownSellOffers");

            migrationBuilder.DropTable(
                name: "Eduapitargetings");

            migrationBuilder.DropTable(
                name: "EduspotsEduapiLogs");

            migrationBuilder.DropTable(
                name: "Extrarequirededucations");

            migrationBuilder.DropTable(
                name: "Leadposts");

            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "MasterSchoolMappings");

            migrationBuilder.DropTable(
                name: "Offertargetings");

            migrationBuilder.DropTable(
                name: "PingCaches");

            migrationBuilder.DropTable(
                name: "Portalclicks");

            migrationBuilder.DropTable(
                name: "Portaltargetings");

            migrationBuilder.DropTable(
                name: "PrepingLogs");

            migrationBuilder.DropTable(
                name: "Programareas");

            migrationBuilder.DropTable(
                name: "Programinterests");

            migrationBuilder.DropTable(
                name: "Schoolgroups");

            migrationBuilder.DropTable(
                name: "Schoolhighlights");

            migrationBuilder.DropTable(
                name: "Schoolstarts");

            migrationBuilder.DropTable(
                name: "Sourceips");

            migrationBuilder.DropTable(
                name: "Submissions");

            migrationBuilder.DropTable(
                name: "TblConfigEducationLevels");

            migrationBuilder.DropTable(
                name: "Campusdegrees");

            migrationBuilder.DropTable(
                name: "Allocations");

            migrationBuilder.DropTable(
                name: "Campuses");

            migrationBuilder.DropTable(
                name: "Eduapis");

            migrationBuilder.DropTable(
                name: "MasterSchools");

            migrationBuilder.DropTable(
                name: "Searchportals");

            migrationBuilder.DropTable(
                name: "Areas");

            migrationBuilder.DropTable(
                name: "Interests");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Degreeprograms");

            migrationBuilder.DropTable(
                name: "Offers");

            migrationBuilder.DropTable(
                name: "Sources");

            migrationBuilder.DropTable(
                name: "PortalStates");

            migrationBuilder.DropTable(
                name: "Postalcodes");

            migrationBuilder.DropTable(
                name: "Levels");

            migrationBuilder.DropTable(
                name: "Programs");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Schools");
        }
    }
}
