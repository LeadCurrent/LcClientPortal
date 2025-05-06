using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Data.CompanyEnums;

namespace Data
{
    public  class CompanyIntegrationDataLibrary
    {
        public DataContext context { get; }

        public CompanyIntegrationDataLibrary(DataContext Context)
        {
            context = Context;
        }
        public async Task<List<CompanyEmailAccount>> GetCompanyEmailAccounts(int CompanyId)
        {
            return await context.CompanyEmailAccount
                .Where(u => u.CompanyId == CompanyId).ToListAsync();
        }

        public async Task<List<CompanyUserEmail>> GetCompanyUserEmailsByCompanyEmailAccountId(int CompanyEmailAccountId)
        {
            return await context.CompanyUserEmail
                                 .Include(x => x.CompanyEmailAccount)
                                 .Include(x => x.CompanyUser)
                                 .ThenInclude(x => x.User)
                                 .Where(e => e.CompanyEmailAccountId == CompanyEmailAccountId)
                                 .ToListAsync();
        } 
        public async Task DeleteCompanyUserEmailById(int Id)
        {
            var CompanyEmailAccount = await context.CompanyUserEmail.Where(s => s.Id == Id).FirstOrDefaultAsync();
            context.CompanyUserEmail.Remove(CompanyEmailAccount);
            await context.SaveChangesAsync();
            if (CompanyEmailAccount.IsDefault)
            {
                var NewDefaultAccount = await context.CompanyUserEmail.FirstOrDefaultAsync();
                if (NewDefaultAccount != null)
                {
                    NewDefaultAccount.IsDefault = true;
                    context.Entry(NewDefaultAccount).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                }

            }

        }

        public async Task<bool> AddCompanyUserEmail(int CompanyEmailAccountId, int CompanyUserId)
        {
            var CompanyUserEmailCount = context.CompanyUserEmail.Where(x => x.CompanyUserId == CompanyUserId).Count();


            var CompanyUserEmail = new CompanyUserEmail();
            CompanyUserEmail.CompanyUserId = CompanyUserId;
            CompanyUserEmail.CompanyEmailAccountId = CompanyEmailAccountId;
            if (CompanyUserEmailCount == 0)
            {
                CompanyUserEmail.IsDefault = true;
            }
            context.CompanyUserEmail.Add(CompanyUserEmail);
            var result = await context.SaveChangesAsync() > 0;
            return result;
        }  

        public async Task<CompanyEmailAccount> GetCompanyEmailAccount(int CompanyEmailAccountId)
        {
            return await context.CompanyEmailAccount
                .Where(u => u.Id == CompanyEmailAccountId).FirstOrDefaultAsync();
        }
        public async Task UpdateCompanyEmailAccount(CompanyEmailAccount CompanyEmailAccount)
        {
            context.ChangeTracker.Clear();
            var CT = context.CompanyEmailAccount.Where(x => x.Id == CompanyEmailAccount.Id).FirstOrDefault();
            CT.Name = CompanyEmailAccount.Name;
            CT.Email = CompanyEmailAccount.Email;
            CT.AllStaffAccess = CompanyEmailAccount.AllStaffAccess;
            CT.LastSyncDate = CompanyEmailAccount.LastSyncDate;
            CT.CalendarColor = CompanyEmailAccount.CalendarColor;
            context.Entry(CT).State = EntityState.Modified;
            await context.SaveChangesAsync();
        } 
        public async Task<bool> CreateCompanyEmailAccount(int CompanyId, int emailType, string Name, string Email, string token)
        {
            context.ChangeTracker.Clear();
            var CompanyEmailAccountCount = context.CompanyEmailAccount.Where(x => x.CompanyId == CompanyId).Count();

            EmailType etype = (EmailType)emailType;
            var CompanyEmailAccount = new CompanyEmailAccount
            {
                EmailType = etype,
                Email = Email,
                Name = Name,
                CompanyId = CompanyId,
                RefreshToken = token,
                IsDefault = false,
            };
            if (CompanyEmailAccountCount == 0)
            {
                CompanyEmailAccount.IsDefault = true;
            }
            context.CompanyEmailAccount.Add(CompanyEmailAccount);
            var success = await context.SaveChangesAsync() > 0;
            return success;
        }
        public async Task<bool> CheckCompanyAccount(int CompanyId, string Email)
        {
            var success = await context.CompanyEmailAccount
                                       .AnyAsync(x => x.CompanyId == CompanyId && x.Email == Email);

            return success;
        }
        public async Task UpdateRefreshtoken(int CompanyId, string Email, string RefreshToken)
        {
            context.ChangeTracker.Clear();
            var CompanyEmailAccount = await context.CompanyEmailAccount.Where(s => s.CompanyId == CompanyId && s.Email == Email).FirstOrDefaultAsync();
            CompanyEmailAccount.RefreshToken = RefreshToken;
            context.Entry(CompanyEmailAccount).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
        public async Task SetDefaultEmail(int EmailAccountId)
        {
            context.ChangeTracker.Clear();

            var CompanyEmailAccounts = await context.CompanyEmailAccount.Where(s => s.Id != EmailAccountId).ToListAsync();

            foreach (var emailaccount in CompanyEmailAccounts)
            {
                emailaccount.IsDefault = false;

                context.Entry(emailaccount).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }

            var CompanyEmailAccount = await context.CompanyEmailAccount.Where(s => s.Id == EmailAccountId).FirstOrDefaultAsync();
            CompanyEmailAccount.IsDefault = true;

            context.Entry(CompanyEmailAccount).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task DeleteEmailAccount(int EmailAccountId)
        {
            var CompanyEmailAccount = await context.CompanyEmailAccount.Where(s => s.Id == EmailAccountId).FirstOrDefaultAsync();
            var CompanyUserEmail = await context.CompanyUserEmail.Where(s => s.CompanyEmailAccountId == EmailAccountId).FirstOrDefaultAsync();
            if (CompanyUserEmail != null)
            {
                context.CompanyUserEmail.Remove(CompanyUserEmail);
                await context.SaveChangesAsync();
                if (CompanyUserEmail.IsDefault)
                {
                    var NewDefaultCompanyUserEmail = await context.CompanyUserEmail.FirstOrDefaultAsync();
                    if (NewDefaultCompanyUserEmail != null)
                    {
                        NewDefaultCompanyUserEmail.IsDefault = true;
                        context.Entry(NewDefaultCompanyUserEmail).State = EntityState.Modified;
                        await context.SaveChangesAsync();

                    }

                }
            }

            context.CompanyEmailAccount.Remove(CompanyEmailAccount);
            await context.SaveChangesAsync();
            if (CompanyEmailAccount.IsDefault)
            {
                var NewDefaultCompanyEmailAccount = await context.CompanyEmailAccount.FirstOrDefaultAsync();
                if (NewDefaultCompanyEmailAccount != null)
                {
                    NewDefaultCompanyEmailAccount.IsDefault = true;
                    context.Entry(NewDefaultCompanyEmailAccount).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                }

            }



        }
    }
}
