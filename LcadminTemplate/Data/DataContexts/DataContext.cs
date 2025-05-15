using Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data
{
    public class DataContext : IdentityDbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {


        }

        /* Company */
        public DbSet<Company> Company { get; set; }
        public DbSet<CompanyContact> CompanyContact { get; set; }
        public DbSet<CompanyNote> CompanyNote { get; set; }
        public DbSet<CompanyUser> CompanyUser { get; set; }
        public DbSet<CompanyUserNote> CompanyUserNote { get; set; }
        public DbSet<CompayUserRole> CompayUserRole { get; set; }
        public DbSet<ContactUs> ContactUs { get; set; }

        /* Contact */

        /* Customer */
        public DbSet<Customer> Customer { get; set; }
        public DbSet<CustomerNote> CustomerNote { get; set; }
        public DbSet<CustomerUpload> CustomerUpload { get; set; }

        /* Document */
        public DbSet<Document> Document { get; set; }
        public DbSet<DocumentVersion> DocumentVersion { get; set; }
        public DbSet<CompanyFolder> CompanyFolder { get; set; }
        public DbSet<FolderAccess> FolderAccess { get; set; }

        /* Email */
        public DbSet<CompanyEmailAccount> CompanyEmailAccount { get; set; }
        public DbSet<CompanyUserEmail> CompanyUserEmail { get; set; }
        public DbSet<EmailRecipient> EmailRecipient { get; set; }
        public DbSet<SendEmail> SendEmail { get; set; }
        public DbSet<SendEmailRecipient> SendEmailRecipient { get; set; }
        public DbSet<UserSignatures> UserSignatures { get; set; }
        public DbSet<UserEmailAutoDelete> UserEmailAutoDelete { get; set; }
        public DbSet<EmailNotifications> EmailNotifications { get; set; }
        public DbSet<UnreadEmailSummaryView> UnreadEmailSummaryView { get; set; }

        /*Exception Log*/
        public DbSet<ExceptionLog> ExceptionLog { get; set; }

        /* Mapping Tables*/

        public DbSet<SchoolIdMap> SchoolIdMap { get; set; }
        public DbSet<ClientIdMap> ClientIdMap { get; set; }
        public DbSet<OfferIdMap> OfferIdMap { get; set; }
        public DbSet<SourceIdMap> SourceIdMap { get; set; }
        public DbSet<StateIdMap> StateIdMap { get; set; }
        public DbSet<PostalCodeIdMap> PostalCodeIdMap {  get; set; }
        public DbSet<CampusIdMap> CampusIdMap {  get; set; }



        /*Template*/
        public DbSet<Template> Template { get; set; }
        public DbSet<TemplateMultiSelect> TemplateMultiSelect { get; set; }

        /* User */
        public DbSet<Role> Role { get; set; }
        public DbSet<RolePermission> RolePermission { get; set; }
        public DbSet<StaffGroup> StaffGroup { get; set; }
        public DbSet<StaffGroupDivision> StaffGroupDivision { get; set; }
        public DbSet<StaffGroupUser> StaffGroupUser { get; set; }
        public DbSet<StaffNote> StaffNote { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserLoginHistory> UserLoginHistory { get; set; }


        /*New CMG DB*/
        public virtual DbSet<Adminip> Adminips { get; set; }

        public virtual DbSet<Allocation> Allocations { get; set; }

        public virtual DbSet<Allocationcampus> Allocationcampuses { get; set; }

        public virtual DbSet<Allocationcampusdegree> Allocationcampusdegrees { get; set; }

        public virtual DbSet<Apilog> Apilogs { get; set; }

        public virtual DbSet<Area> Areas { get; set; }

        public virtual DbSet<Campus> Campuses { get; set; }

        public virtual DbSet<Campusdegree> Campusdegrees { get; set; }

        public virtual DbSet<Campuspostalcode> Campuspostalcodes { get; set; }

        public virtual DbSet<Client> Clients { get; set; }

        public virtual DbSet<Degreeprogram> Degreeprograms { get; set; }

        public virtual DbSet<DownSellOffer> DownSellOffers { get; set; }

        public virtual DbSet<DownSellOfferPostalCode> DownSellOfferPostalCodes { get; set; }

        public virtual DbSet<Eduapi> Eduapis { get; set; }

        public virtual DbSet<Eduapitargeting> Eduapitargetings { get; set; }

        public virtual DbSet<EduspotsEduapiLog> EduspotsEduapiLogs { get; set; }

        public virtual DbSet<Extrarequirededucation> Extrarequirededucations { get; set; }

        public virtual DbSet<Group> Groups { get; set; }

        public virtual DbSet<Interest> Interests { get; set; }

        public virtual DbSet<Leadpost> Leadposts { get; set; }

        public virtual DbSet<Level> Levels { get; set; }

        public virtual DbSet<Log> Logs { get; set; }

        public virtual DbSet<MasterSchool> MasterSchools { get; set; }

        public virtual DbSet<MasterSchoolMapping> MasterSchoolMappings { get; set; }

        public virtual DbSet<Offer> Offers { get; set; }

        public virtual DbSet<Offertargeting> Offertargetings { get; set; }

        public virtual DbSet<PingCache> PingCaches { get; set; }

        public virtual DbSet<Portalclick> Portalclicks { get; set; }

        public virtual DbSet<Portaltargeting> Portaltargetings { get; set; }

        public virtual DbSet<Postalcode> Postalcodes { get; set; }

        public virtual DbSet<PrepingLog> PrepingLogs { get; set; }

        public virtual DbSet<Program> Programs { get; set; }

        public virtual DbSet<Programarea> Programareas { get; set; }

        public virtual DbSet<Programinterest> Programinterests { get; set; }

        public virtual DbSet<School> Schools { get; set; }

        public virtual DbSet<Schoolgroup> Schoolgroups { get; set; }

        public virtual DbSet<Schoolhighlight> Schoolhighlights { get; set; }

        public virtual DbSet<Schoolstart> Schoolstarts { get; set; }

        public virtual DbSet<Searchportal> Searchportals { get; set; }

        public virtual DbSet<Source> Sources { get; set; }

        public virtual DbSet<Sourceip> Sourceips { get; set; }

        public virtual DbSet<PortalStates> PortalStates { get; set; }

        public virtual DbSet<Submission> Submissions { get; set; }

        public virtual DbSet<TblConfigEducationLevel> TblConfigEducationLevels { get; set; }

        public virtual DbSet<VwAllSubmission> VwAllSubmissions { get; set; }

        public virtual DbSet<VwAllocation> VwAllocations { get; set; }

        public virtual DbSet<VwAllocationsApi> VwAllocationsApis { get; set; }

        public virtual DbSet<VwFullOfferDetailsApi> VwFullOfferDetailsApis { get; set; }

        public virtual DbSet<VwOffer> VwOffers { get; set; }

        public virtual DbSet<VwOffersApi> VwOffersApis { get; set; }

        public virtual DbSet<VwPingsCacheDetail> VwPingsCacheDetails { get; set; }

        public virtual DbSet<VwSchoolGroup> VwSchoolGroups { get; set; }
        public virtual DbSet<VwVendorAllocation> VwVendorAllocations { get; set; }
        public virtual DbSet<VxFulfillmentApiv1> VxFulfillmentApiv1s { get; set; }

    }

}
