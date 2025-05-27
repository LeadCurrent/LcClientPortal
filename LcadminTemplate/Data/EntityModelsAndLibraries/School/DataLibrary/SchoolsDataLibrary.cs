using DocumentFormat.OpenXml.Spreadsheet;
using Google.Apis.Util;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class SchoolsDataLibrary
    {
        public DataContext context { get; }

        public SchoolsDataLibrary(DataContext Context)
        {
            context = Context;
        }

        public async Task<List<Scholls>> SchoolsByCompanyId(int? companyId)
        {
            if (companyId <= 0)
                return new List<Scholls>();

            return await context.Schools
                .Where(x => x.CompanyId == companyId)
                .Include(s => s.Schoolgroups)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<Scholls> GetSchool(int SchollsId)
        {
            return await context.Schools
                .Where(x => x.Id == SchollsId)
                .Include(s => s.Schoolgroups)
                .ThenInclude(s => s.Group)
                .FirstOrDefaultAsync();
        }

        public async Task<int> CreateSchool(Scholls scholls)
        {
            context.ChangeTracker.Clear();
            context.Schools.Add(scholls);
            await context.SaveChangesAsync();
            return scholls.Id;
        }

        public async Task<bool> UpdateSchool(Scholls updatedScholls)
        {
            var existingScholl = await context.Schools.FindAsync(updatedScholls.Id);

            if (existingScholl != null)
            {
                existingScholl.Name = updatedScholls.Name;
                existingScholl.Abbr = updatedScholls.Abbr;
                existingScholl.Website = updatedScholls.Website;
                existingScholl.Logo100 = updatedScholls.Logo100;
                existingScholl.Minage = updatedScholls.Minage;
                existingScholl.Maxage = updatedScholls.Maxage;
                existingScholl.Minhs = updatedScholls.Minhs;
                existingScholl.Maxhs = updatedScholls.Maxhs;
                existingScholl.Notes = updatedScholls.Notes;
                existingScholl.Shortcopy = updatedScholls.Shortcopy;
                existingScholl.Targeting = updatedScholls.Targeting;
                existingScholl.Accreditation = updatedScholls.Accreditation;
                existingScholl.Highlights = updatedScholls.Highlights;
                existingScholl.Alert = updatedScholls.Alert;
                existingScholl.Startdate = updatedScholls.Startdate;
                existingScholl.Scoreadjustment = updatedScholls.Scoreadjustment;
                existingScholl.Militaryfriendly = updatedScholls.Militaryfriendly;
                existingScholl.Disclosure = updatedScholls.Disclosure;
                existingScholl.Schoolgroup = updatedScholls.Schoolgroup;
                existingScholl.TcpaText = updatedScholls.TcpaText;
               

                await context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task UpdateSchoolGroups(int schoolId, List<Group> groupIds, int? oldSchoolId , int? companyId)
        {
          
            // Remove existing mappings
            var existing = context.Schoolgroups.Where(sg => sg.Schoolid == schoolId);
            context.Schoolgroups.RemoveRange(existing);

            // Insert only checked groups
            foreach (var group in groupIds.Where(g => g.IsChecked))
            {
                context.Schoolgroups.Add(new Schoolgroup
                {
                    Schoolid = schoolId,
                    Groupid = group.Id,
                    oldId = oldSchoolId,
                    CompanyId = companyId
                });
            }

            await context.SaveChangesAsync();
        }

    }
}
