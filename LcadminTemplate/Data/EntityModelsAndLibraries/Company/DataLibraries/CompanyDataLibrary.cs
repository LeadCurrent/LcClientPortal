using Data;
using DocumentFormat.OpenXml.ExtendedProperties;
using Google.Apis.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;
using static Data.CompanyEnums;
using static Data.GeneralEnums;

namespace Data
{
    public class CompanyDataLibrary
    {
        public DataContext context { get; }
        static readonly char[] padding = { '=' };

        public CompanyDataLibrary(DataContext Context)
        {
            context = Context;
        }

        public async Task<List<Company>> GetCompanys()
        {
            List<Company> Companys = await context.Company
                .Where(s => s.Status == GeneralEnums.Status.Active)
                .OrderBy(s => s.Name)
                .ToListAsync();

            return Companys;
        }
        public async Task<List<Company>> GetFilteredCompanys(string FilterName, int FilterStatus)
        {
            GeneralEnums.Status filterStatus = (GeneralEnums.Status)FilterStatus;
            List<Company> ContractorCompanies = await context.Company
                .Where(x => x.Name.Contains(FilterName) && x.Status == (Status)filterStatus || FilterName == null && x.Status == (Status)filterStatus)
                .OrderBy(x => x.Name)
                .ToListAsync();

            return ContractorCompanies;
        }
        public async Task<Company> GetCompany(int CompanyId)
        {
            var Company = await context.Company
                .Include(x => x.CompanyContacts)
                .Where(s => s.Id == CompanyId).FirstOrDefaultAsync();

            return Company;
        }

        public async Task<Company> GetCompanyFromAPIKey(string APIKey)
        {
            var Company = await context.Company
                .Where(s => s.APIKey == APIKey).FirstOrDefaultAsync();

            return Company;
        }

