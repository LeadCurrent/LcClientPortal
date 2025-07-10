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

namespace Scheduler.Controllers
{
    public class SystemAdminFirstTimeLogin : Controller
    {
        public UserManager<User> UserManager { get; }
        public SignInManager<User> SignInManager { get; }
        public UserDataLibrary UserDL { get; }
        private readonly RazorViewToStringRenderer viewRenderer;


        public SystemAdminFirstTimeLogin(UserManager<User> userManager,
            SignInManager<User> signInManager,
            UserDataLibrary UserDataLibrary,
            RazorViewToStringRenderer RazorViewToStringRenderer)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            UserDL = UserDataLibrary;

            viewRenderer = RazorViewToStringRenderer;
        }


        [HttpGet]
        public IActionResult CreateUserFirstTime()
        {
            var Model = new UserVM();
            Model.User = new User();
            Model.User.SystemAdmin = true;
            if (HttpContext.Session.GetString("MobileApp") != null)
                Model.MobileApp = true;
            return View("CreateUserFirstTime", Model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserFirstTime(UserVM ViewModel, string Action)
        {
            var HTML = "";

            if (Action == "CreateUserFirstTime")
            {
                ViewModel.User.Status = Status.Active;
                ViewModel.User.TemporaryPassword = true;
                if (ViewModel.Password != ViewModel.ConfirmPassword)
                {
                    ViewModel.ErrorMessage = "Passwords do not match";
                    HTML = await viewRenderer.RenderViewToStringAsync("SystemAdminFirstTimeLogin/CreateUser_Partial", ViewModel);
                    return Json(new { isValid = true, html = HTML });
                }

                var NewUser = new User
                {
                    UserName = ViewModel.User.UserName,
                    FirstName = ViewModel.User.FirstName,
                    LastName = ViewModel.User.LastName,
                    Email = ViewModel.User.Email,
                    Status = Status.Active,
                    SystemAdmin = ViewModel.User.SystemAdmin,
                    TemporaryPassword = true,
                    EmailConfirmed = true,
                    Phone = ViewModel.User.Phone,
                };

                var result = await UserManager.CreateAsync(NewUser, ViewModel.Password);
                if (!result.Succeeded)
                {
                    if (result.Errors.Any())
                        ViewModel.ErrorMessage = result.Errors.FirstOrDefault().Description;
                    else
                        ViewModel.ErrorMessage = "User was not successfully created";
                    HTML = await viewRenderer.RenderViewToStringAsync("SystemAdminFirstTimeLogin/CreateUserFirstTime", ViewModel);
                    return Json(new { isValid = true, html = HTML });
                }
                else
                {
                    if (ViewModel.User.SystemAdmin)
                        await UserManager.AddClaimAsync(NewUser, new Claim("SystemAdmin", "True"));

                    var claimsPrincipal = await SignInManager.CreateUserPrincipalAsync(NewUser);
                    return RedirectToAction("Login", "Account");
                }
            }
            HTML = await viewRenderer.RenderViewToStringAsync("SystemAdminFirstTimeLogin/CreateUserFirstTime", ViewModel);
            return Json(new { isValid = true, html = HTML });
        }
    }
}
