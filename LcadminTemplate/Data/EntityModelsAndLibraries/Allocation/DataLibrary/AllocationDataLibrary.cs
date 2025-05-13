using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class AllocationDataLibrary
    {
        public DataContext context { get; }

        public AllocationDataLibrary(DataContext Context)
        {
            context = Context;
        }

        public async Task<(int Active, int Inactive)> GetAllocationCountsBySourceId(int sourceId)
        {
            var result = await context.Allocations
                .Where(a => a.Sourceid == sourceId)
                .GroupBy(a => a.Active)
                .Select(g => new { Active = g.Key, Count = g.Count() })
                .ToListAsync();

            int active = result.FirstOrDefault(x => x.Active == true)?.Count ?? 0;
            int inactive = result.FirstOrDefault(x => x.Active == false)?.Count ?? 0;

            return (active, inactive);
        }
    }
}
