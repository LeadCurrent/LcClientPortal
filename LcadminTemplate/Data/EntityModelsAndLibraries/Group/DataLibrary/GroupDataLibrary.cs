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
    public class GroupDataLibrary
    {
        public DataContext context { get; }

        public GroupDataLibrary(DataContext Context)
        {
            context = Context;
        }

        public async Task<List<Group>> GroupByCompanyId(int? companyId)
        {
            if (companyId <= 0)
                return new List<Group>();

            return await context.Groups
                .Where(x => x.CompanyId == companyId)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<Group> GetGroups(int GroupID)
        {
            return await context.Groups
                .Where(x => x.Id == GroupID)
                .FirstOrDefaultAsync();
        }

        public async Task<int> CreateGroups(Group Groups)
        {
            context.ChangeTracker.Clear();
            context.Groups.Add(Groups);
            await context.SaveChangesAsync();
            return Groups.Id;
        }

        public async Task<bool> UpdateGroups(Group updatedGroups)
        {
            var existingScholl = await context.Groups.FindAsync(updatedGroups.Id);

            if (existingScholl != null)
            {
                existingScholl.Name = updatedGroups.Name;
                existingScholl.Copy = updatedGroups.Copy;
                              


                 await context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
