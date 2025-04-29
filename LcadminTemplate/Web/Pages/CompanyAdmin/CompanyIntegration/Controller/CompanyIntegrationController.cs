using Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Identity;
using Project.Utilities;
using CommonClasses;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace Web
{
    [Authorize(Policy = "AdminOrIntegration")]

    public class CompanyIntegrationController : Controller
    {
        
        private readonly RazorViewToStringRenderer viewRenderer;       
        public ExceptionLogger exceptionLogger { get; }
        public UserDataLibrary UserDL { get; }
        public UserManager<User> UserManager { get; }
        public CompanyDataLibrary CompanyDL { get; }
        public CompanyRolesDataLibrary CompanyRolesDL { get; }
        public EmailDataLibrary EmailDL { get; }
        public SignInManager<User> SignInManager { get; }
        public CompanyIntegrationDataLibrary CompanyIntegrationDL { get; set; }
        public CompanyIntegrationController(
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
            //ProductsDL = ProductsDataLibrary;
        }
        public async Task<CompanyIntegrationVM> getModel()
        {
            var CompanyId = Int32.Parse(User.Claims.Where(x => x.Type == "CompanyId").FirstOrDefault().Value);
            var Model = new CompanyIntegrationVM();
            Model.Company = await CompanyDL.GetCompany(CompanyId);
            Model.UserProfile = await UserDL.GetCurrentUser(User.Identity.Name);

            Model.CompanyId = CompanyId;
            Model.CompanyPhoneNumbers = await CompanyIntegrationDL.GetCompanyPhoneNumbers(CompanyId);
            Model.CompanyEmailAccounts = await CompanyIntegrationDL.GetCompanyEmailAccounts(Model.CompanyId);
           
            return Model;
        }

        [HttpPost]
        public async Task<IActionResult> QuickBooksRedirect(CompanyIntegrationVM ViewModel, string Action)
        {
            try
            {
                var Model = await getModel();
                var HTML2 = Task.Run(() => viewRenderer.RenderViewToStringAsync("CompanyIntegration/PartialViews/Integration_Partial", Model)).Result;

                return Json(new { isValid = true, html = HTML2 });
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, ViewModel.AjaxUpdate, ViewModel.Action, System.Text.Json.JsonSerializer.Serialize(ViewModel));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var Model = await getModel();
                Model.Company = await CompanyDL.GetCompany(Model.Company.Id);
                if (HttpContext.Session.GetString("MobileApp") != null)
                    Model.MobileApp = true;

                Model.CurrentTab = "Integrations";
                return View("Integration", Model);
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, false);
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> Integration(CompanyIntegrationVM ViewModel, string Action)
        {
            try
            {
                var CompanyId = Int32.Parse(User.Claims.Where(x => x.Type == "CompanyId").FirstOrDefault().Value);

                if (Action == "Connect to my gmail account")
                {
                    if (ViewModel.LastSyncDate.HasValue)
                    {
                        HttpContext.Session.SetString("LastSyncDate", ViewModel.LastSyncDate.Value.ToString("o"));
                    }
                    if (ViewModel.AllStaffAccess)
                        HttpContext.Session.SetInt32("AllStaffAccess", 1);
                    else
                        HttpContext.Session.SetInt32("AllStaffAccess", 0);

                    return RedirectToAction("GoogleAuthentication");
                }

                if (Action == "Connect to my microsoft account")
                {

                    if (ViewModel.LastSyncDate.HasValue)
                    {
                        HttpContext.Session.SetString("LastSyncDate", ViewModel.LastSyncDate.Value.ToString("o"));
                    }
                    if (ViewModel.AllStaffAccess)
                        HttpContext.Session.SetInt32("AllStaffAccess", 1);
                    else
                        HttpContext.Session.SetInt32("AllStaffAccess", 0);

                    return RedirectToAction("MicrosoftAuthentication");
                }
                if (Action == "Reconnect Account")
                {
                    var CompanyEmailAccount = await CompanyIntegrationDL.GetCompanyEmailAccount(ViewModel.Param);

                    if (CompanyEmailAccount.EmailType == CompanyEnums.EmailType.Gmail)
                    {
                        HttpContext.Session.SetString("LastSyncDate", CompanyEmailAccount.LastSyncDate.ToString("o")); return RedirectToAction("GoogleAuthentication");
                    }
                    if (CompanyEmailAccount.EmailType == CompanyEnums.EmailType.Office365)
                    {
                        HttpContext.Session.SetString("LastSyncDate", CompanyEmailAccount.LastSyncDate.ToString("o"));
                        return RedirectToAction("MicrosoftAuthentication");

                    }
                }
                if (Action == "Connect to my other account")
                {
                    // await CompanyDL.CreateCompanyEmailAccount(CompanyId, 3, ViewModel.OtherAccountName, ViewModel.OtherAccountEmail, null);
                }
                if (Action == "Connect to Phone Number")
                {

                    await CompanyIntegrationDL.CreateCompanyPhoneNumber(CompanyId, ViewModel.PhoneNumber, ViewModel.ContactName,ViewModel.AllStaffAccess);
                }
                if (Action == "Remove Email Account")
                {
                    await CompanyIntegrationDL.DeleteEmailAccount(ViewModel.Param);
                    return RedirectToAction("Index");
                }
                if (Action == "Remove Default Email Account")
                {
                    await CompanyIntegrationDL.DeleteEmailAccount(ViewModel.Param);
                    return RedirectToAction("Index");
                }
                if (Action == "Set Default Email")
                {
                    await CompanyIntegrationDL.SetDefaultEmail(ViewModel.Param);
                    return RedirectToAction("Index");
                }
                if (Action == "Set Default PhoneNumber")
                {
                    await CompanyIntegrationDL.SetDefaultPhoneNumber(ViewModel.Param);
                    return RedirectToAction("Index");
                }
                if (Action == "Remove Default Phone Number")
                {
                    await CompanyIntegrationDL.DeletePhoneNumber(ViewModel.Param);
                    return RedirectToAction("Index");
                }
                if (Action == "Remove Phone Number")
                {
                    await CompanyIntegrationDL.DeletePhoneNumber(ViewModel.Param);
                    return RedirectToAction("Index");
                }
                if (Action == "Update Email Account")
                {
                    var CompanyEmailAccount = await CompanyIntegrationDL.GetCompanyEmailAccount(ViewModel.Param);
                    if (CompanyEmailAccount.EmailType != CompanyEnums.EmailType.Other)
                    {
                        CompanyEmailAccount.AllStaffAccess = ViewModel.CompanyEmailAccount.AllStaffAccess;
                        CompanyEmailAccount.LastSyncDate = ViewModel.CompanyEmailAccount.LastSyncDate;
                        CompanyEmailAccount.CalendarColor = ViewModel.CompanyEmailAccount.CalendarColor;
                    }
                    else
                    {
                        CompanyEmailAccount.Name = ViewModel.CompanyEmailAccount.Name;
                        CompanyEmailAccount.Email = ViewModel.CompanyEmailAccount.Email;
                    }
                    await CompanyIntegrationDL.UpdateCompanyEmailAccount(CompanyEmailAccount);

                }   
                if (Action == "Update Phone Number")
                {
                    var CompanyPhoneNumber = await CompanyIntegrationDL.GetCompanyPhoneNumberById(ViewModel.Param);

                    CompanyPhoneNumber.Name = ViewModel.CompanyPhoneNumber.Name;
                    CompanyPhoneNumber.PhoneNumber = ViewModel.CompanyPhoneNumber.PhoneNumber;
                    CompanyPhoneNumber.AllStaffAccess = ViewModel.CompanyPhoneNumber.AllStaffAccess;
                    await CompanyIntegrationDL.UpdateCompanyPhoneNUmber(CompanyPhoneNumber);

                }
                var Model = await getModel();
                Model.Company = await CompanyDL.GetCompany(Model.Company.Id);
                if (Action == "Add Access")
                {
                    var CompanyUser = await CompanyDL.GetCompanyUser(ViewModel.CompanyUserId);
                    var CompanyEmailAccount = await CompanyIntegrationDL.GetCompanyEmailAccount(ViewModel.CompanyEmailAccount.Id);
                    CompanyEmailAccount.AllStaffAccess = false;
                    await CompanyIntegrationDL.UpdateCompanyEmailAccount(CompanyEmailAccount);
                    CompanyEmailAccount = await CompanyIntegrationDL.GetCompanyEmailAccount(ViewModel.CompanyEmailAccount.Id);
                    await CompanyIntegrationDL.AddCompanyUserEmail(CompanyEmailAccount.Id, CompanyUser.Id);
                    // var VM = await GetProfileModel();
                    Model.EditEmailAccount = true;
                    Model.CompanyEmailAccount = await CompanyIntegrationDL.GetCompanyEmailAccount(ViewModel.CompanyEmailAccount.Id);
                    Model.CompanyUserEmails = await CompanyIntegrationDL.GetCompanyUserEmailsByCompanyEmailAccountId(ViewModel.CompanyEmailAccount.Id) ?? new List<CompanyUserEmail>(); ;
                    Model.CompanyUsers = await UserDL.GetCompanyUsers(Model.Company.Id) ?? new List<CompanyUser>();
                    Model.CompanyUsers = Model.CompanyUsers
                   .Where(user => !Model.CompanyUserEmails.Any(email => email.CompanyUserId == user.Id))
                  .ToList();

                   
                }  
                if (Action == "Add Access Phone")
                {
                    var CompanyUser = await CompanyDL.GetCompanyUser(ViewModel.CompanyUserId);
                    var CompanyPhoneNumber = await CompanyIntegrationDL.GetCompanyPhoneNumberById(ViewModel.CompanyPhoneNumber.Id);
                    CompanyPhoneNumber.AllStaffAccess = false;
                    await CompanyIntegrationDL.UpdateCompanyPhoneNUmber(CompanyPhoneNumber);
                    CompanyPhoneNumber = await CompanyIntegrationDL.GetCompanyPhoneNumberById(ViewModel.CompanyPhoneNumber.Id);
                    await CompanyIntegrationDL.AddCompanyUserPhoneNumber(CompanyPhoneNumber.Id, CompanyUser.Id);
                    // var VM = await GetProfileModel();
                    Model.EditPhoneNumber = true;
                    Model.CompanyPhoneNumber = await CompanyIntegrationDL.GetCompanyPhoneNumberById(ViewModel.CompanyPhoneNumber.Id);
                    Model.CompanyUserPhoneNumbers = await CompanyIntegrationDL.GetCompanyUserPhoneNumberByCompanyPhoneNumberId(ViewModel.CompanyPhoneNumber.Id) ?? new List<CompanyUserPhoneNumber>(); ;
                    Model.CompanyUsers = await UserDL.GetCompanyUsers(Model.Company.Id) ?? new List<CompanyUser>();
                    Model.CompanyUsers = Model.CompanyUsers
                   .Where(user => !Model.CompanyUserPhoneNumbers.Any(email => email.CompanyUserId == user.Id))
                  .ToList();

    
                }
                if (Action == "Remove Access")
                {
                    await CompanyIntegrationDL.DeleteCompanyUserEmailById(ViewModel.Param);
                    var CompanyEmailAccount = await CompanyIntegrationDL.GetCompanyEmailAccount(ViewModel.CompanyEmailAccount.Id);
                    CompanyEmailAccount.AllStaffAccess = false;
                    await   CompanyIntegrationDL.UpdateCompanyEmailAccount(CompanyEmailAccount);
                    //var VM = await GetProfileModel();
                    Model.EditEmailAccount = true;
                    Model.CompanyEmailAccount = await CompanyIntegrationDL.GetCompanyEmailAccount(ViewModel.CompanyEmailAccount.Id);
                    Model.CompanyUserEmails = await CompanyIntegrationDL.GetCompanyUserEmailsByCompanyEmailAccountId(ViewModel.CompanyEmailAccount.Id) ?? new List<CompanyUserEmail>(); ;
                    Model.CompanyUsers = await UserDL.GetCompanyUsers(Model.Company.Id) ?? new List<CompanyUser>();
                    Model.CompanyUsers = Model.CompanyUsers
                   .Where(user => !Model.CompanyUserEmails.Any(email => email.CompanyUserId == user.Id))
                  .ToList();

                }  
                if (Action == "Remove Access Phone")
                {
                    await CompanyIntegrationDL.DeleteCompanyUserPhoneNumberById(ViewModel.Param);
                    var CompanyPhoneNumber = await CompanyIntegrationDL.GetCompanyPhoneNumberById(ViewModel.CompanyPhoneNumber.Id);
                    CompanyPhoneNumber.AllStaffAccess = false;
                    await   CompanyIntegrationDL.UpdateCompanyPhoneNUmber(CompanyPhoneNumber);
                    //var VM = await GetProfileModel();
                    Model.EditPhoneNumber = true;
                    Model.CompanyPhoneNumber = await CompanyIntegrationDL.GetCompanyPhoneNumberById(ViewModel.CompanyPhoneNumber.Id);
                    Model.CompanyUserPhoneNumbers = await CompanyIntegrationDL.GetCompanyUserPhoneNumberByCompanyPhoneNumberId(ViewModel.CompanyEmailAccount.Id) ?? new List<CompanyUserPhoneNumber>(); ;
                    Model.CompanyUsers = await UserDL.GetCompanyUsers(Model.Company.Id) ?? new List<CompanyUser>();
                    Model.CompanyUsers = Model.CompanyUsers
                   .Where(user => !Model.CompanyUserEmails.Any(email => email.CompanyUserId == user.Id))
                  .ToList();

                }
                if (Action == "Show Other Account Fields")
                {
                    Model.ShowAddOtherAccount = true;
                    Model.CurrentTab = "Integrations";

                }
                if (Action == "Show Add Phone Number")
                {
                    Model.ShowAddPhoneNumber = true;
                    Model.AllStaffAccess = true;
                    Model.CurrentTab = "Integrations";
                }
                if (Action == "Edit Email Account")
                {
                    Model.EditEmailAccount = true;
                    Model.CompanyEmailAccount = await CompanyIntegrationDL.GetCompanyEmailAccount(ViewModel.Param);
                    Model.CompanyUserEmails = await CompanyIntegrationDL.GetCompanyUserEmailsByCompanyEmailAccountId(ViewModel.Param) ?? new List<CompanyUserEmail>(); ;
                    Model.CompanyUsers = await UserDL.GetCompanyUsers(Model.Company.Id) ?? new List<CompanyUser>();
                    Model.CompanyUsers = Model.CompanyUsers
                   .Where(user => !Model.CompanyUserEmails.Any(email => email.CompanyUserId == user.Id))
                  .ToList();
                }     
                if (Action == "Edit Phone Number")
                {
                    Model.EditPhoneNumber = true;
                    Model.CompanyPhoneNumber = await CompanyIntegrationDL.GetCompanyPhoneNumberById(ViewModel.Param);
                    Model.CompanyUserPhoneNumbers = await CompanyIntegrationDL.GetCompanyUserPhoneNumberByCompanyPhoneNumberId(ViewModel.Param) ?? new List<CompanyUserPhoneNumber>(); ;
                    Model.CompanyUsers = await UserDL.GetCompanyUsers(Model.Company.Id) ?? new List<CompanyUser>();
                    Model.CompanyUsers = Model.CompanyUsers
                   .Where(user => !Model.CompanyUserPhoneNumbers.Any(email => email.CompanyUserId == user.Id))
                  .ToList();
                }
                if (Action == "Show Add StartSyncDate Google")
                {
                    Model.ShowAddStartSyncDate = true;
                    Model.ConnectGmailAccount = true;
                    Model.AllStaffAccess = true;
                    Model.CurrentTab = "Integrations";

                }
                if (Action == "Show Add StartSyncDate Microsoft")
                {
                    Model.ShowAddStartSyncDate = true;
                    Model.ConnectMicrosoftAccount = true;
                    Model.AllStaffAccess = true;
                    Model.CurrentTab = "Integrations";

                }
                if (Action == "Cancel Add other Account")
                {
                    Model.ShowAddOtherAccount = false;
                    Model.ShowAddStartSyncDate = false;
                    Model.ShowAddPhoneNumber = false;
                    Model.CurrentTab = "Integrations";
                }
                if (HttpContext.Session.GetString("MobileApp") != null)
                    Model.MobileApp = true;

                var HTML2 = Task.Run(() => viewRenderer.RenderViewToStringAsync("CompanyIntegration/PartialViews/Integration_Partial", Model)).Result;
                return Json(new { isValid = true, html = HTML2 });
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, ViewModel.AjaxUpdate, ViewModel.Action, System.Text.Json.JsonSerializer.Serialize(ViewModel));
            }
        }

        public ActionResult GoogleAuthentication()
        {
            string state = Guid.NewGuid().ToString("N");

            string redirectUri = Url.Action("GoogleCallback", "CompanyIntegration", null, HttpContext.Request.Scheme);
            string authUrl = GoogleAPI.GetAuthorizationUrl(redirectUri, state);

            return Redirect(authUrl);
        }

        public async Task<IActionResult> MicrosoftAuthentication()
        {
            try
            {
                string redirectUri = Url.Action("MicrosoftAuthCallback", "CompanyIntegration", null, HttpContext.Request.Scheme);
                //string redirectUri = "https://localhost:44390/CompanyProfile/MicrosoftAuthCallback";
                string authUrl = await MicrosoftAPI.GetAuthorizationUrl(redirectUri);

                return Redirect(authUrl);
            }
            catch (Exception ex)
            {
                var str = ex.InnerException.ToString();
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        public async Task<ActionResult> GoogleCallback(string code)
        {
            try
            {
                string redirectUri = Url.Action("GoogleCallback", "CompanyIntegration", null, HttpContext.Request.Scheme);
                var CompanyId = Int32.Parse(User.Claims.Where(x => x.Type == "CompanyId").FirstOrDefault().Value);
                var StoredDate = HttpContext.Session.GetString("LastSyncDate");
                DateTime LastSyncDate = DateTime.Parse(StoredDate);
                var AllStaffAccess = HttpContext.Session.GetInt32("AllStaffAccess");
                bool StaffAccess = false;

                // Get the authorization code flow and exchange the code for tokens
                var refreshtoken = await GoogleAPI.GetRefreshToken(code, redirectUri);

                if (refreshtoken != null)
                {
                    string accessToken = await GoogleAPI.GetAccessTokenFromRefreshTokenAsync(refreshtoken);
                    var userinfo = GoogleAPI.GetUserInfo(accessToken);
                    var IsAccountPresent = await CompanyIntegrationDL.CheckCompanyAccount(CompanyId, userinfo.Email);

                    if (IsAccountPresent)
                    {
                        await CompanyIntegrationDL.UpdateRefreshtoken(CompanyId, userinfo.Email, refreshtoken);
                    }
                    else
                    {
                        if (AllStaffAccess == 1)
                            StaffAccess = true;                        
                        else
                            StaffAccess = false;

                        await CompanyIntegrationDL.CreateCompanyEmailAccount(CompanyId, 1, userinfo.Name, userinfo.Email, LastSyncDate, refreshtoken,StaffAccess);
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error during authentication: " + ex.Message;
                return View("Error");
            }
        }

        public async Task<IActionResult> MicrosoftAuthCallback(string code)
        {
            if (code != null)
            {
                // Obtain tokens and other user information using code
                string redirectUri = Url.Action("MicrosoftAuthCallback", "CompanyIntegration", null, HttpContext.Request.Scheme);

                var refreshtoken = await MicrosoftAPI.GetAccessTokenAndRefreshFromCodeTokenAsync(code, redirectUri);
                var CompanyId = Int32.Parse(User.Claims.Where(x => x.Type == "CompanyId").FirstOrDefault().Value);

                var StoredDate = HttpContext.Session.GetString("LastSyncDate");
                DateTime LastSyncDate = DateTime.Parse(StoredDate);
                var AllStaffAccess = HttpContext.Session.GetInt32("AllStaffAccess");
                bool StaffAccess = false;
                if (refreshtoken != null)
                {
                    var accesstoken = await MicrosoftAPI.GetAccessTokenAsync(refreshtoken);
                    var userinfo = await MicrosoftAPI.GetUserEmailAsync(accesstoken);
                    var IsAccountPresent = await CompanyIntegrationDL.CheckCompanyAccount(CompanyId, userinfo.Email);
                    if (IsAccountPresent)
                    {
                        await CompanyIntegrationDL.UpdateRefreshtoken(CompanyId, userinfo.Email, refreshtoken);
                    }
                    else
                    {
                        if (AllStaffAccess == 1)
                           StaffAccess = true;
                        else
                           StaffAccess = false;

                        await CompanyIntegrationDL.CreateCompanyEmailAccount(CompanyId, 2, userinfo.Name, userinfo.Email, LastSyncDate, refreshtoken,StaffAccess);
                    }

                }

            }
            return RedirectToAction("Index");
        }

    }
}
