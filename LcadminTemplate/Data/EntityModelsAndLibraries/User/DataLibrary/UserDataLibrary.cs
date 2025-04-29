using Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Agreement;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Data.CompanyEnums;
using static Data.RoleEnums;

namespace Data
{
    public class UserDataLibrary
    {
        static readonly char[] padding = { '=' };
        public DataContext context { get; }

        public UserDataLibrary(DataContext Context)
        {
            context = Context;
        }

        public async Task<List<CompayUserRole>> GetCompanyUserRoles(int UserId)
        {
            var cur = await context.CompayUserRole
                .Include(r => r.Role)
                .Where(x => x.CompanyUserId == UserId).ToListAsync();

            return cur;
        }
        public async Task<User> GetUserByFullName(string fullName)
        {
            var users = await context.User.ToListAsync();
            return users.FirstOrDefault(u => (u.FirstName + " " + u.LastName) == fullName);
        }
        public async Task<User> GetUserWithCompany(string UserId)
        {
            return await context.User
                 .Include(x => x.CompanyUsers)
                .ThenInclude(x => x.Company)
                  .Include(x => x.CompanyUsers)
                .ThenInclude(x => x.CompanyUserRoles)
                .ThenInclude(x => x.Role)

                .Where(u => u.Id == UserId).FirstOrDefaultAsync();
        }

        public void ClearChangeTracker()
        {
            context.ChangeTracker.Clear();
        }

        public async Task<Role> GetRole(int roleId)
        {
            return await context.Role.Include(x => x.RolePermissions).Where(x => x.Id == roleId).FirstOrDefaultAsync();
        }

        public async Task<List<User>> GetUsers()
        {
            var Users = await context.User.Where(x => x.SystemAdmin || x.Developer).ToListAsync();
            if (Users != null)
            {
                foreach (var User in Users)
                {
                    var company = await context.Company.Where(x => x.Id == User.SelectedCompanyId).FirstOrDefaultAsync();
                    if (company != null)
                    {
                        User.CompanyName = company.Name;
                    }
                }
            }

            return Users;
        }
        public async Task SetUserLoginDetaills(User user, String device)
        {
            context.ChangeTracker.Clear();

            UserLoginHistory userLogin = new UserLoginHistory()
            {
                UserId = user.Id,
                LoginDateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time"),
                Device = device == "mobile-app" ? UserEnums.Device.MobileApp : UserEnums.Device.Browser,

            };

            context.UserLoginHistory.Add(userLogin);
            await context.SaveChangesAsync();

        }
        public async Task DeleteUser(int cuId, string CurrentUser)
        {
            context.ChangeTracker.Clear();
            var user = await context.User.FirstOrDefaultAsync(u => u.UserName == CurrentUser);
            var cu = await context.CompanyUser.FirstOrDefaultAsync(cu => cu.Id == cuId);

            cu.Status = GeneralEnums.Status.Inactive;

            context.Entry(cu).State = EntityState.Modified;
        }

        public async Task<string> GetUserIdByUserName(string UserName)
        {
            var user = await context.User.Where(u => u.UserName == UserName).FirstOrDefaultAsync();
            return user.Id;
        }

        public async Task<List<User>> GetUsers(int CompanyId)
        {
            var Users = await context.User.Where(x => x.SelectedCompanyId == CompanyId).ToListAsync();

            return Users;
        }

        public async Task<CompanyUser> GetCompanyUser(int Id)
        {
            return await context.CompanyUser
                .Include(x => x.User)
                .Where(x => x.Id == Id).FirstOrDefaultAsync();
        }


        public async Task<List<CompanyUser>> GetCompanyUsers(int CompanyId)
        {
            return await context.CompanyUser
                .Include(x => x.User)
                .Where(x => x.CompanyId == CompanyId).ToListAsync();
        }

        public async Task<CompanyUser> GetCompanyUserByUserName(string CurrentUser)
        {
            return await context.CompanyUser
                .Include(x => x.User)
                .Where(x => x.User.UserName == CurrentUser).FirstOrDefaultAsync();
        }

