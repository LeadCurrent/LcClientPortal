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
        public async Task<List<Interest>> GetStudentInterestList()
        {
            return await context.Interests
                .ToListAsync();
        }

        public async Task<Interest> GetStudentInterest(int Id)
        {
            return await context.Interests
                .Where(x => x.Id == Id)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> DeleteStudentInterest(int id)
        {
            var Interest = await context.Interests
                .FirstOrDefaultAsync(x => x.Id == id);

            if (Interest == null)
                return false;

            context.Interests.Remove(Interest);
            var result = await context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> CreateStudentInterest(Interest Interest)
        {
            await context.Interests.AddAsync(Interest);
            var result = await context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> UpdateStudentInterest(Interest Interest)
        {
            if (Interest == null)
                return false;

            context.Interests.Update(Interest);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
