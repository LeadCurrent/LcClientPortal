using Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using static Data.GeneralEnums;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using CommonClasses;


namespace Web.Controllers
{
    [Authorize]
    public class ContactUsController : Controller
    {
        
        private readonly RazorViewToStringRenderer viewRenderer;
        private UserDataLibrary UserDL { get; }
        public CompanyDataLibrary CompanyDL { get; }
        ExceptionLogger exceptionLogger { get; }
        public UserManager<User> UserManager { get; }
        Email email { get; }

        public ContactUsController(
            RazorViewToStringRenderer RazorViewToStringRenderer,
            UserDataLibrary UserDataLibrary,
            ExceptionLogger ExceptionLogger,
            CompanyDataLibrary CompanyDataLibrary,
            Email Email


        )
        {
            CompanyDL = CompanyDataLibrary;
            UserDL = UserDataLibrary;
            viewRenderer = RazorViewToStringRenderer;
            exceptionLogger = ExceptionLogger;
            email = Email;
        }
        public async Task<ContactUsVM> GetComanyListModel()
        {
            var Model = new ContactUsVM();
            Model.User = await UserDL.GetUserByUserName(User.Identity.Name);
            Model.Companys = new List<Company>();

            if (HttpContext.Session.GetInt32("FilterStatus") != null)
            {
                Model.FilterStatus = (Status)HttpContext.Session.GetInt32("FilterStatus").Value;
                Model.Companys = Model.Companys.Where(x => x.Status == Model.FilterStatus).ToList();
            }
            else if (HttpContext.Session.GetString("CompanyFilterName") != null)
            {
                Model.FilterName = HttpContext.Session.GetString("CompanyFilterName");
                Model.Companys = Model.Companys.Where(x => x.Name.ToLower() == Model.FilterName.ToLower()).ToList();
            }
            else
                Model.Companys = Model.Companys.Where(cu => cu.Status == Status.Active).ToList();
            return Model;
        }

        [HttpGet]
        public async Task<IActionResult> Index(bool MobileApp)
        {
            var Model = new ContactUsVM();
            try
            {
                Model = await GetComanyListModel();
                if (HttpContext.Session.GetString("MobileApp") != null)
                {
                    Model.MobileApp = true;
                }
                else if (MobileApp == true)
                {
                    Model.MobileApp = MobileApp;
                    HttpContext.Session.SetString("MobileApp", "True");
                }
                return View("ContactUsList", Model);
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, false, this.ControllerContext.RouteData.Values["action"].ToString(), System.Text.Json.JsonSerializer.Serialize(Model), this.ControllerContext.RouteData.Values["controller"].ToString(), HttpContext.Request.GetDisplayUrl());
            }
        }

        [HttpPost]
        public async Task<IActionResult> ContactUsList(ContactUsVM ViewModel, string Action)
        {
            try
            {
                var Model = new ContactUsVM();
                
                if (Action == "Apply Filters")
                {
                    if (ViewModel.FilterName != null)
                        HttpContext.Session.SetString("CompanyFilterName", ViewModel.FilterName);
                    else
                        HttpContext.Session.Remove("CompanyFilterName");

                    if (ViewModel.FilterStatus != 0)
                        HttpContext.Session.SetInt32("FilterStatus", (int)ViewModel.FilterStatus);
                    else
                        HttpContext.Session.Remove("FilterStatus");
                }
                if (Action == "Clear Filters")
                {
                    HttpContext.Session.Remove("CompanyFilterName");
                    HttpContext.Session.Remove("CompanyFilterStatus");
                }
               
                if (Action == "Edit")
                {
                    return RedirectToAction("Edit", new { CompanyId = ViewModel.Param });
                }
              
                Model = await GetComanyListModel();
               
                var HTML = Task.Run(() => viewRenderer.RenderViewToStringAsync("ContactUsList/PartialViews/ContactUsList_Partial", Model)).Result;
                return Json(new { isValid = true, html = HTML });
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, ViewModel.AjaxUpdate, ViewModel.Action, System.Text.Json.JsonSerializer.Serialize(ViewModel), HttpContext.Request.GetDisplayUrl(), HttpContext.Request.GetDisplayUrl());
            }
        }

        private async Task<ContactUsVM> GetEditModel(int CompanyId)
        {
            var Model = new ContactUsVM();                    
            Model.Company = await CompanyDL.GetCompany(CompanyId);

            return Model;
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int CompanyId)
        {
            var Model = new ContactUsVM();
            try
            {
                Model = await GetEditModel(CompanyId);
                if (HttpContext.Session.GetString("MobileApp") != null)
                {
                    Model.MobileApp = true;
                }
                return View("CompanyProfile", Model);
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, false, this.ControllerContext.RouteData.Values["action"].ToString(), System.Text.Json.JsonSerializer.Serialize(Model), this.ControllerContext.RouteData.Values["controller"].ToString(), HttpContext.Request.GetDisplayUrl());
            }
        }
       

    }
}