        public async Task<User> GetUser(string UserId)
        {
            return await context.User.Where(u => u.Id == UserId).FirstOrDefaultAsync();
        }
        public async Task<CompanyUser> GetCompanyUserByUserId(string UserId)
        {
            return await context.CompanyUser.Where(u => u.UserId == UserId).FirstOrDefaultAsync();
        }
        public async Task<List<User>> GetAdmins()
        {
            return await context.User.Where(x => x.Admin).ToListAsync();
        }



        //public async Task<List<Role>> GetAvailableRoleForDocument(int CompanyId)
        //{
        //    var FolderAccess = await context.FolderAccess
        //        .Include(x => x.Role)
        //        .Where(x => x.CompanyUser.CompanyId == CompanyId)
        //        .ToListAsync();

        //    var roles = await context.Role
        //        .Where(x => x.CompanyId == CompanyId)
        //        .ToListAsync();

        //    var assignedRoleIds = companyUserRoles.Select(x => x.Role.Id);

        //    return roles.Where(role => !assignedRoleIds.Contains(role.Id)).ToList();
        //}



        public async Task<User> GetCurrentUser(string UserName)
        {
            return await context.User
                .Where(u => u.UserName == UserName).FirstOrDefaultAsync();
        }
        public async Task<User> GetUserByPhone(string phone)
        {
            return await context.User
                .Where(u => u.Phone == phone).FirstOrDefaultAsync();
        }
        public async Task<User> GetUserByUserName(string UserName)
        {
            return await context.User

                .Where(u => u.UserName == UserName).FirstOrDefaultAsync();
        }
        public async Task<List<CompanyUser>> GetActiveUsersForCompany(int CompanyId)
        {
            var cu = await context.CompanyUser
                .Include(cu => cu.User)
                .Where(cu => cu.CompanyId == CompanyId && cu.Status == GeneralEnums.Status.Active)
                .ToListAsync();

            return cu;
        }
        public async Task<List<User>> GetSystemAdmins()
        {
            return await context.User
                .Where(u => u.SystemAdmin == true)
                .ToListAsync();
        }

        public async Task<User> GetCurrentUserById(string userId)
        {
            var user = await context.User
                .Where(u => u.Id == userId).FirstOrDefaultAsync();
            return user;
        }

        public async Task<bool> GetUserClaimAsync(string userId, string claimType, string claimValue)
        {
            var userClaimExists = await context.UserClaims
                .AnyAsync(uc => uc.UserId == userId && uc.ClaimType == claimType && uc.ClaimValue == claimValue);

            return userClaimExists;
        }

        public async Task AddUserClaimAsync(string userId, string claimType, string claimValue)
        {
            var userClaim = new IdentityUserClaim<string>
            {
                UserId = userId,
                ClaimType = claimType,
                ClaimValue = claimValue
            };

            await context.UserClaims.AddAsync(userClaim);
            await context.SaveChangesAsync();
        }

        public async Task RemoveUserClaimAsync(string userId, string claimType, string claimValue)
        {
            var userClaim = await context.UserClaims
                .FirstOrDefaultAsync(uc => uc.UserId == userId && uc.ClaimType == claimType && uc.ClaimValue == claimValue);

            if (userClaim != null)
            {
                context.UserClaims.Remove(userClaim);
                await context.SaveChangesAsync();
            }
        }





        public async Task<List<CompanyUser>> GetUsersForCompany(int CompanyId)
        {
            var ccus = await context.CompanyUser
                .Include(ccu => ccu.User)
                .Where(u => u.CompanyId == CompanyId && !u.Deleted)
                .ToListAsync();

            return ccus;
        }
        public async Task<User> GetUserByEmail(string Email)
        {
            return await context.User
                .Where(u => u.Email == Email).FirstOrDefaultAsync();
        }
        public async Task<List<CompanyPhoneNumber>> GetCompanyPhoneNumber(int CompanyId)
        {
            return await context.CompanyPhoneNumber
                .Where(u => u.CompanyId == CompanyId).ToListAsync();
        }


