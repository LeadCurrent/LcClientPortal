using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Data;
using Project.Utilities;
using CommonClasses;
using Microsoft.AspNetCore.Http.Extensions;


namespace Web.Controllers
{
    [Authorize]

    public class ErrorController : Controller
    {
        
        RazorViewToStringRenderer viewRenderer;
        ExceptionLogDataLibrary ExceptionLogDL { get; }
        CompanyDataLibrary CompanyDL { get; }
        UserDataLibrary UserDL { get; }
        public ExceptionLogger exceptionLogger { get; }
        public Email email { get; }


        public ErrorController(
            Email Email,
            RazorViewToStringRenderer RazorViewToStringRenderer,
            ExceptionLogDataLibrary ExceptionLogDataLibrary,
            UserDataLibrary UserDataLibrary,
            ExceptionLogger ExceptionLogger,
            CompanyDataLibrary CompanyDataLibrary
        )
        {
            email = Email;
            ExceptionLogDL = ExceptionLogDataLibrary;
            UserDL = UserDataLibrary;
            viewRenderer = RazorViewToStringRenderer;
            exceptionLogger = ExceptionLogger;
            CompanyDL = CompanyDataLibrary;

        }
            

        [HttpGet]
        public async Task<IActionResult> Index(int ExceptionLogId = 0)
        {
            var Model = new ErrorVM();
            try
            {

                Model.Environment = CommonClasses.Environment.environment;
                Model.ExceptionLog = await ExceptionLogDL.GetExceptionLog(ExceptionLogId);
                var EmailHTML = "";
                EmailHTML += "<b>DateTime: </b>" + CommonClasses.StringFormating.FormatDateTime(Model.ExceptionLog.DateTime) + "<br><br>";
                EmailHTML += "<b>Controller: </b>" + Model.ExceptionLog.Controller + "<br>";
                EmailHTML += "<b>Action: </b>" + Model.ExceptionLog.Action + "<br>";
                EmailHTML += "<b>User: </b>" + Model.ExceptionLog.UserId + "<br>";
                EmailHTML += "<b>User Email: </b>" + Model.ExceptionLog.UserEmailAddress + "<br>";
                EmailHTML += "<b>URL: </b>" + Model.ExceptionLog.URL + "<br>";
                EmailHTML += "<b>Page: </b>" + Model.ExceptionLog.Page + "<br>";
                EmailHTML += "<b>Notes: </b>" + Model.ExceptionLog.Notes + "<br>";
                EmailHTML += "<b>Model.ExceptionLog: </b>" + Model.ExceptionLog.Message + "<br>";
                EmailHTML += "<b>Stack Trace: </b>" + Model.ExceptionLog.StackTrace + "<br>";
                EmailHTML += "<b>Model: </b>" + Model.ExceptionLog.Model + "<br>";

                email.Message = EmailHTML;
                email.EmailFrom = CommonClasses.Environment.SupportEmail;
                email.EmailFromName = CommonClasses.Environment.SupportName;
                email.Subject = "An Error Occurred in " + CommonClasses.Environment.SiteName;
                email.EmailTo = CommonClasses.Environment.SupportEmail;
                await email.SendMail();
                return View("Error", Model);
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, false, this.ControllerContext.RouteData.Values["action"].ToString(), System.Text.Json.JsonSerializer.Serialize(Model), this.ControllerContext.RouteData.Values["controller"].ToString(), HttpContext.Request.GetDisplayUrl());
            }
        }

        [HttpPost]
        public async Task<IActionResult> Error(ErrorVM ViewModel, string Action)
        {
            try
            {
                if (Action == "Submit")
                {
                    var Error = await ExceptionLogDL.UpdateExceptionLog(ViewModel.ExceptionLog.Id, ViewModel.ExceptionLog.Page, ViewModel.ExceptionLog.Notes);
                    ViewModel.Submitted = true;

                    var user = await UserDL.GetUserbyUserName(Error.UserId);

                    var EmailHTML = "";
                    EmailHTML += "<b>Page: </b>" + Error.Page + "<br>";
                    EmailHTML += "<b>Notes: </b>" + Error.Notes + "<br>";
                    EmailHTML += "<b>DateTime: </b>" + CommonClasses.StringFormating.FormatDateTime(Error.DateTime) + "<br><br>";
                    EmailHTML += "<b>Controller: </b>" + Error.Controller + "<br>";
                    EmailHTML += "<b>Action: </b>" + Error.Action + "<br>";
                    EmailHTML += "<b>User: </b>" + Error.UserId + "<br>";
                    EmailHTML += "<b>User Email: </b>" + Error.UserEmailAddress + "<br>";
                    EmailHTML += "<b>URL: </b>" + Error.URL + "<br>";
                    EmailHTML += "<b>Error: </b>" + Error.Message + "<br>";
                    EmailHTML += "<b>Stack Trace: </b>" + Error.StackTrace + "<br>";
                    EmailHTML += "<b>Model: </b>" + Error.Model + "<br>";
                    var Subject = 

                    email.Message = EmailHTML;
                    email.EmailFrom = CommonClasses.Environment.SupportEmail;
                    email.EmailFromName = CommonClasses.Environment.SupportName;
                    email.Subject = "A comment was added to an error in " + CommonClasses.Environment.SiteName;
                    email.EmailTo = CommonClasses.Environment.SupportEmail;
                    await email.SendMail();

                }
                if (Action == "ShowTechnicalDetails")
                {
                    ViewModel.ExceptionLog = await ExceptionLogDL.GetExceptionLog(ViewModel.ExceptionLog.Id);
                    ViewModel.ShowTechnicalDetails = true;
                    ViewModel.Environment = CommonClasses.Environment.environment;
                }
                var HTML = Task.Run(() => viewRenderer.RenderViewToStringAsync("Error/Error_Partial", ViewModel)).Result;
                return Json(new { isValid = true, html = HTML });
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, ViewModel.AjaxUpdate, ViewModel.Action, System.Text.Json.JsonSerializer.Serialize(ViewModel), this.ControllerContext.RouteData.Values["controller"].ToString(), HttpContext.Request.GetDisplayUrl());
            }

        }
    }
}