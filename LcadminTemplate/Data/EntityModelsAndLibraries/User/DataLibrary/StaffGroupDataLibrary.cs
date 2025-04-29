using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class StaffGroupDataLibrary
    {
        public DataContext context { get; }

        public StaffGroupDataLibrary(DataContext Context)
        {
            context = Context;
        }

        public async Task<List<StaffGroup>> GetStaffGroups(int CompanyId)
        {
            List<StaffGroup> StaffGroups = await context.StaffGroup
                .Where(x => x.CompanyId == CompanyId && !x.Deleted)
                .Include(x => x.StaffGroupUsers)
                    .ThenInclude(sgu => sgu.CompanyUser)
                        .ThenInclude(ccu => ccu.User)
                .Include(x => x.StaffGroupDivisions)
           
                .ToListAsync();

            return StaffGroups;
        }

        public async Task<List<StaffGroupDivision>> GetStaffGroupUserDivision(int CompanyId)
        {
            List<StaffGroupDivision> sgd = await context.StaffGroupDivision
                .Include(s => s.StaffGroup)
               
                .Where(x => x.StaffGroup.CompanyId == CompanyId).ToListAsync();

            return sgd;
        }

        public async Task<List<StaffGroup>> GetActiveStaffGroups(int CompanyId)
        {
            List<StaffGroup> StaffGroups = await context.StaffGroup
                .Where(x => x.CompanyId == CompanyId && x.Status == GeneralEnums.Status.Active)
                .Include(x => x.StaffGroupUsers)
                    .ThenInclude(sgu => sgu.CompanyUser)
                        .ThenInclude(ccu => ccu.User)
                .ToListAsync();

            return StaffGroups;
        }

      

        public async Task<StaffGroup> GetStaffGroupByName(string StaffMember)
        {
            var StaffGroup = await context.StaffGroup
                  .Include(x => x.StaffGroupUsers)
                    .ThenInclude(sgu => sgu.CompanyUser)
                        .ThenInclude(ccu => ccu.User)
                  .Include(x => x.StaffGroupDivisions)
                .Where(x => x.Name == StaffMember && x.Status == GeneralEnums.Status.Active).FirstOrDefaultAsync();

            return StaffGroup;
        }

        public async Task<StaffGroup> GetStaffGroup(int StaffGroupId)
        {
            var StaffGroup = await context.StaffGroup
                  .Include(x => x.StaffGroupUsers)
                    .ThenInclude(sgu => sgu.CompanyUser)
                        .ThenInclude(ccu => ccu.User)
                  .Include(x => x.StaffGroupDivisions)
                
                .Where(x => x.Id == StaffGroupId && x.Status == GeneralEnums.Status.Active).FirstOrDefaultAsync();

            return StaffGroup;
        }

        public async Task CreateStaffGroup(StaffGroup StaffGroup, string CurrentUser)
        {
            context.ChangeTracker.Clear();
            var user = await context.User.Where(u => u.UserName == CurrentUser).FirstOrDefaultAsync();
            StaffGroup.UpdateDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time");
            StaffGroup.UpdatedBy = user.FullName;
            StaffGroup.CreateDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time");
            StaffGroup.CreatedBy = user.FullName;
            await context.StaffGroup.AddAsync(StaffGroup);
            await context.SaveChangesAsync();

        }

        public async Task UpdateStaffGroup(StaffGroup StaffGroup, string CurrentUser)
        {
            context.ChangeTracker.Clear();
            var user = await context.User.Where(u => u.UserName == CurrentUser).FirstOrDefaultAsync();
            var CurrentStaffGroup = await context.StaffGroup.Where(x => x.Id == StaffGroup.Id).FirstOrDefaultAsync();
            context.Entry(CurrentStaffGroup).State = EntityState.Detached;
            StaffGroup.UpdateDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time");
            StaffGroup.UpdatedBy = user.FullName;
            StaffGroup.CreateDate = CurrentStaffGroup.CreateDate;
            StaffGroup.CreatedBy = CurrentStaffGroup.CreatedBy;
            context.Entry(StaffGroup).State = EntityState.Modified;
            await context.SaveChangesAsync();

            if (StaffGroup.Status == GeneralEnums.Status.Inactive)
            {
                var staffgroupmembers = await context.StaffGroupUser.Where(x => x.StaffGroupId == StaffGroup.Id).ToListAsync();
                foreach (var sgm in staffgroupmembers)
                    context.StaffGroupUser.Remove(sgm);

                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteStaffGroup(int StaffGroupId, string CurrentUser)
        {
            context.ChangeTracker.Clear();
            var user = await context.User.Where(u => u.UserName == CurrentUser).FirstOrDefaultAsync();
            var StaffGroup = await context.StaffGroup.Where(s => s.Id == StaffGroupId).FirstOrDefaultAsync();
            StaffGroup.Deleted = true;

            var staffgroupmembers = await context.StaffGroupUser.Where(x => x.StaffGroupId == StaffGroup.Id).ToListAsync();
            foreach (var sgm in staffgroupmembers)
                context.StaffGroupUser.Remove(sgm);

            await context.SaveChangesAsync();

        }

        public async Task RemoveStaffGroup(int StaffGroupId, string CurrentUser)
        {
            context.ChangeTracker.Clear();
            var user = await context.User.Where(u => u.UserName == CurrentUser).FirstOrDefaultAsync();
            var StaffGroup = await context.StaffGroup.Where(s => s.Id == StaffGroupId).FirstOrDefaultAsync();
            StaffGroup.Status = GeneralEnums.Status.Inactive;
            context.Entry(StaffGroup).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task AddStaffMember(int StaffGroupId, int CompanyUserId, string CurrentUser)
        {
            context.ChangeTracker.Clear();
            if (StaffGroupId != 0)
            {
                var user = await context.User.Where(u => u.UserName == CurrentUser).FirstOrDefaultAsync();
                var sgu = new StaffGroupUser();
                sgu.CompanyUserId = CompanyUserId;
                sgu.StaffGroupId = StaffGroupId;
                sgu.UpdateDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time");
                sgu.UpdatedBy = user.FullName;
                sgu.CreateDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time");
                sgu.CreatedBy = user.FullName;
                await context.StaffGroupUser.AddAsync(sgu);
                await context.SaveChangesAsync();
            }
        }

        public async Task RemoveStaffMemer(int StaffGroupUserId)
        {
            context.ChangeTracker.Clear();
            var sgu = await context.StaffGroupUser.Where(x => x.Id == StaffGroupUserId).FirstOrDefaultAsync();
            context.StaffGroupUser.Remove(sgu);
            await context.SaveChangesAsync();
        }

        public async Task<List<CompanyUser>> GetAvailableStaff(int StaffGroupId)
        {
            var staffgroup = await GetStaffGroup(StaffGroupId);
            var staff = new List<CompanyUser>();
          
                staff = await context.CompanyUser
                    .Where(x => x.CompanyId == staffgroup.CompanyId)
                    .Include(x => x.User)
                    .ToListAsync();
            
            var availablestaff = new List<CompanyUser>();
            foreach (var user in staff)
            {
                if (!staffgroup.StaffGroupUsers.Where(x => x.CompanyUserId == user.Id).Any())
                    availablestaff.Add(user);
            }
            return availablestaff;
        }
        public async Task CreateStaffGroupDivision(int StaffGroupId, int DivisionId)
        {
            context.ChangeTracker.Clear();
            var x = new StaffGroupDivision();
            x.StaffGroupId = StaffGroupId;
        
            await context.StaffGroupDivision.AddAsync(x);
            await context.SaveChangesAsync();
        }

        public async Task RemoveStaffGroupDivision(int StaffGroupDivisionId)
        {
            context.ChangeTracker.Clear();
            var StaffGroupDivision = await context.StaffGroupDivision.Where(x => x.Id == StaffGroupDivisionId).FirstOrDefaultAsync();
            context.StaffGroupDivision.Remove(StaffGroupDivision);
            await context.SaveChangesAsync();
        }

        public async Task<List<CompanyUser>> GetAvailableStaffs(string StaffMember)
        {
            var staffgroup = await GetStaffGroupByName(StaffMember);
            var staff = new List<CompanyUser>();
            
                staff = await context.CompanyUser
                    .Where(x => x.CompanyId == staffgroup.CompanyId)
                    .Include(x => x.User)
                   .ToListAsync();
            
            var availablestaff = new List<CompanyUser>();
            foreach (var user in staff)
            {
                if (!staffgroup.StaffGroupUsers.Where(x => x.CompanyUserId == user.Id).Any())
                    availablestaff.Add(user);
            }
            return availablestaff;
        }
    }
}
