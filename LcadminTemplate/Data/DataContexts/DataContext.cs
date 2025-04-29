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
        public DbSet<CompanyPhoneNumber> CompanyPhoneNumber { get; set; }
        public DbSet<CompanyUser> CompanyUser { get; set; }
        public DbSet<CompanyUserNote> CompanyUserNote { get; set; }
        public DbSet<CompanyUserPhoneNumber> CompanyUserPhoneNumber { get; set; }
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
        public DbSet<UserEmailAutoDelete> UserEmailAutoDelete {  get; set; }
        public DbSet<EmailNotifications> EmailNotifications {  get; set; }
        public DbSet<UnreadEmailSummaryView> UnreadEmailSummaryView {  get; set; }

        /*Exception Log*/
        public DbSet<ExceptionLog> ExceptionLog { get; set; }


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

    }

}