        public async Task UpdateCompanyEmailAccount(int CompanyEmailAccountId)
        {
            context.ChangeTracker.Clear();
            var CompanyEmailAccount = await context.CompanyEmailAccount
                .Where(u => u.Id == CompanyEmailAccountId).FirstOrDefaultAsync();
            CompanyEmailAccount.LastSyncDate = DateTime.UtcNow;
            context.Entry(CompanyEmailAccount).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
        public async Task<List<CompanyUserEmail>> GetCompanyUserEmails(int companyUserId)
        {
            return await context.CompanyUserEmail
                                 .Include(x => x.CompanyEmailAccount)
                                 .Where(e => e.CompanyUserId == companyUserId)
                                 .ToListAsync();
        }
        public async Task<List<CompanyUserPhoneNumber>> GetCompanyUserPhoneNumbers(int companyUserId)
        {
            return await context.CompanyUserPhoneNumber
                                 .Include(x => x.CompanyPhoneNumber)
                                 .Where(e => e.CompanyUserId == companyUserId)
                                 .ToListAsync();
        }
        public async Task<CompanyUserEmail> GetDefaultOrFirstCompanyUserEmail(int companyUserId)
        {
            var emailAccount = await context.CompanyUserEmail
                                 .Include(x => x.CompanyEmailAccount)
                                 .Where(x => x.CompanyUserId == companyUserId)
                                 .OrderByDescending(x => x.IsDefault)
                                 .ThenBy(x => x.Id)
                                 .FirstOrDefaultAsync();

            return emailAccount;

        }

        public async Task SetTemporaryPassword(string UserId, Boolean TempPassword)
        {
            var user = await context.User.Where(u => u.Id == UserId).FirstOrDefaultAsync();
            user.TemporaryPassword = TempPassword;
            await UpdateUser(user);
        }


        public async Task<User> GetUserbyUserName(string UserName)
        {
            return await context.User.Where(u => u.UserName == UserName).FirstOrDefaultAsync();
        }

        public async Task UpdateUser(User User)
        {
            context.ChangeTracker.Clear();
            User.UpdateDate = DateTime.Now;
            context.User.Attach(User);
            context.Entry(User).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task CreateUser(User user)
        {
            context.ChangeTracker.Clear();
            user.UpdateDate = DateTime.Now;
            await context.User.AddAsync(user);
            await context.SaveChangesAsync();
        }


        public async Task DeleteUserEmail(int EmailAccountId)
        {
            var CompanyEmailAccount = await context.CompanyUserEmail.Where(s => s.CompanyEmailAccountId == EmailAccountId).FirstOrDefaultAsync();
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
        public async Task DeleteUserPhoneNumber(int PhoneNumberId)
        {
            var CompanyUserPhoneNumber = await context.CompanyUserPhoneNumber.Where(s => s.CompanyPhoneNumberId == PhoneNumberId).FirstOrDefaultAsync();
            context.CompanyUserPhoneNumber.Remove(CompanyUserPhoneNumber);
            await context.SaveChangesAsync();
            if (CompanyUserPhoneNumber.IsDefault)
            {
                var NewDefaultCompanyUserPhoneNumber = await context.CompanyUserPhoneNumber.FirstOrDefaultAsync();
                if (NewDefaultCompanyUserPhoneNumber != null)
                {
                    NewDefaultCompanyUserPhoneNumber.IsDefault = true;
                    context.Entry(NewDefaultCompanyUserPhoneNumber).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                }

            }

        }
        public async Task SetDefaultEmail(int EmailAccountId)
        {
            context.ChangeTracker.Clear();

            var CompanyUserEmails = await context.CompanyUserEmail.Where(s => s.CompanyEmailAccountId != EmailAccountId).ToListAsync();

            foreach (var emailaccount in CompanyUserEmails)
            {
                emailaccount.IsDefault = false;

                context.Entry(emailaccount).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }

            var CompanyUserEmail = await context.CompanyUserEmail.Where(s => s.CompanyEmailAccountId == EmailAccountId).FirstOrDefaultAsync();
            CompanyUserEmail.IsDefault = true;

            context.Entry(CompanyUserEmail).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
        public async Task SetDefaultUserPhoneNumber(int PhoneNumberId)
        {
            context.ChangeTracker.Clear();

            var CompanyUserPhoneNumbers = await context.CompanyUserPhoneNumber.Where(s => s.CompanyPhoneNumberId != PhoneNumberId).ToListAsync();

            foreach (var phoneNumber in CompanyUserPhoneNumbers)
            {
                phoneNumber.IsDefault = false;

                context.Entry(phoneNumber).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }

            var CompanyUserPhoneNumber = await context.CompanyUserPhoneNumber.Where(s => s.CompanyPhoneNumberId == PhoneNumberId).FirstOrDefaultAsync();
            CompanyUserPhoneNumber.IsDefault = true;

            context.Entry(CompanyUserPhoneNumber).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
        public async Task<bool> CreateCompanyEmailAccountAndCompanyUserEmail(int CompanyId, int emailType, string Name, string Email, string token, int CompanyUserId, DateTime LastSyncDate)
        {
            context.ChangeTracker.Clear();

            var CompanyEmailAccountCount = context.CompanyEmailAccount.Where(x => x.CompanyId == CompanyId).Count();
            var CompanyUserEmailCount = context.CompanyUserEmail.Where(x => x.CompanyUserId == CompanyUserId).Count();


            EmailType etype = (EmailType)emailType;
            var CompanyEmailAccount = new CompanyEmailAccount
            {
                EmailType = etype,
                Email = Email,
                Name = Name,
                CompanyId = CompanyId,
                RefreshToken = token,
                IsDefault = false,
                LastSyncDate = LastSyncDate
            };
            if (CompanyEmailAccountCount == 0)
            {
                CompanyEmailAccount.IsDefault = true;
            }

            context.CompanyEmailAccount.Add(CompanyEmailAccount);
            var success = await context.SaveChangesAsync() > 0;

            if (success)
            {
                var CompanyUserEmail = new CompanyUserEmail();
                CompanyUserEmail.CompanyUserId = CompanyUserId;
                CompanyUserEmail.CompanyEmailAccountId = CompanyEmailAccount.Id;
                if (CompanyUserEmailCount == 0)
                {
                    CompanyUserEmail.IsDefault = true;
                }
                context.CompanyUserEmail.Add(CompanyUserEmail);
            }


            var result = await context.SaveChangesAsync() > 0;
            return result;
        }
        public async Task<bool> CreateCompanyPhoneNoAndCompanyUserPhoneNo(int CompanyId, string Name, int CompanyUserId, string PhoneNumber)
        {
            context.ChangeTracker.Clear();

            var CompanyPhoneNumberCount = context.CompanyPhoneNumber.Where(x => x.CompanyId == CompanyId).Count();
            var CompanyUserPhoneNumbersCount = context.CompanyUserPhoneNumber.Where(x => x.CompanyUserId == CompanyUserId).Count();


            var CompanyPhoneNumber = new CompanyPhoneNumber
            {
                Name = Name,
                PhoneNumber = PhoneNumber,
                CompanyId = CompanyId
            };
            if (CompanyPhoneNumberCount == 0)
            {
                CompanyPhoneNumber.IsDefault = true;
            }

            context.CompanyPhoneNumber.Add(CompanyPhoneNumber);
            var success = await context.SaveChangesAsync() > 0;

            if (success)
            {
                var CompanyUserPhoneNumber = new CompanyUserPhoneNumber();
                CompanyUserPhoneNumber.CompanyUserId = CompanyUserId;
                CompanyUserPhoneNumber.CompanyPhoneNumberId = CompanyPhoneNumber.Id;
                if (CompanyUserPhoneNumbersCount == 0)
                {
                    CompanyUserPhoneNumber.IsDefault = true;
                }
                context.CompanyUserPhoneNumber.Add(CompanyUserPhoneNumber);
            }


            var result = await context.SaveChangesAsync() > 0;
            return result;
        }

        public async Task<bool> AddUserEmail(int CompanyEmailAccountId, int CompanyUserId)
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
        public async Task<bool> AddUserCompanyPhoneNumber(int CompanyPhoneNumberId, int CompanyUserId)
        {
            var CompanyUserPhoneCount = context.CompanyUserPhoneNumber.Where(x => x.CompanyUserId == CompanyUserId).Count();


            var CompanyUserPhoneNumber = new CompanyUserPhoneNumber();
            CompanyUserPhoneNumber.CompanyUserId = CompanyUserId;
            CompanyUserPhoneNumber.CompanyPhoneNumberId = CompanyPhoneNumberId;
            if (CompanyUserPhoneCount == 0)
            {
                CompanyUserPhoneNumber.IsDefault = true;
            }
            context.CompanyUserPhoneNumber.Add(CompanyUserPhoneNumber);
            var result = await context.SaveChangesAsync() > 0;
            return result;
        }

        public async Task UpdateRefreshtokenAndCreateCompanyUserEmail(int CompanyId, string Email, string RefreshToken, int CompanyUserId)
        {

            context.ChangeTracker.Clear();

            var CompanyUserEmailCount = context.CompanyUserEmail.Where(x => x.CompanyUserId == CompanyUserId).Count();

            var CompanyEmailAccount = await context.CompanyEmailAccount.Where(s => s.CompanyId == CompanyId && s.Email == Email).FirstOrDefaultAsync();
            CompanyEmailAccount.RefreshToken = RefreshToken;
            context.Entry(CompanyEmailAccount).State = EntityState.Modified;
            var success = await context.SaveChangesAsync() > 0;
            if (success)
            {
                var CompanyUserEmail = new CompanyUserEmail();
                CompanyUserEmail.CompanyUserId = CompanyUserId;
                CompanyUserEmail.CompanyEmailAccountId = CompanyEmailAccount.Id;
                if (CompanyUserEmailCount == 0)
                {
                    CompanyUserEmail.IsDefault = true;
                }
                context.CompanyUserEmail.Add(CompanyUserEmail);
                await context.SaveChangesAsync();
            }
        }

        #region Mobile
        public async Task<User> GetUserFromAuthorizationCode(string AuthorizationCode)
        {
            return await context.User
                .Where(u => u.MobileAuthorizationCode == AuthorizationCode).FirstOrDefaultAsync();
        }

        public async Task<User> SetAuthorizationCode(string username)
        {
            context.ChangeTracker.Clear();
            var user = await context.User
                .Where(u => u.UserName == username).FirstOrDefaultAsync();

            if (user.MobileAuthorizationCode == null)
            {
                user.MobileAuthorizationCode = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).TrimEnd(padding).Replace('+', '-').Replace('/', '_').Substring(0, 10);
                context.Entry(user).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
            return user;

        }

        public async Task<string> SetForgotPasswordCode(string phone)
        {
            context.ChangeTracker.Clear();
            var user = await context.User
                .Where(x => x.PhoneNumber == phone)
                .FirstOrDefaultAsync();

            if (user != null)
            {
                //if (student.MobileCode == null)
                //{
                string _numbers = "0123456789";
                Random random = new Random();
                StringBuilder builder = new StringBuilder(6);
                for (int i = 0; i < 6; i++)
                    builder.Append(_numbers[random.Next(0, _numbers.Length)]);

                var MobileCode = builder.ToString();
                var check = context.User.Where(x => x.ForgotPasswordCode == MobileCode).FirstOrDefault();
                if (check != null)
                    return await SetForgotPasswordCode(phone);
                else
                    user.ForgotPasswordCode = builder.ToString();

                context.Entry(user).State = EntityState.Modified;
                context.SaveChanges();
                return user.ForgotPasswordCode;
            }


            return "Invalid";


        }

        #endregion

        public async Task<int> HasAnyUsersAsync()
        {
            bool hasUsers = await context.Users.AnyAsync();
            return hasUsers ? 1 : 0;
        }

        public async Task<int> GetUserCountAsync()
        {
            return await context.Users.CountAsync();
        }

    }
}
