using Google.Apis.Util;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class CampusDataLibrary
    {
        public DataContext context { get; }

        public CampusDataLibrary(DataContext Context)
        {
            context = Context;
        }

        public async Task<List<Campus>> CampusByCompanyId(int companyId)
        {
            if (companyId <= 0)
                return new List<Campus>();

            return await context.Campuses
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<Campus> GetCampus(int campusId)
        {
            return await context.Campuses
                .Where(x => x.Id == campusId)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Campus>> GetCampusBYSchooldId(int schoolsId)
        {
            return await context.Campuses
                .Where(x => x.Schoolid == schoolsId)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }


        public async Task<List<Campus>> GetCampusWithZIPCountBySchoolId(int schoolsId)
        {
            var campuses = await context.Campuses
                .Where(x => x.Schoolid == schoolsId)
                .Include(x => x.PortalStates)
                .Include(x => x.Postalcode)
                .OrderBy(x => x.Name)
                .ToListAsync();

            // Calculate ZipCount for each campus
            foreach (var campus in campuses)
            {
                campus.ZipCount = await context.Campuspostalcodes.CountAsync(z => z.Campusid == campus.Id);
            }

            return campuses;
        }


        public async Task<int> CreateCampus(Campus campus)
        {
            context.ChangeTracker.Clear();
            context.Campuses.Add(campus);
            await context.SaveChangesAsync();
            return campus.Id;
        }

        public async Task<bool> UpdateSchool(Campus updatedcampus)
        {
            var existingScholl = await context.Campuses.FindAsync(updatedcampus.Id);

            if (existingScholl != null)
            {
                existingScholl.Name = updatedcampus.Name;
                existingScholl.Address = updatedcampus.Address;
                existingScholl.City = updatedcampus.City;
                existingScholl.Postalcode = updatedcampus.Postalcode;
                existingScholl.Active = updatedcampus.Active;
                existingScholl.Campustype = updatedcampus.Campustype;
                existingScholl.Copy = updatedcampus.Copy;
                existingScholl.Clientid = updatedcampus.Clientid;
                existingScholl.PortalStatesid = updatedcampus.PortalStatesid;
                
               

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
