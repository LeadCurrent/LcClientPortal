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

        public async Task<List<Group>> GetGroups()
        {
            return await context.Groups
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<Group> GetGroupById(int GroupID)
        {
            return await context.Groups
                .Where(x => x.Id == GroupID)
                .FirstOrDefaultAsync();
        }

        public async Task<int> CreateGroups(Group Group)
        {
            context.ChangeTracker.Clear();
            context.Groups.Add(Group);
            await context.SaveChangesAsync();
            return Group.Id;
        }

        public async Task<bool> UpdateGroup(Group updatedGroups)
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

        public async Task<bool> DeleteGroupById(int groupId)
        {
            var group = await context.Groups.FindAsync(groupId);
            if (group == null)
                return false;

            context.Groups.Remove(group);
            await context.SaveChangesAsync();
            return true;
        }

    }
}
