using Google.Apis.Util;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class SourcesDataLibrary
    {
        public DataContext context { get; }
        public SourcesDataLibrary(DataContext Context)
        {
            context = Context;
        }
        public async Task<List<Source>> GetSourcesByCompanyId(int companyId)
        {
            if (companyId <= 0)
                return new List<Source>();

            return await context.Sources
                .Where(x => x.CompanyId == companyId)
                .OrderByDescending(x => x.Active)
                .ThenBy(x => x.Name)
                .ToListAsync();
        }
        public async Task<Source> GetSource(int sourceId)
        {
            return await context.Sources
                .Where(x => x.Id == sourceId)
                .FirstOrDefaultAsync();
        }
        public async Task<int> CreateSource(Source Source)
        {
            context.ChangeTracker.Clear();
            context.Sources.Add(Source);
            await context.SaveChangesAsync();
            return Source.Id;
        }
        public async Task<bool> UpdateSource(Source updatedSource)
        {
            var existingSource = await context.Sources.FindAsync(updatedSource.Id);

            if (existingSource != null)
            {
                existingSource.Name = updatedSource.Name;
                existingSource.Apikey = updatedSource.Apikey;
                existingSource.Lcsourceid = updatedSource.Lcsourceid;
                existingSource.Lcsiteid = updatedSource.Lcsiteid;
                existingSource.Accesskey = updatedSource.Accesskey;
                existingSource.Active = updatedSource.Active;

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
