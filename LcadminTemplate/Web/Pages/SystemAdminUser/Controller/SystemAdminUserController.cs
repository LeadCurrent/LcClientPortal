using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Data;
using Web;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Project.Utilities;
using Microsoft.AspNetCore.Http;
using static Data.GeneralEnums;
using CommonClasses;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Scheduler.Controllers
{
    [Authorize(Policy = "SystemAdmin")]
    public class SystemAdminUserController : Controller
    {
        
        public UserManager<User> UserManager { get; }
        public SignInManager<User> SignInManager { get; }
        public UserDataLibrary UserDL { get; }
        public CompanyDataLibrary CompanyDL { get; }
        private readonly RazorViewToStringRenderer viewRenderer;


        public SystemAdminUserController(UserManager<User> userManager,
            SignInManager<User> signInManager,
            UserDataLibrary UserDataLibrary,
            CompanyDataLibrary CompanyDataLibrary,
            RazorViewToStringRenderer RazorViewToStringRenderer)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            UserDL = UserDataLibrary;
            CompanyDL = CompanyDataLibrary;
            viewRenderer = RazorViewToStringRenderer;
        }

        public async Task<UserVM> getUserListModel()
        {
            var UserVM = new UserVM();
            UserVM.Users = await UserDL.GetUsers();
         

            //Filters Start
            if (HttpContext.Session.GetString("FilterName") != null)
            {
                UserVM.FilterName = HttpContext.Session.GetString("FilterName");
                var FilterList = new List<User>();
                foreach (var user in UserVM.Users)
                    if (user.FirstName.ToUpper().Contains(UserVM.FilterName.ToUpper()))
                        FilterList.Add(user);
                UserVM.Users = FilterList;
            }     
            if (HttpContext.Session.GetInt32("UserFilterStatus") != null)
            {
                UserVM.FilterStatus = (Status)HttpContext.Session.GetInt32("UserFilterStatus").Value;
                UserVM.Users = UserVM.Users.Where(x => x.Status == UserVM.FilterStatus).ToList();
            }
            else
                UserVM.Users = UserVM.Users.Where(x => x.Status == Status.Active).ToList();
            //Filters End
            return UserVM;
        }

        [HttpGet]
        public async Task<IActionResult> Index(bool MobileApp)
        {
            var UserVM = await getUserListModel();
            if (HttpContext.Session.GetString("MobileApp") != null)
                UserVM.MobileApp = true;
            else if (MobileApp == true)
            {
                UserVM.MobileApp = MobileApp;
                HttpContext.Session.SetString("MobileApp", "True");
            }
            return View("UserList", UserVM);
        }

        [HttpPost]
        public async Task<IActionResult> List(UserVM ViewModel, string Action)
        {
           
            //Filters Start
            if (Action == "Apply Filters")
            {
                HttpContext.Session.SetInt32("UserFilterStatus", (int)ViewModel.FilterStatus);

                if (ViewModel.FilterName != null)
                    HttpContext.Session.SetString("FilterName", ViewModel.FilterName);
                else
                    HttpContext.Session.Remove("FilterName");
            }
            if (Action == "Clear Filters")
            {
                HttpContext.Session.SetInt32("UserFilterStatus", 0);
                HttpContext.Session.Remove("FilterDropdown");
                HttpContext.Session.Remove("FilterName");
            }
            //Filters End

            if (Action == "Create")
                return RedirectToAction("Create");

            if (Action == "Edit")
                return RedirectToAction("EditUser", new { Id = ViewModel.StringParam });

            var UserVM = await getUserListModel();

            var HTML = await viewRenderer.RenderViewToStringAsync("SystemAdminUser/PartialViews/UserList_Partial", UserVM);
            return Json(new { isValid = true, html = HTML });
        }

        [HttpGet]
        public async Task< IActionResult> Create()
        {
            var Model = new UserVM();
            Model.Companys = await CompanyDL.GetCompanys();
            Model.User = new User();
            Model.User.SystemAdmin = true;
            if (HttpContext.Session.GetString("MobileApp") != null)
                Model.MobileApp = true;
            return View("CreateUser", Model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserVM ViewModel, string Action)
        {
            var HTML = "";
            if (Action == "Cancel")
                return RedirectToAction("Index", "SystemAdminUser", "");

            if (Action == "Create")
            {
                ViewModel.User.Status = Status.Active;
                ViewModel.User.TemporaryPassword = true;
                if (ViewModel.Password != ViewModel.ConfirmPassword)
                {
                    ViewModel.ErrorMessage = "Passwords do not match";
                    HTML = await viewRenderer.RenderViewToStringAsync("SystemAdminUser/PartialViews/CreateUser_Partial", ViewModel);
                    return Json(new { isValid = true, html = HTML });
                }

                var NewUser = new User
                {
                    UserName = ViewModel.User.UserName,
                    FirstName = ViewModel.User.FirstName,
                    LastName = ViewModel.User.LastName,
                    Email = ViewModel.User.Email,
                    Status = Status.Active,
                    Developer = ViewModel.User.Developer,
                    SystemAdmin = ViewModel.User.SystemAdmin,
                    TemporaryPassword = true,
                    EmailConfirmed = true,
                    Phone = ViewModel.User.Phone,
                    SelectedCompanyId=ViewModel.User.SelectedCompanyId
                };

                var result = await UserManager.CreateAsync(NewUser, ViewModel.Password);
                if (!result.Succeeded)
                {
                    if (result.Errors.Any())
                        ViewModel.ErrorMessage = result.Errors.FirstOrDefault().Description;
                    else
                        ViewModel.ErrorMessage = "User was not successfully created";
                    HTML = await viewRenderer.RenderViewToStringAsync("SystemAdminUser/PartialViews/CreateUser_Partial", ViewModel);
                    return Json(new { isValid = true, html = HTML });
                }
                else
                {
                    if (ViewModel.User.Developer)
                        await UserManager.AddClaimAsync(NewUser, new Claim("Developer", "True"));

                    if (ViewModel.User.SystemAdmin)
                        await UserManager.AddClaimAsync(NewUser, new Claim("SystemAdmin", "True"));

                    var claimsPrincipal = await SignInManager.CreateUserPrincipalAsync(NewUser);
                    ViewModel.CreateSuccessful = true;
                    HTML = await viewRenderer.RenderViewToStringAsync("SystemAdminUser/PartialViews/CreateUser_Partial", ViewModel);
                    return Json(new { isValid = true, html = HTML });
                }
            }
            HTML = await viewRenderer.RenderViewToStringAsync("SystemAdminUser/PartialViews/CreateUser_Partial", ViewModel);
            return Json(new { isValid = true, html = HTML });
        }


        [HttpGet]
        public async Task<IActionResult> EditUser(string Id)
        {
            var VM = new UserVM();
            VM.User = await UserDL.GetUser(Id);
            VM.Companys = await CompanyDL.GetCompanys();
            if (HttpContext.Session.GetString("MobileApp") != null)
                VM.MobileApp = true;
            return View("EditUser", VM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserVM ViewModel, string Action)
        {
            if (Action == "Save Changes")
            {
                var user = await UserManager.FindByIdAsync(ViewModel.User.Id);

                user.UserName = ViewModel.User.UserName;
                user.FirstName = ViewModel.User.FirstName;
                user.LastName = ViewModel.User.LastName;
                user.Email = ViewModel.User.Email;
                user.Developer = ViewModel.User.Developer;
                user.SystemAdmin = ViewModel.User.SystemAdmin;
                user.Status = ViewModel.User.Status;
                user.Phone = ViewModel.User.Phone;
                user.SelectedCompanyId= ViewModel.User.SelectedCompanyId;   

                var claims = await UserManager.GetClaimsAsync(user);
                var DeveloperClaim = claims.Where(c => c.Type == "Developer").FirstOrDefault();
                var SystemAdminClaim = claims.Where(c => c.Type == "SystemAdmin").FirstOrDefault();
                if (user.Developer)
                {
                    if (DeveloperClaim == null)
                        await UserManager.AddClaimAsync(user, new Claim("Developer", "True"));
                }
                else
                {
                    if (DeveloperClaim != null)
                        await UserManager.RemoveClaimAsync(user, DeveloperClaim);
                }

                if (user.SystemAdmin)
                {
                    if (SystemAdminClaim == null)
                        await UserManager.AddClaimAsync(user, new Claim("SystemAdmin", "True"));
                }
                else
                {
                    if (SystemAdminClaim != null)
                        await UserManager.RemoveClaimAsync(user, SystemAdminClaim);
                }

                await UserManager.UpdateAsync(user);

                ViewModel.UpdateSuccessful = true;
            }
          
            if (Action == "Cancel")
            {
                return RedirectToAction("Index");
            }

            var VM = new UserVM();
            VM.User = await UserDL.GetUser(ViewModel.User.Id);
            VM.UpdateSuccessful = ViewModel.UpdateSuccessful;

            var HTML = await viewRenderer.RenderViewToStringAsync("SystemAdminUser/PartialViews/EditUser_Partial", VM);
            return Json(new { isValid = true, html = HTML });
        }

        
    }
}
