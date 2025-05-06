using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Data;
using Project.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using static Data.GeneralEnums;
using static Data.CompanyEnums;
using CommonClasses;
using DinkToPdf;
using DinkToPdf.Contracts;
using static Data.RoleEnums;
namespace Web
{
    //[Authorize(Policy = "SystemAdmin")]

    public class CompanyController : Controller
    {
        private readonly IConverter PDFConverter;

        private readonly RazorViewToStringRenderer viewRenderer;
        public UserManager<User> UserManager { get; }
        public UserDataLibrary UserDL { get; }
        public SignInManager<User> SignInManager { get; }
        public CompanyDataLibrary CompanyDL { get; }
        public CompanyRolesDataLibrary CompanyRolesDL { get; }
        public ExceptionLogger exceptionLogger { get; }

        public CompanyController(

            UserManager<User> userManager,
            SignInManager<User> signInManager,
            UserDataLibrary UserDataLibrary,
            RazorViewToStringRenderer RazorViewToStringRenderer,
            CompanyDataLibrary CompanyDataLibrary,
            CompanyRolesDataLibrary CompanyRolesDataLibrary,
            ExceptionLogger _exceptionLogger,
                    IConverter converter

        )
        {
            UserManager = userManager;
            SignInManager = signInManager;
            CompanyDL = CompanyDataLibrary;
            viewRenderer = RazorViewToStringRenderer;
            UserDL = UserDataLibrary;
            CompanyRolesDL = CompanyRolesDataLibrary;
            exceptionLogger = _exceptionLogger;
            PDFConverter = converter;

        }

        public async Task<CompanyViewModel> getCompanyViewModel(bool MobileApp = false)
        {
            var Model = new CompanyViewModel();
            if (HttpContext.Session.GetString("CompanyFilterName") != null)
                Model.FilterName = HttpContext.Session.GetString("CompanyFilterName");

            if (HttpContext.Session.GetInt32("CompanyFilterStatus") != null)
                Model.FilterStatus = (int)HttpContext.Session.GetInt32("CompanyFilterStatus");

            Model.Companys = await CompanyDL.GetCompanys();
            if (User.Claims.Where(x => x.Type == "CompanyId").FirstOrDefault() != null)
            {
                int currentcompanyid = Int32.Parse(User.Claims.Where(x => x.Type == "CompanyId").FirstOrDefault().Value);
                if (Model.Companys.Where(x => x.Id == currentcompanyid).FirstOrDefault() != null)
                    Model.Companys.Where(x => x.Id == currentcompanyid).FirstOrDefault().Selected = true;
            }

            Model.Companys = await CompanyDL.GetFilteredCompanys(Model.FilterName, Model.FilterStatus);
            if (HttpContext.Session.GetString("MobileApp") != null)
                Model.MobileApp = true;
            else if (MobileApp == true)
            {
                Model.MobileApp = MobileApp;
                HttpContext.Session.SetString("MobileApp", "True");
            }
            return Model;
        }

        [HttpGet]
        public async Task<IActionResult> Index(bool MobileApp)
        {
            try
            {
                return View("CompanyList", await getCompanyViewModel(MobileApp));
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, false);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CompanyList(CompanyViewModel ViewModel, string Action)
        {
            try
            {
                if (Action == "Edit")
                    return RedirectToAction("Profile", new { CompanyId = ViewModel.Param });

                if (Action == "Create")
                    return RedirectToAction("Create");

                if (Action == "Select Company")
                {
                    await SelectCompany(ViewModel , Action);
                }

                if (Action == "Apply Filters")
                {
                    if (ViewModel.FilterName != null)
                        HttpContext.Session.SetString("CompanyFilterName", ViewModel.FilterName);
                    else
                        HttpContext.Session.Remove("CompanyFilterName");

                    if (ViewModel.FilterStatus != 0)
                        HttpContext.Session.SetInt32("CompanyFilterStatus", ViewModel.FilterStatus);
                    else
                        HttpContext.Session.Remove("CompanyFilterStatus");
                }

                if (Action == "Clear Filters")
                {
                    HttpContext.Session.Remove("CompanyFilterName");
                    HttpContext.Session.Remove("CompanyFilterStatus");
                }

                var VM = await getCompanyViewModel();

                var HTML = Task.Run(() => viewRenderer.RenderViewToStringAsync("Company/PartialViews/CompanyList_Partial", VM)).Result;
                return Json(new { isValid = true, html = HTML });

            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, ViewModel.AjaxUpdate, ViewModel.Action, System.Text.Json.JsonSerializer.Serialize(ViewModel));
            }
        }



