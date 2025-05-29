using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class AreaOfInterestDataLibrary
    {
        public DataContext context { get; }

        public AreaOfInterestDataLibrary(DataContext Context)
        {
            context = Context;
        }

        public async Task<Area> GetAreaOfIntrest(int Id)
        {
            return await context.Areas
                .Where(x => x.Id == Id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Area>> GetAreaOfIntrestList()
        {
            return await context.Areas
                .ToListAsync();
        }


        public async Task<bool> DeleteAreaOfIntrest(int id)
        {
            var level = await context.Areas
                .FirstOrDefaultAsync(x => x.Id == id);

            if (level == null)
                return false;

            context.Areas.Remove(level);
            var result = await context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> CreateAreaOfIntrest(Area Area)
        {
            await context.Areas.AddAsync(Area);
            var result = await context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> UpdateAreaOfIntrest(Area Area)
        {
            if (Area == null)
                return false;

            context.Areas.Update(Area);
            await context.SaveChangesAsync();
            return true;
        }

    }
}
