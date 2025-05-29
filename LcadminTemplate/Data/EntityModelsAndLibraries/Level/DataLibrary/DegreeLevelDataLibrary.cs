using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class DegreeLevelDataLibrary
    {
        public DataContext context { get; }

        public DegreeLevelDataLibrary(DataContext Context)
        {
            context = Context;
        }

        public async Task<Level> GetDegreeLevel(int Id)
        {
            return await context.Levels
                .Where(x => x.Id == Id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Level>> GetDegreeLevels(int CompanyId)
        {
            return await context.Levels
                .Where(x => x.CompanyId == CompanyId)
                .ToListAsync();
        }


        public async Task<bool> DeleteDegreeLevel(int id)
        {
            var level = await context.Levels
                .FirstOrDefaultAsync(x => x.Id == id);

            if (level == null)
                return false;

            context.Levels.Remove(level);
            var result = await context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> CreateDegreeLevel(Level level)
        {
            await context.Levels.AddAsync(level);
            var result = await context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> UpdateDegreeLevel(Level level)
        {
            if (level == null)
                return false;

            context.Levels.Update(level);
            await context.SaveChangesAsync();
            return true;
        }

    }
}