        [HttpGet]
        public async Task<IActionResult> Create(string message)
        {
            try
            {
                var Model = new CompanyViewModel();
                Model.Company = new Company();
                Model.Companys = await CompanyDL.GetCompanys();
                ViewBag.Message = message;
                if (HttpContext.Session.GetString("MobileApp") != null)
                    Model.MobileApp = true;
                return View("Create", Model);
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, false);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(CompanyViewModel ViewModel, string Action)
        {
            try
            {
                var Model = new CompanyViewModel();

                int userCount = await UserDL.GetUserCountAsync();//to check first login

                if (Action == "Create")//old
                {
                    var NewUser = ViewModel.User;
                    //User
                    var result = await UserManager.CreateAsync(NewUser, ViewModel.Password);
                    if (result.Succeeded)
                    {
                        //company
                        var (Company, CompanyUserId) = await CompanyDL.CreateCompany(ViewModel.Company, NewUser);

                        //Role
                        var AdminRole = await CompanyRolesDL.CreateRole("Admin", Company.Id);
                        var CompanyUserRole = await CompanyRolesDL.CreateCompanyUserRole(CompanyUserId, AdminRole.Id);

                        //claims
                        await UserDL.AddUserClaimAsync(NewUser.Id, "CompanyId", Company.Id.ToString());
                        await UserDL.AddUserClaimAsync(NewUser.Id, "Plan", "Default");
                        await UserDL.AddUserClaimAsync(NewUser.Id, "Admin", "True");
                        await UserDL.AddUserClaimAsync(NewUser.Id, "CompanyName", Company.Name);

                        foreach (var p in AdminRole.RolePermissions)
                            await UserDL.AddUserClaimAsync(NewUser.Id, p.Permission.ToString(), AdminRole.Id.ToString());
                        var claimsPrincipal = await SignInManager.CreateUserPrincipalAsync(NewUser);


                        if(userCount == 1)
                        {
                            ViewModel.Param = Company.Id;
                            await SelectCompany(ViewModel, Action);
                        }
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        return RedirectToAction("Create", new { message = result.Errors.Select(x => x.Description).FirstOrDefault() });
                    }
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, ViewModel.AjaxUpdate, ViewModel.Action, System.Text.Json.JsonSerializer.Serialize(ViewModel));
            }
        }

        private async Task<CompanyViewModel> GetModel(int CompanyId, string CurrentTab)
        {
            var Model = new CompanyViewModel();
            Model.CurrentTab = CurrentTab;
            if (User.Claims.Where(c => c.Type == "SystemAdmin").FirstOrDefault() != null)
                Model.SystemAdmin = true;



            if (CurrentTab == "Profile")
            {
                Model.Company = await CompanyDL.GetCompanyProfile(CompanyId);
                Model.CompanyNotes = Model.Company.CompanyNotes;
            }

            else
            {
                Model.Company = await CompanyDL.GetBasicCompany(CompanyId);
            }

            if (User.Claims.Where(x => x.Type == "CompanyId").FirstOrDefault() != null)
            {
                int currentcompanyid = Int32.Parse(User.Claims.Where(x => x.Type == "CompanyId").FirstOrDefault().Value);
                if (Model.Company.Id == currentcompanyid)
                    Model.Company.Selected = true;
            }



            if (HttpContext.Session.GetString("MobileApp") != null)
                Model.MobileApp = true;
            return Model;
        }

        [HttpGet]
        public async Task<IActionResult> Profile(int CompanyId)
        {
            try
            {
                return View("Profile", await GetModel(CompanyId, "Profile"));
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, false);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Profile(CompanyViewModel ViewModel, string Action)
        {
            try
            {
                var Model = new CompanyViewModel();

                if (Action == "Update")
                {
                    await CompanyDL.UpdateCompanyFromSystemAdmin(ViewModel.Company, User.Identity.Name);
                    Model = await GetModel(ViewModel.Company.Id, ViewModel.CurrentTab);
                    ViewModel.UpdateSuccessful = true;
                }


                if (Action == "Remove")
                {
                    await CompanyDL.DeleteCompany(ViewModel.Company.Id);
                    return RedirectToAction("Index");
                }


                if (Action == "Switch Company")
                {
                    var user = await UserDL.GetCurrentUser(User.Identity.Name);
                    var claims = await UserManager.GetClaimsAsync(user);
                    if (claims.Where(c => c.Type == "CompanyId").Any())
                    {
                        var CompanyIdClaim = claims.Where(c => c.Type == "CompanyId").FirstOrDefault();
                        await UserManager.RemoveClaimAsync(user, CompanyIdClaim);
                        var CompanyNameClaims = claims.Where(x => x.Type == "CompanyName").ToList();
                        foreach (var claim in CompanyNameClaims)
                            await UserManager.RemoveClaimAsync(user, claim);
                    }
                    await UserManager.AddClaimAsync(user, new Claim("CompanyId", ViewModel.Company.Id.ToString()));
                    await UserManager.AddClaimAsync(user, new Claim("CompanyName", ViewModel.Company.Name.ToString()));
                    await SignInManager.SignOutAsync();
                    await SignInManager.SignInAsync(user, true);

                    return RedirectToAction("Profile", new { CompanyId = ViewModel.Company.Id });
                }

                if (Action == "Show Add Note")
                {
                    Model = await GetModel(ViewModel.Company.Id, ViewModel.CurrentTab);
                    Model.ShowAddNote = true;
                }

                if (Action == "Cancel")
                {
                    Model = await GetModel(ViewModel.Company.Id, ViewModel.CurrentTab);
                }

                if (Action == "Add Note")
                {
                    await CompanyDL.AddCompanyNote(ViewModel.AddNote, NoteType.General, ViewModel.Company.Id, User.Identity.Name);
                    Model = await GetModel(ViewModel.Company.Id, ViewModel.CurrentTab);
                }

                if (Action == "Remove Note")
                {
                    await CompanyDL.RemoveCompanyNote(ViewModel.Param);
                    Model = await GetModel(ViewModel.Company.Id, ViewModel.CurrentTab);
                }


                if (Action == "Change Assign User")
                {
                    await CompanyDL.UpdateCompanyForCompanyAccount(ViewModel.Company, User.Identity.Name, ViewModel.SelectedAccountHolderId, ViewModel.SelectedBillingContactId, ViewModel.SelectedSupportContactId);
                    Model = await GetModel(ViewModel.Company.Id, ViewModel.CurrentTab);
                }


                if (Action == "EditCompanyDetails")
                {
                    Model = await GetModel(ViewModel.Company.Id, ViewModel.CurrentTab);
                    Model.ShowEditCompanyDetails = true;
                }

                if (Action == "Show Edit Contact")
                {
                    Model = await GetModel(ViewModel.Company.Id, ViewModel.CurrentTab);
                    Model.CompanyContact = Model.Company.CompanyContacts.Where(x => x.Id == ViewModel.Param).FirstOrDefault();
                }
                if (Action == "Update Contact")
                {
                    await CompanyDL.UpdateCompanyContact(ViewModel.CompanyContact, User.Identity.Name);
                    Model = await GetModel(ViewModel.Company.Id, ViewModel.CurrentTab);
                }
                if (Action == "Add New Contact")
                {
                    Model = await GetModel(ViewModel.Company.Id, ViewModel.CurrentTab);
                    Model.CompanyUsers = await CompanyDL.GetCompanyUsers(ViewModel.Company.Id);
                    Model.CompanyContact = new CompanyContact();
                    Model.AddNewContact = true;
                }
                if (Action == "Create Contact")
                {
                    ViewModel.CompanyContact.CompanyId = ViewModel.Company.Id;
                    ViewModel.CompanyContact.CompanyUserId = ViewModel.CompanyUserId;
                    ViewModel.CompanyContact.PrimaryContact = ViewModel.PrimaryContact;

                    await CompanyDL.CreateCompanyContact(ViewModel.CompanyContact, User.Identity.Name);
                    Model = await GetModel(ViewModel.Company.Id, ViewModel.CurrentTab);
                }

                if (Action == "Cancel")
                {
                    Model = await GetModel(ViewModel.Company.Id, ViewModel.CurrentTab);
                }

                var HTML = Task.Run(() => viewRenderer.RenderViewToStringAsync("Company/PartialViews/Profile_Partial", Model)).Result;
                return Json(new { isValid = true, html = HTML });

            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, ViewModel.AjaxUpdate, ViewModel.Action, System.Text.Json.JsonSerializer.Serialize(ViewModel));
            }
        }

        public async Task<IActionResult> SelectCompany(CompanyViewModel ViewModel, string Action)
        {
            var user = await UserDL.GetCurrentUser(User.Identity.Name);
            var claims = await UserManager.GetClaimsAsync(user);
            if (claims.Where(c => c.Type == "CompanyId").Any())
            {
                var CompanyIdClaim = claims.Where(c => c.Type == "CompanyId").FirstOrDefault();
                await UserManager.RemoveClaimAsync(user, CompanyIdClaim);
            }
            await UserManager.AddClaimAsync(user, new Claim("CompanyId", ViewModel.Param.ToString()));

            var Company = await CompanyDL.GetCompany(ViewModel.Param);
            var CompanyNameClaim = User.Claims.Where(x => x.Type == "CompanyName").FirstOrDefault();
            if (CompanyNameClaim == null)
            {
                await UserManager.AddClaimAsync(user, new Claim("CompanyName", Company.Name));
            }
            else
            {
                var CompanyNameClaims = User.Claims.Where(x => x.Type == "CompanyName").ToList();
                foreach (var claim in CompanyNameClaims)
                    await UserManager.RemoveClaimAsync(user, claim);

                await UserManager.AddClaimAsync(user, new Claim("CompanyName", Company.Name));
            }

            await SignInManager.SignOutAsync();
            await SignInManager.SignInAsync(user, true);
            return RedirectToAction("Index");
        }

    }
}