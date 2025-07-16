using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class VendorDashboardDataLibrary
    {
        public DataContext context { get; }
        public VendorDashboardDataLibrary(DataContext Context)
        {
            context = Context;
        }
        public async Task<List<VwVendorAllocation>> GetVendorAllocationsByAccessKey(string accessKey)
        {
          var vendorAllocation =  await context.VwVendorAllocations
                .Where(x => x.Accesskey == accessKey && x.AllocationActive == true)
                .OrderBy(x => x.Name)
                .ToListAsync();

            return vendorAllocation;
        }

        public async Task<VwVendorAllocation> GetVendorAllocationById(int allocationId)
        {
            var allocation = await context.VwVendorAllocations
                .FirstOrDefaultAsync(x => x.Id == allocationId && x.AllocationActive == true);

            return allocation;
        }

        public async Task<int> GetZipcodeCountAsync(int campusId)
        {
            return await context.Campuspostalcodes
                .Where(c => c.Campusid == campusId)
                .CountAsync();
        }

    }
}
