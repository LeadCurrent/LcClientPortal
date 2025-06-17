using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web;

namespace Data
{ 
    public class ProgramDataLibrary
    {
        public DataContext context { get; }

        public ProgramDataLibrary(DataContext Context)
        {
            context = Context;
        }

        public async Task<Program> GetProgramById(int Id)
        {
            return await context.Programs
                .Include(p => p.Programinterests)
                .Include(p => p.ProgramAreas)
                .Include(p => p.Degreeprograms)
                .Where(x => x.Id == Id)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> CreateProgram(Program Program)
        {
            await context.Programs.AddAsync(Program);
            var result = await context.SaveChangesAsync();

            int programId = Program.Id;

            foreach (var level in Program.Levels.Where(l => l.IsChecked))
            {
                context.Degreeprograms.Add(new Degreeprogram
                {
                    Programid = programId,
                    Levelid = level.Id
                });
            }

            foreach (var area in Program.Areas.Where(a => a.IsChecked))
            {
                context.Programareas.Add(new Programarea
                {
                    Programid = programId,
                    Areaid = area.Id
                });
            }

            foreach (var interest in Program.Interests.Where(i => i.IsChecked))
            {
                context.Programinterests.Add(new Programinterest
                {
                    Programid = programId,
                    Interestid = interest.Id
                });
            }

            var relatedResult = await context.SaveChangesAsync();
            return relatedResult > 0;
        }
        public async Task<bool> UpdateProgram(Program updatedProgram)
        {
            var existingProgram = await context.Programs
                .Include(p => p.Degreeprograms)
                .Include(p => p.ProgramAreas)
                .Include(p => p.Programinterests)
                .FirstOrDefaultAsync(p => p.Id == updatedProgram.Id);

            if (existingProgram == null)
                return false;

            // Update basic fields
            existingProgram.Name = updatedProgram.Name;
            existingProgram.Copy = updatedProgram.Copy;

            // Update Degree Levels
            var updatedLevelIds = updatedProgram.Levels.Where(l => l.IsChecked).Select(l => l.Id).ToList();

            // Remove unselected levels
            foreach (var item in existingProgram.Degreeprograms
                         .Where(d => !updatedLevelIds.Contains(d.Levelid)).ToList())
            {
                context.Degreeprograms.Remove(item);
            }

            // Add newly selected levels
            var existingLevelIds = existingProgram.Degreeprograms.Select(d => d.Levelid).ToList();
            var newLevelIds = updatedLevelIds.Except(existingLevelIds);

            foreach (var levelId in newLevelIds)
            {
                context.Degreeprograms.Add(new Degreeprogram
                {
                    Programid = existingProgram.Id,
                    Levelid = levelId
                });
            }

            // Update Areas
            var updatedAreaIds = updatedProgram.Areas.Where(a => a.IsChecked).Select(a => a.Id).ToList();

            foreach (var item in existingProgram.ProgramAreas
                         .Where(a => !updatedAreaIds.Contains(a.Areaid)).ToList())
            {
                context.Programareas.Remove(item);
            }

            var existingAreaIds = existingProgram.ProgramAreas.Select(a => a.Areaid).ToList();
            var newAreaIds = updatedAreaIds.Except(existingAreaIds);

            foreach (var areaId in newAreaIds)
            {
                context.Programareas.Add(new Programarea
                {
                    Programid = existingProgram.Id,
                    Areaid = areaId
                });
            }

            // Update Interests
            var updatedInterestIds = updatedProgram.Interests.Where(i => i.IsChecked).Select(i => i.Id).ToList();

            foreach (var item in existingProgram.Programinterests
                         .Where(i => !updatedInterestIds.Contains(i.Interestid)).ToList())
            {
                context.Programinterests.Remove(item);
            }

            var existingInterestIds = existingProgram.Programinterests.Select(i => i.Interestid).ToList();
            var newInterestIds = updatedInterestIds.Except(existingInterestIds);

            foreach (var interestId in newInterestIds)
            {
                context.Programinterests.Add(new Programinterest
                {
                    Programid = existingProgram.Id,
                    Interestid = interestId
                });
            }

            // Save all changes
            var result = await context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<List<Program>>                                                                                                                    GetPrograms()
        {
            return await context.Programs
                .ToListAsync();
        }

        public async Task<bool> DeleteProgram(int id)
        {
            var program = await context.Programs
                .Include(p => p.Programinterests)
                .Include(p => p.ProgramAreas)
                .Include(p => p.Degreeprograms)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (program == null)
                return false;

            // Step 1: Delete child records
            context.Programinterests.RemoveRange(program.Programinterests);
            context.Programareas.RemoveRange(program.ProgramAreas);
            context.Degreeprograms.RemoveRange(program.Degreeprograms);

            // Step 2: Delete the main Program
            context.Programs.Remove(program);

            // Step 3: Save changes
            var result = await context.SaveChangesAsync();
            return result > 0;
        }


    }
}
