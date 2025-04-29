using Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Project.Utilities;
using Microsoft.Extensions.Hosting;
using CommonClasses;

namespace Web
{
    public class ExceptionLogger : Controller
    {
        
        ExceptionLogDataLibrary ExceptionLogDL { get; }
        private readonly RazorViewToStringRenderer viewRenderer;
        public ExceptionLogger(ExceptionLogDataLibrary ExceptionLogDataLibrary,
            RazorViewToStringRenderer RazorViewToStringRenderer)
        {
            ExceptionLogDL = ExceptionLogDataLibrary;
            viewRenderer = RazorViewToStringRenderer;
        }
        public async Task<IActionResult> LogException(Exception ex, string UserName = null, bool AjaxUpdate = false, string Action = null, string Model = null, string ControllerName = null, string PageURL = null)
        {
            var ExceptionLog = new ExceptionLog();

            ExceptionLog.UserId = UserName;
            ExceptionLog.Action = Action;
            if (ControllerName != null)
                ExceptionLog.Controller = ControllerName;
            ExceptionLog.DateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time");
            if (ex.InnerException != null)
                ExceptionLog.InnerException = ex.InnerException.Message;
            ExceptionLog.StackTrace = ex.StackTrace;
            ExceptionLog.Message = ex.Message;
            ExceptionLog.Model = Model;
            ExceptionLog.URL = PageURL;
            ExceptionLog.Id = await ExceptionLogDL.CreateExceptionLog(ExceptionLog);

            if (AjaxUpdate)
            {
                var ErrorVM = new ErrorVM();
                ErrorVM.ExceptionLog = ExceptionLog;
                ErrorVM.Environment = CommonClasses.Environment.environment;
                var HTML = await viewRenderer.RenderViewToStringAsync("Error/ErrorRedirect", ErrorVM);
                return Json(new { isValid = true, html = HTML });
            }
            else
                return RedirectToAction("Index", "Error", new { ExceptionLogId = ExceptionLog.Id });
        }
    }
}
