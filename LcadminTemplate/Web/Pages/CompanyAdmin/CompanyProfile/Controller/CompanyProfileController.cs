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
using static Data.RoleEnums;
using Microsoft.EntityFrameworkCore;
using CommonClasses;


namespace Web
{
    [Authorize(Policy = "AdminOrCompanyProfile")]


    public class CompanyProfileController : Controller
    {
        
        private readonly RazorViewToStringRenderer viewRenderer;
        
        public ExceptionLogger exceptionLogger { get; }
        public UserDataLibrary UserDL { get; }
        public CompanyIntegrationDataLibrary CompanyIntegrationDL { get; }

        public UserManager<User> UserManager { get; }
        public CompanyDataLibrary CompanyDL { get; }
        public CompanyRolesDataLibrary CompanyRolesDL { get; }
        public EmailDataLibrary EmailDL { get; }
        public SignInManager<User> SignInManager { get; }


        public CompanyProfileController(
            RazorViewToStringRenderer RazorViewToStringRenderer,
            CompanyDataLibrary CompanyDataLibrary,
            UserDataLibrary UserDataLibrary,
            CompanyRolesDataLibrary CompanyRolesDataLibrary,
            EmailDataLibrary EmailDataLibrary,
            UserManager<User> userManager,
            
             ExceptionLogger _exceptionLogger,
             SignInManager<User> signInManager,
            CompanyIntegrationDataLibrary CompanyIntegrationDataLibrary
        )
        {
            viewRenderer = RazorViewToStringRenderer;
            UserDL = UserDataLibrary;
            CompanyDL = CompanyDataLibrary;
            CompanyRolesDL = CompanyRolesDataLibrary;
            UserManager = userManager;
            
            EmailDL = EmailDataLibrary;
            exceptionLogger = _exceptionLogger;
            SignInManager = signInManager;
            CompanyIntegrationDL = CompanyIntegrationDataLibrary;
        }

        public async Task<CompanyProfileVM> getModel()
        {
            var CompanyId = Int32.Parse(User.Claims.Where(x => x.Type == "CompanyId").FirstOrDefault().Value);
            var Model = new CompanyProfileVM();
            Model.Company = await CompanyDL.GetCompanyProfile(CompanyId);
            Model.CompanyUsers = await CompanyDL.GetCompanyUsers(CompanyId);
            Model.UserProfile = await UserDL.GetCurrentUser(User.Identity.Name);
            Model.Roles = await CompanyRolesDL.GetRoles(CompanyId);
            Model.CompanyId = Model.Company.Id;
            Model.CompanyEmailAccounts = await CompanyIntegrationDL.GetCompanyEmailAccounts(Model.CompanyId);
            var companyUserId = Model.CompanyUsers.FirstOrDefault(x => x.CompanyId == CompanyId)?.Id;
            if (companyUserId != null)
            {
                Model.CompayUserRoles = await CompanyRolesDL.GetCompanyUserRoles(companyUserId.Value);

            }
            return Model;
        }

        [HttpGet]
        public async Task<IActionResult> Index(bool MobileApp)
        {
            try
            {
                var Model = await getModel();
                if (HttpContext.Session.GetString("MobileApp") != null)
                    Model.MobileApp = true;
                else if (MobileApp == true)
                {
                    Model.MobileApp = MobileApp;
                    HttpContext.Session.SetString("MobileApp", "True");
                }
                Model.CurrentTab = "Company Profile";
                return View("CompanyProfile", Model);
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, false);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Profile(CompanyProfileVM ViewModel, string Action)
        {
            try
            {
                if (Action == "Update Profile")
                    return RedirectToAction("EditProfile");

                if (Action == "Cancel") { }

               
                if (Action == "Update Contact")
                {
                    await CompanyDL.UpdateCompanyContact(ViewModel.Company.CompanyContacts.Where(x => x.Id == ViewModel.Param).FirstOrDefault(), User.Identity.Name);
                }
               
                if (Action == "Create Contact")
                {
                    ViewModel.CompanyContact.CompanyId = ViewModel.Company.Id;
                    await CompanyDL.CreateCompanyContact(ViewModel.CompanyContact, User.Identity.Name);
                }

                var Model = await getModel();

                if (Action == "Show Edit Contact")
                {
                    Model.EditContactId = ViewModel.Param;
                }
                if (Action == "Add New Contact")
                {
                    Model.CompanyUsers = await CompanyDL.GetCompanyUsers(ViewModel.Company.Id);
                    Model.CompanyContact = new CompanyContact();
                    Model.AddNewContact = true;
                }

                if (Action == "Show Add Social Media")
                    Model.ShowAddSocialMedia = true;

                if (Action == "Show Update Services" || Action == "Add New Service" || Action == "Create New Service")
                    Model.ShowUpdateServices = true;

                if (Action == "Add New Service")
                    Model.NewService = true;


                if (Action == "Cancel")
                {
                    Model.ShowEditAccountHolder = false;
                    Model.ShowEditBillingContact = false;
                    Model.ShowEditSupportContact = false;
                }

                if (Action == "Change Assign User")
                {
                    await CompanyDL.UpdateCompanyForCompanyAccount(ViewModel.Company, User.Identity.Name, ViewModel.SelectedAccountHolderId, ViewModel.SelectedBillingContactId, ViewModel.SelectedSupportContactId);
                    Model = await getModel();
                }

               

                if (HttpContext.Session.GetString("MobileApp") != null)
                    Model.MobileApp = true;

                Model.CurrentTab = "Company Profile";

                var HTML = Task.Run(() => viewRenderer.RenderViewToStringAsync("CompanyProfile/PartialViews/CompanyProfile_Partial", Model)).Result;
                return Json(new { isValid = true, html = HTML });
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, ViewModel.AjaxUpdate, ViewModel.Action, System.Text.Json.JsonSerializer.Serialize(ViewModel));
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            try
            {
                var Model = new CompanyProfileVM();
                var CompanyId = Int32.Parse(User.Claims.Where(x => x.Type == "CompanyId").FirstOrDefault().Value);
                Model.Company = await CompanyDL.GetCompanyProfile(CompanyId);
                if (HttpContext.Session.GetString("MobileApp") != null)
                    Model.MobileApp = true;

                Model.CurrentTab = "Company Profile";
                return View("EditCompanyProfile", Model);
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, false);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(CompanyProfileVM ViewModel, string Action)
        {
            try
            {
                if (Action == "Save Changes")
                {
                    if (ViewModel.Logo != null)
                    {
                        string filename = "CompanyLogos/" + ViewModel.Company.Name.Replace(" ", "") + "Logo.png";
                        Storage.uploadimage(ViewModel.Logo, filename);
                        ViewModel.Company.Logo = CommonClasses.Environment.StorageURL() + filename;
                    }
                    await CompanyDL.UpdateCompanyFromCompanyProfile(ViewModel.Company, User.Identity.Name);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, ViewModel.AjaxUpdate, ViewModel.Action, System.Text.Json.JsonSerializer.Serialize(ViewModel));
            }
        } 

    }
}