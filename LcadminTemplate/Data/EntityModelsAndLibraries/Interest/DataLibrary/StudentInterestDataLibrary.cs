using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class StudentInterestDataLibrary
    {
        public DataContext context { get; }

        public StudentInterestDataLibrary(DataContext Context)
        {
            context = Context;
        }
        public async Task<List<Interest>> GetStudentIntrestList()
        {
            return await context.Interests
                .ToListAsync();
        }
    }
}
