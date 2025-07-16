using Google.Apis.Util;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
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
                //.Include(x => x.Client)
                //.Include(x => x.School)
                //.Include(x => x.PortalStates)
                .Where(x => x.Id == campusId)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Campus>> GetCampusBYSchooldId(int schoolId, string[] campusIds = null)
        {
            var campuses = new List<Campus>();
            try
            {
                var query = context.Campuses
                    .Include(c => c.PortalStates)
                    .Include(c => c.Postalcode)
                    .Where(c => c.Schoolid == schoolId);

                if (campusIds != null && campusIds.Length > 0)
                {
                    var idList = campusIds.Select(id => int.Parse(id)).ToList();
                    query = query.Where(c => idList.Contains(c.Id));
                }

                campuses = await query
                    .OrderBy(c => c.Name)
                    .ToListAsync();

                // Post-process ZipCount and readable CampusType
                foreach (var campus in campuses)
                {
                    campus.ZipCount = GetZipcodeCount(campus.Id);

                    // Optional: Convert campustype codes to human-readable format
                    campus.Campustype = campus.Campustype?.ToUpper() switch
                    {
                        "O" => "Online",
                        "C" => "Campus",
                        _ => "Mixed"
                    };
                }

                return campuses;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return campuses;
            }
        }

        public int GetZipcodeCount(int campusid)
        {
            return context.Campuspostalcodes.Count(x => x.Campusid == campusid);
        }


        public async Task<Client> GetClientByCampusId(int Id)
        {
            return await context.Clients
                .Where(x => x.Id == Id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Campus>> GetInactiveCampuses()
        {
            return await context.Campuses
                .Include(x => x.School)
                .Where(x => !x.Active)
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

        public async Task<bool> UpdateCampusesStatus(List<Campus> campusesToUpdate)
        {
            try
            {
                var campusIds = campusesToUpdate.Select(c => c.Id).ToList();
                var existingCampuses = await context.Campuses
                    .Where(c => campusIds.Contains(c.Id))
                    .ToListAsync();

                foreach (var dbCampus in existingCampuses)
                {
                    var updatedCampus = campusesToUpdate.FirstOrDefault(c => c.Id == dbCampus.Id);
                    if (updatedCampus != null)
                    {
                        dbCampus.Active = updatedCampus.Active;
                    }
                }

                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // log ex if needed
                return false;
            }
        }

        public async Task<bool> DeleteCampuses(List<Campus> campusesToDelete)
        {
            try
            {
                var campusIds = campusesToDelete.Select(c => c.Id).ToList();
                var campuses = await context.Campuses
                    .Where(c => campusIds.Contains(c.Id))
                    .ToListAsync();

                if (campuses.Any())
                {
                    context.Campuses.RemoveRange(campuses);
                    await context.SaveChangesAsync();
                }

                return true;
            }
            catch (Exception ex)
            {
                // Log ex if needed
                return false;
            }
        }

    }
}
