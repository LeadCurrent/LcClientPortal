using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Data.GeneralEnums;
namespace Data
{
    public class TemplateDataLibrary
    {
        public DataContext context { get; }

        public TemplateDataLibrary(DataContext Context)
        {
            context = Context;
        }

        public async Task<List<Template>> GetTemplates()
        {
            List<Template> Templates = await context.Template
                .ToListAsync();

            return Templates;
        }

        public async Task<List<Template>> GetActiveTemplates()
        {
            List<Template> Templates = await context.Template
                .Where(x => x.Status == Status.Active)
                .ToListAsync();

            return Templates;
        }

        public async Task<Template> GetTemplate(int TemplateId)
        {
            var Template = await context.Template
                 .Where(x => x.Id == TemplateId).FirstOrDefaultAsync();

            return Template;
        }

        public async Task<Template> CreateTemplate(Template Template, string CurrentUser)
        {
            context.ChangeTracker.Clear();
            var user = await context.User.Where(u => u.UserName == CurrentUser).FirstOrDefaultAsync();
            Template.UpdateDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time");
            Template.UpdatedBy = user.FullName;
            Template.CreateDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time");
            Template.CreatedBy = user.FullName;
            Template.Hour = Template.Time.Hour;
            Template.Minute = Template.Time.Minute;
            context.Template.Add(Template);
            await context.SaveChangesAsync();
            return Template;
        }
        public async Task<Template> GetTemplateByTemplateName(string TemplateName)
        {
            var Template = await context.Template
                 .Where(x => x.Name == TemplateName).FirstOrDefaultAsync();

            return Template;
        }
        public async Task UpdateTemplate(Template Template, string CurrentUser)
        {
            context.ChangeTracker.Clear();
            var user = await context.User.Where(u => u.UserName == CurrentUser).FirstOrDefaultAsync();
            var CurrentTemplate = await context.Template.Where(s => s.Id == Template.Id).FirstOrDefaultAsync();
            context.Entry(CurrentTemplate).State = EntityState.Detached;
            Template.UpdateDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time");
            Template.UpdatedBy = user.FullName;
            Template.CreateDate = CurrentTemplate.CreateDate;
            Template.CreatedBy = CurrentTemplate.CreatedBy;
            Template.Hour = Template.Time.Hour;
            Template.Minute = Template.Time.Minute;
            context.Entry(Template).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }


        public async Task<TemplateMultiSelect> CreateOrUpdateTemplateMultiSelect(int TemplateId, TemplateEnums.SampleDropdown sampleDropdownId)
        {
            context.ChangeTracker.Clear();
            var existingRecord = await context.TemplateMultiSelect.FirstOrDefaultAsync(t => t.TemplateId == TemplateId && t.SampleDropdown == (int)(TemplateEnums.SampleDropdown)sampleDropdownId);
            if (existingRecord != null)
            {
                existingRecord.TemplateId = TemplateId;
                existingRecord.SampleDropdown = (int)(TemplateEnums.SampleDropdown)sampleDropdownId;
                await context.SaveChangesAsync();
                return existingRecord;
            }
            else
            {
                var templateMultiSelect = new TemplateMultiSelect();
                templateMultiSelect.TemplateId = TemplateId;
                templateMultiSelect.SampleDropdown = (int)(TemplateEnums.SampleDropdown)sampleDropdownId;
                context.TemplateMultiSelect.Add(templateMultiSelect);
                await context.SaveChangesAsync();
                return templateMultiSelect;
            }
        }

        public async Task DeleteTemplateMultiSelect(int TemplateId)
        {
            context.ChangeTracker.Clear();
            var TemplateMultiSelects = await context.TemplateMultiSelect.Where(s => s.TemplateId == TemplateId).ToListAsync();

            if (TemplateMultiSelects.Count() > 0)
            {
                foreach (var TemplateMultiSelect in TemplateMultiSelects)
                {
                    context.TemplateMultiSelect.Remove(TemplateMultiSelect);
                    await context.SaveChangesAsync();
                }
            }
        }
        public async Task<List<TemplateMultiSelect>> GetMutiSelectedTemplates(int TemplateID)
        {
            List<TemplateMultiSelect> TemplateMultiSelects = await context.TemplateMultiSelect.Where(x => x.TemplateId == TemplateID)
                .ToListAsync();
            return TemplateMultiSelects;
        }

        public async Task DeleteTemplate(int TemplateId)
        {
            context.ChangeTracker.Clear();
            var Template = await context.Template.Where(s => s.Id == TemplateId).FirstOrDefaultAsync();
            context.Template.Remove(Template);
            context.SaveChanges();
        }


    }
}