        public async Task UpdateCompanyContact(CompanyContact CompanyContact, string CurrentUser)
        {
            context.ChangeTracker.Clear();
            var UpdateCompanyContact = await context.CompanyContact.Where(s => s.Id == CompanyContact.Id).FirstOrDefaultAsync();
            UpdateCompanyContact.UpdateDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time");
            UpdateCompanyContact.UpdatedBy = CurrentUser;
            UpdateCompanyContact.PrimaryContact = CompanyContact.PrimaryContact;
            context.Entry(UpdateCompanyContact).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task CreateCompanyContact(CompanyContact CompanyContact, string CurrentUser)
        {
            context.ChangeTracker.Clear();
            CompanyContact.UpdateDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time");
            CompanyContact.UpdatedBy = CurrentUser;
            CompanyContact.CreateDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time");
            CompanyContact.CreatedBy = CurrentUser;
            context.CompanyContact.Add(CompanyContact);
            await context.SaveChangesAsync();
        }

        public async Task<List<UserLoginHistory>> GetUserLogin(string CompanyUser)
        {
            return await context.UserLoginHistory.Where(x => x.UserId == CompanyUser).ToListAsync();
        }
        public async Task<User> GetUser(string CompanyUser)
        {
            return await context.User.Where(x => x.Id == CompanyUser).FirstOrDefaultAsync();
        }
        public async Task<Company> GetBasicCompany(int CompanyId)
        {
            var Company = await context.Company

                .Where(s => s.Id == CompanyId).FirstOrDefaultAsync();

            return Company;
        }

        public async Task<Company> GetCompanyWithContacts(int CompanyId)
        {
            var Company = await context.Company
                .Include(x=>x.CompanyContacts)
                    .ThenInclude(x=>x.CompanyUser)
                        .ThenInclude(x=>x.User)
                .Where(s => s.Id == CompanyId).FirstOrDefaultAsync();

            return Company;
        }

        public async Task<Company> GetCompanyProfile(int CompanyId)
        {
            var Company = await context.Company
                .Include(x => x.CompanyContacts)
                    .ThenInclude(x=>x.CompanyUser)
                        .ThenInclude(x=>x.User)
                .Include(x => x.CompanyNotes)
                .Where(s => s.Id == CompanyId).FirstOrDefaultAsync();

            Company.CompanyContacts = Company.CompanyContacts.OrderBy(x=>x.PrimaryContact).ThenBy(x => x.CompanyUser.User.FullName).ToList();

            return Company;
        }

        public async Task<Company> CreateCompany(Company Company, User user)
        {
            context.ChangeTracker.Clear();
            //var CurrentPlan = await context.Plans.Where(x => x.PlanPricing == PlanPricing.Default).FirstOrDefaultAsync();

            var CurrentCompany = new Company();
            CurrentCompany.Name = Company.Name;
            CurrentCompany.Email = Company.Email;
            CurrentCompany.Phone = Company.Phone;
            CurrentCompany.CompanyTimeZone = Company.CompanyTimeZone;
            CurrentCompany.UpdateDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time");
         
            CurrentCompany.UpdatedBy = user.FullName;
            CurrentCompany.CreateDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time");
            CurrentCompany.CreatedBy = user.FullName;

            
           

            //Billing fields
            var BillDate = CurrentCompany.CreateDate.AddDays(15).Day;
            BillDate = BillDate == 29 || BillDate == 30 || BillDate == 31 ? 1 : BillDate;

            context.Company.Add(CurrentCompany);
            await context.SaveChangesAsync();

            var companyUser = new CompanyUser
            {
                CompanyId = CurrentCompany.Id,
                UserId = user.Id
            };

            context.CompanyUser.Add(companyUser);
            await context.SaveChangesAsync();

            var CompanyContact = new CompanyContact
            {
                CompanyId = CurrentCompany.Id,
                CompanyUserId = companyUser.Id

            };

            context.CompanyContact.Add(CompanyContact);
            await context.SaveChangesAsync();

            return CurrentCompany;
        }

        public async Task<CompanyEmailAccount> GetEmailAccount(string EmailAccount)
        {
            return await context.CompanyEmailAccount
                .Where(u => u.Email == EmailAccount).FirstOrDefaultAsync();
        }
        public async Task CreateCompany(Company Company, string CurrentUser)
        {
            var user = await context.User.Where(u => u.UserName == CurrentUser).FirstOrDefaultAsync();
            Company.UpdateDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time");
            Company.UpdatedBy = user.FullName;
            Company.CreateDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time");
            Company.CreatedBy = user.FullName;
            context.Company.Add(Company);
            await context.SaveChangesAsync();

            var ccUser = new CompanyUser();
            ccUser.CreateDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time");
            ccUser.UserId = user.Id;
            ccUser.CompanyId = Company.Id;
            context.CompanyUser.Add(ccUser);
            await context.SaveChangesAsync();
        }

        public async Task<CompanyEmailAccount> GetDefaultOrFirstCompanyEmailAccount(int companyId)
        {
            // Fetch the default account, if none exists, fetch the first one
            var emailAccount = await context.CompanyEmailAccount.Where(x => x.CompanyId == companyId).FirstOrDefaultAsync(e => e.IsDefault);

            if (emailAccount == null)
            {
                emailAccount = await context.CompanyEmailAccount.Where(x => x.CompanyId == companyId).FirstOrDefaultAsync();
            }

            return emailAccount;
        }

        public async Task<CompanyEmailAccount> GetFeedbackEmail()
        {
            // Fetch the default account, if none exists, fetch the first one
            var emailAccount = await context.CompanyEmailAccount
                .Where(x => x.Name == "Feedback")
                .OrderByDescending(x => x.IsDefault)
                .ThenBy(x => x.Id)
                .FirstOrDefaultAsync();

            return emailAccount;
        }

        public async Task UpdateCompanyFromSystemAdmin(Company Company, string CurrentUser)
        {
            context.ChangeTracker.Clear();

            var user = await context.User.Where(u => u.UserName == CurrentUser).FirstOrDefaultAsync();
            var CurrentCompany = await context.Company.Where(x => x.Id == Company.Id).FirstOrDefaultAsync();
            CurrentCompany.Name = Company.Name;
            CurrentCompany.Address = Company.Address;
            CurrentCompany.City = Company.City;
            CurrentCompany.State = Company.State;
            CurrentCompany.ZipCode = Company.ZipCode;
            CurrentCompany.Phone = Company.Phone;
            CurrentCompany.Email = Company.Email;
            CurrentCompany.Status = Company.Status;
            CurrentCompany.APIKey = Company.APIKey;

            Company.UpdateDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time");
            Company.UpdatedBy = user.FullName;
            context.Entry(CurrentCompany).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task UpdateCompanyFromCompanyProfile(Company Company, string CurrentUser)
        {
            context.ChangeTracker.Clear();

            var user = await context.User.Where(u => u.UserName == CurrentUser).FirstOrDefaultAsync();
            var CurrentCompany = await context.Company.Where(x => x.Id == Company.Id).FirstOrDefaultAsync();
            CurrentCompany.Name = Company.Name;
            CurrentCompany.Address = Company.Address;
            CurrentCompany.City = Company.City;
            CurrentCompany.State = Company.State;
            CurrentCompany.ZipCode = Company.ZipCode;
            CurrentCompany.Phone = Company.Phone;
            CurrentCompany.Email = Company.Email;
            CurrentCompany.Logo = Company.Logo;
            CurrentCompany.CompanyTimeZone = Company.CompanyTimeZone;

            Company.UpdateDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time");
            Company.UpdatedBy = user.FullName;
            context.Entry(CurrentCompany).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task DeleteCompany(int CompanyId)
        {
            var Company = await context.Company.Where(s => s.Id == CompanyId).FirstOrDefaultAsync();
            context.Company.Remove(Company);
            await context.SaveChangesAsync();
        }

        public async Task RemoveCompany(int CompanyId)
        {
            var Company = await context.Company.Where(s => s.Id == CompanyId).FirstOrDefaultAsync();
            Company.Status = GeneralEnums.Status.Inactive;
            context.Entry(Company).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task UpdateCompanyForCompanyAccount(Company Company, string CurrentUser, int AccountHolderId, int BillingContactId, int SupportContactId)
        {
            context.ChangeTracker.Clear();
            var AccountHolderUser = await context.CompanyUser.Include(x => x.User).Where(x => x.Id == AccountHolderId).FirstOrDefaultAsync();
            var BillingContactUser = await context.CompanyUser.Include(x => x.User).Where(x => x.Id == BillingContactId).FirstOrDefaultAsync();
            var SupportContactUser = await context.CompanyUser.Include(x => x.User).Where(x => x.Id == SupportContactId).FirstOrDefaultAsync();
            var user = await context.User.Where(u => u.UserName == CurrentUser).FirstOrDefaultAsync();
            var CurrentCompany = await context.Company.Where(x => x.Id == Company.Id).FirstOrDefaultAsync();


            Company.UpdateDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time");
            Company.UpdatedBy = user.FullName;
            context.Entry(CurrentCompany).State = EntityState.Modified;
            await context.SaveChangesAsync();

            if (AccountHolderId > 0)
            {
                var AccHolder = await context.User.Where(x => x.Id == AccountHolderUser.UserId).FirstOrDefaultAsync();
                AccHolder.Email = AccountHolderUser.User.Email;
                AccHolder.Phone = AccountHolderUser.User.Phone;
                AccHolder.UpdateDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time");
                context.Entry(AccHolder).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }

            if (BillingContactId > 0)
            {
                var BillCont = await context.User.Where(x => x.Id == BillingContactUser.UserId).FirstOrDefaultAsync();
                BillCont.Email = BillingContactUser.User.Email;
                BillCont.Phone = BillingContactUser.User.Phone;
                BillCont.UpdateDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time");
                context.Entry(BillCont).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }

            if (SupportContactId > 0)
            {
                var SuppCont = await context.User.Where(x => x.Id == SupportContactUser.UserId).FirstOrDefaultAsync();
                SuppCont.Email = SupportContactUser.User.Email;
                SuppCont.Phone = SupportContactUser.User.Phone;
                SuppCont.UpdateDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time");
                context.Entry(SuppCont).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }

        public async Task<int> AddCompanyNote(string Note, NoteType NoteType, int CompanyId, string CurrentUser)
        {
            var user = await context.User.Where(u => u.UserName == CurrentUser).FirstOrDefaultAsync();
            var CompanyNote = new CompanyNote();
            CompanyNote.CreatedBy = user.FullName;
            CompanyNote.CreateDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time");
            CompanyNote.Note = Note;
            CompanyNote.CompanyId = CompanyId;
            CompanyNote.NoteType = NoteType;
            await context.CompanyNote.AddAsync(CompanyNote);
            await context.SaveChangesAsync();
            return CompanyNote.Id;
        }

        public async Task<List<CompanyNote>> GetCompanyNotes(int CompanyId)
        {
            var CompanyNotes = await context.CompanyNote
                .Where(x => x.CompanyId == CompanyId)
                .OrderBy(x => x.CreateDate)
                .ToListAsync();

            return CompanyNotes;
        }

        public async Task<CompayUserRole> GetCompayUserRole(int CompayUserRoleID)
        {
            var CompayUserRole = await context.CompayUserRole.Where(s => s.Id == CompayUserRoleID).FirstOrDefaultAsync();
            return CompayUserRole;
        }

        public async Task RemoveCompanyNote(int CompanyNoteId)
        {
            var CompanyNote = await context.CompanyNote.Where(x => x.Id == CompanyNoteId).FirstOrDefaultAsync();

            context.CompanyNote.Remove(CompanyNote);
            await context.SaveChangesAsync();
        }

        public async Task SaveCompany(Company Company, string CurrentUser)
        {
            var user = await context.User.Where(u => u.UserName == CurrentUser).FirstOrDefaultAsync();
            Company.UpdateDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time");
            Company.UpdatedBy = user.FullName;
            Company.CreateDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time");
            Company.CreatedBy = user.FullName;
            context.Company.Add(Company);
            await context.SaveChangesAsync();

            var ccUser = new CompanyUser();
            ccUser.CreateDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time");
            ccUser.UserId = user.Id;
            ccUser.CompanyId = Company.Id;
            context.CompanyUser.Add(ccUser);
            await context.SaveChangesAsync();
        }

        public async Task<CompanyUser> GetCompanyUser(int CompanyUser)
        {
            return await context.CompanyUser.Where(x => x.Id == CompanyUser).Include(x => x.User).Include(x => x.CompanyUserRoles).
                FirstOrDefaultAsync();
        }
        public async Task<int> GetCompanyUserIdByCompanyId(int CompanyId)
        {
            var companyUser = await context.CompanyUser
                                         .Where(x => x.CompanyId == CompanyId)
                                         .Select(x => x.Id)
                                         .FirstOrDefaultAsync();
            return companyUser;
        }
        public async Task<List<CompanyUser>> GetCompanyUsers(int CompanyId)
        {
            return await context.CompanyUser.Include(x => x.User).Include(x => x.CompanyUserRoles).ThenInclude(x => x.Role).Where(x => x.CompanyId == CompanyId && !x.User.IsDeleted).ToListAsync();
        }
       
        public async Task DeleteCompanyUser(int CompanyUserId)
        {
            var ccu = await context.CompanyUser.Where(x => x.Id == CompanyUserId).FirstOrDefaultAsync();
            ccu.Deleted = true;

            await context.SaveChangesAsync();
        }

        public async Task<int> CreateCompanyUser(int CompanyId, string UserId,
             string CurrentUser)
        {
            context.ChangeTracker.Clear();
            var user = await context.User.Where(u => u.UserName == CurrentUser).FirstOrDefaultAsync();
            var ccu = new CompanyUser();
            ccu.CreateDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time");
            ccu.CompanyId = CompanyId;
            ccu.UserId = UserId;
            ccu.Status = GeneralEnums.Status.Active;

            context.CompanyUser.Add(ccu);
            await context.SaveChangesAsync();

            return ccu.Id;

        }

        public async Task<int> AddCompanyUserNote(string Note, NoteType NoteType, int CompanyUserId, string CurrentUser)
        {
            var user = await context.User.Where(u => u.UserName == CurrentUser).FirstOrDefaultAsync();
            var CompanyUserNote = new CompanyUserNote();
            CompanyUserNote.CreatedBy = user.FullName;
            CompanyUserNote.CreateDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time");
            CompanyUserNote.Note = Note;
            CompanyUserNote.CompanyUserId = CompanyUserId;
            CompanyUserNote.NoteType = NoteType;
            await context.CompanyUserNote.AddAsync(CompanyUserNote);
            await context.SaveChangesAsync();
            return CompanyUserNote.Id;
        }

        public async Task<List<CompanyUserNote>> GetCompanyUserNotes(int JobId)
        {
            var CompanyUserNotes = await context.CompanyUserNote
                .Where(x => x.CompanyUserId == JobId)
                .OrderBy(x => x.CreateDate)
                .ToListAsync();

            return CompanyUserNotes;
        }

        public async Task RemoveCompanyUserNote(int JobNoteId)
        {
            var JobNote = await context.CompanyUserNote.Where(x => x.Id == JobNoteId).FirstOrDefaultAsync();

            context.CompanyUserNote.Remove(JobNote);
            await context.SaveChangesAsync();
        }

        public async Task DeleteNote(int NoteId)
        {
            var note = await context.CompanyUserNote.Where(x => x.Id == NoteId).FirstOrDefaultAsync();
            context.CompanyUserNote.Remove(note);
            await context.SaveChangesAsync();
        }


        /* Company User */

    
        public async Task<CompanyUser> GetCompanyUserByUser(string CurrentUser)
        {
            var companyUser = await context.CompanyUser
                   .Include(x => x.User)
                .Where(x => x.User.UserName == CurrentUser).FirstOrDefaultAsync();

            return companyUser;
        }
        public async Task<List<CompanyUser>> GetActiveStaffs(int CompanyId)
        {
            List<CompanyUser> CompanyUsers = await context.CompanyUser
                .Include(c => c.User)
                .Where(s => s.Status == Status.Active && s.CompanyId == CompanyId)
                .OrderBy(s => s.User.FullName)
                .ToListAsync();

            return CompanyUsers;
        }

        public async Task UpdateCompanyUser(CompanyUser CompanyUser, string CurrentUser)
        {
            context.ChangeTracker.Clear();
            var user = await context.User.Where(u => u.UserName == CurrentUser).FirstOrDefaultAsync();
            var ccu = await context.CompanyUser.Where(x => x.Id == CompanyUser.Id).FirstOrDefaultAsync();
            ccu.Status = CompanyUser.Status;
            ccu.CreateDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time");
            ccu.CompanyAdmin = CompanyUser.CompanyAdmin;
            context.Entry(ccu).State = EntityState.Modified;
            await context.SaveChangesAsync();

            var usr = await context.User.Where(u => u.Id == ccu.UserId).FirstOrDefaultAsync();
        }

        public async Task<int> CreateCompanyUser(int CompanyId, string UserId)
        {
            context.ChangeTracker.Clear();
            var user = await context.User.Where(u => u.Id == UserId).FirstOrDefaultAsync();
            var company = await context.Company.Where(c => c.Id == CompanyId).FirstOrDefaultAsync();
            var ccu = new CompanyUser();
            ccu.CreateDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time");
            ccu.CompanyId = CompanyId;
            ccu.UserId = UserId;
            ccu.Status = Status.Active;
            context.CompanyUser.Add(ccu);
            await context.SaveChangesAsync();

            return ccu.Id;
        }

        public async Task<int> AddStaffNote(string Note, NoteType NoteType, int CompanyUserId, string CurrentUser)
        {
            context.ChangeTracker.Clear();
            var user = await context.User.Where(u => u.UserName == CurrentUser).FirstOrDefaultAsync();
            var StaffNote = new StaffNote();
            StaffNote.CreatedBy = user.FullName;
            StaffNote.CreateDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time");
            StaffNote.Note = Note;
            StaffNote.CompanyUserId = CompanyUserId;
            StaffNote.NoteType = NoteType;
            await context.StaffNote.AddAsync(StaffNote);
            await context.SaveChangesAsync();
            return StaffNote.Id;
        }

        public async Task<List<StaffNote>> GetStaffNotes(int JobId)
        {
            var StaffNotes = await context.StaffNote
                .Where(x => x.CompanyUserId == JobId)
                .OrderBy(x => x.CreateDate)
                .ToListAsync();

            return StaffNotes;
        }

        public async Task RemoveStaffNote(int JobNoteId)
        {
            context.ChangeTracker.Clear();
            var JobNote = await context.StaffNote.Where(x => x.Id == JobNoteId).FirstOrDefaultAsync();

            context.StaffNote.Remove(JobNote);
            await context.SaveChangesAsync();
        }
    }
}
