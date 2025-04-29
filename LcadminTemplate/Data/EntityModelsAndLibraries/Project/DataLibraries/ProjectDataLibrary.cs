//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Data
//{
//    public class ProjectDataLibrary
//    {
//        public DataContext context { get; }

//        public ProjectDataLibrary(DataContext Context)
//        {
//            context = Context;
//        }

//        public async Task<List<Project>> GetProjects()
//        {
//            List<Project> Projects = await context.Project
//                .ToListAsync();

//            return Projects;
//        }

//        public async Task<List<Project>> GetActiveProjects()
//        {
//            List<Project> Projects = await context.Project
//                .Where(x => x.Status == GeneralEnums.Status.Active)
//                .ToListAsync();

//            return Projects;
//        }

//        public async Task<Project> GetProject(int ProjectId)
//        {
//            var Project = await context.Project
//                 .Where(x => x.Id == ProjectId).FirstOrDefaultAsync();

//            return Project;
//        }

//        public async Task<Project> CreateProject(Project Project, string CurrentUser)
//        {
//            context.ChangeTracker.Clear();
//            var user = await context.User.Where(u => u.UserName == CurrentUser).FirstOrDefaultAsync();
//            Project.UpdateDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time");
//            Project.UpdatedBy = user.FullName;
//            Project.CreateDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time");
//            Project.CreatedBy = user.FullName;
//            context.Project.Add(Project);
//            await context.SaveChangesAsync();
//            return Project;
//        }

//        public async Task UpdateProject(Project Project, string CurrentUser)
//        {
//            context.ChangeTracker.Clear();
//            var user = await context.User.Where(u => u.UserName == CurrentUser).FirstOrDefaultAsync();
//            var CurrentProject = await context.Project.Where(s => s.Id == Project.Id).FirstOrDefaultAsync();
//            context.Entry(CurrentProject).State = EntityState.Detached;
//            Project.UpdateDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time");
//            Project.UpdatedBy = user.FullName;
//            Project.CreateDate = CurrentProject.CreateDate;
//            Project.CreatedBy = CurrentProject.CreatedBy;
//            context.Entry(Project).State = EntityState.Modified;
//            await context.SaveChangesAsync();
//        }

//        public async Task DeleteProject(int ProjectId)
//        {
//            context.ChangeTracker.Clear();
//            var Project = await context.Project.Where(s => s.Id == ProjectId).FirstOrDefaultAsync();
//            context.Project.Remove(Project);
//            context.SaveChanges();
//        }
//    }
//}
