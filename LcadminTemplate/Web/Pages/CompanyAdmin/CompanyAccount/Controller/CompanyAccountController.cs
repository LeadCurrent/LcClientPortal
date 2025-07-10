using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Data;
//using OfficeOpenXml.Drawing.Chart;
using Project.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;


namespace Web
{
    [Authorize(Policy = "AdminOrCompanyRoles")]
    public class CompanyAccountController : Controller
    {
        
        private readonly RazorViewToStringRenderer viewRenderer;
        public ExceptionLogger exceptionLogger { get; }
        public UserDataLibrary UserDL { get; }
        public UserManager<User> UserManager { get; }
        public CompanyDataLibrary CompanyDL { get; }
        public EmailDataLibrary EmailDL { get; }
        public SignInManager<User> SignInManager { get; }
        public CompanyAccountController(
            RazorViewToStringRenderer RazorViewToStringRenderer,
            CompanyDataLibrary CompanyDataLibrary,
            UserDataLibrary UserDataLibrary,
            CompanyRolesDataLibrary CompanyRolesDataLibrary,
            EmailDataLibrary EmailDataLibrary,
            UserManager<User> userManager,
            
             ExceptionLogger _exceptionLogger,
             SignInManager<User> signInManager
        //ProductsDataLibrary ProductsDataLibrary
        )
        {
            viewRenderer = RazorViewToStringRenderer;
            UserDL = UserDataLibrary;
            CompanyDL = CompanyDataLibrary;
            UserManager = userManager;
            
            EmailDL = EmailDataLibrary;
            exceptionLogger = _exceptionLogger;
            SignInManager = signInManager;
            //ProductsDL = ProductsDataLibrary;
        }

        public CompanyAccountVM getModel()
        {
            var CompanyId = Int32.Parse(User.Claims.Where(x => x.Type == "CompanyId").FirstOrDefault().Value);
            var Model = new CompanyAccountVM();
            //Model.CompanyId = Model.Company.Id;
            Model.CompanyId = CompanyId;
            return Model;
        }


        
        [HttpGet]
        public async Task<IActionResult> Index(int ChangePlan)
        {
            try
            {
                var Model =  getModel();
                if (ChangePlan > 0)
                    Model.ShowChangePlan = true;
                if (HttpContext.Session.GetString("MobileApp") != null)
                    Model.MobileApp = true;

                Model.CurrentTab = "Account";
                if (HttpContext.Session.GetString("MobileApp") != null)
                {
                    Model.MobileApp = true;
                }
                return View("CompanyAccount", Model);

            }

            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, false);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CompanyAccount(CompanyAccountVM ViewModel, string Action)
        {
            try
            {
                var Model = getModel();

                if (HttpContext.Session.GetString("MobileApp") != null)
                    Model.MobileApp = true;

                Model.CurrentTab = "Account";

                var HTML = Task.Run(() => viewRenderer.RenderViewToStringAsync("CompanyAccount/PartialViews/CompanyAccount_Partial", Model)).Result;
                return Json(new { isValid = true, html = HTML });

            }

            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, false);
            }
        }
    }
}