using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class ExceptionLogDataLibrary
    {
        public DataContext context { get; }

        public ExceptionLogDataLibrary(DataContext Context)
        {
            context = Context;
        }

        public async Task<List<ExceptionLog>> GetExceptionLogs()
        {
            List<ExceptionLog> ExceptionLogs = await context.ExceptionLog
            .ToListAsync();

            return ExceptionLogs;
        }

        public async Task<ExceptionLog> GetExceptionLog(int ExceptionLogId)
        {
            var ExceptionLog = await context.ExceptionLog
            .Where(x => x.Id == ExceptionLogId).FirstOrDefaultAsync();

            return ExceptionLog;
        }

        public async Task<List<ExceptionLog>> GetActiveExceptionLogs()
        {
            List<ExceptionLog> ExceptionLogs = await context.ExceptionLog
            .ToListAsync();

            return ExceptionLogs;
        }

        public async Task<int> CreateExceptionLog(ExceptionLog ExceptionLog)
        {
            context.ChangeTracker.Clear();
            await context.ExceptionLog.AddAsync(ExceptionLog);
            var success = await context.SaveChangesAsync() > 0;
            return ExceptionLog.Id;
        }

        public async Task<ExceptionLog> UpdateExceptionLog(int ExceptionLogId, string Page, string Notes)
        {
            context.ChangeTracker.Clear();
            var exception = await context.ExceptionLog.Where(x => x.Id == ExceptionLogId).FirstOrDefaultAsync();
            exception.Page = Page;
            exception.Notes = Notes;
            context.Entry(exception).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return exception;

        }

        public async Task<bool> DeleteExceptionLog(int ExceptionLogId)
        {
            context.ChangeTracker.Clear();
            var ExceptionLog = context.ExceptionLog.Where(s => s.Id == ExceptionLogId).FirstOrDefault();
            context.ExceptionLog.Remove(ExceptionLog);
            var success = await context.SaveChangesAsync() > 0;
            return success;
        }
    }
}
