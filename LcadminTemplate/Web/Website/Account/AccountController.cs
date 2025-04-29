using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Data;
using Web;
using Microsoft.AspNetCore.Identity;
using Project.Utilities;
using System.Linq;
using CommonClasses;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;



// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Scheduler.Controllers
{

    public class AccountController : Controller
    {
        
        public UserManager<User> UserManager { get; }
        public SignInManager<User> SignInManager { get; }
        public UserDataLibrary UserDL { get; }
        public CompanyDataLibrary CompanyDL { get; }
        public Email email { get; }
        public RazorViewToStringRenderer viewRenderer;
        public ExceptionLogger exceptionLogger { get; }


        public AccountController(
            Email Email,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            UserDataLibrary UserDataLibrary,
            CompanyDataLibrary CompanyDataLibrary,
            RazorViewToStringRenderer RazorViewToStringRenderer,
            ExceptionLogger _exceptionLogger)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            UserDL = UserDataLibrary;
            CompanyDL = CompanyDataLibrary;
            viewRenderer = RazorViewToStringRenderer;
            exceptionLogger = _exceptionLogger;
            email = Email;
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await SignInManager.SignOutAsync();
            return RedirectToAction("Login", "Account", null);
        }


        [HttpGet]
        public async Task<IActionResult> Login(bool InvalidLogin = false)
        {

            int userCheck = await UserDL.HasAnyUsersAsync();//to check first login

            if (userCheck == 0)
            {
                return RedirectToAction("CreateUserFirstTime", "SystemAdminFIrstTimeLogin"); 
            }


            var UserVM = new UserVM
            {
                InvalidLogin = InvalidLogin
            };

            
            return View("Login", UserVM);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            try
            {


                var user = await UserDL.GetUserbyUserName(User.Identity.Name);
                user = await UserDL.GetUserWithCompany(user.Id);
                user.LastLoginDate = StringFormating.CurrentTime();
                await UserDL.UpdateUser(user);
                UserDL.ClearChangeTracker();
                var Claims = await UserManager.GetClaimsAsync(user);
                var Company = new Company();
                var CustomerClaim = Claims.FirstOrDefault(x => x.Type == "Contact");
                var VendorClaim = Claims.FirstOrDefault(x => x.Type == "VendorContact");
                if (Claims.Where(c => c.Type == "CompanyId").FirstOrDefault() != null)
                {
                    var CompanyID = Int32.Parse(Claims.Where(c => c.Type == "CompanyId").FirstOrDefault().Value);
                    if (user.CompanyUsers.Any())
                    {
                        var CompanyClaim = Claims.Where(c => c.Type == "CompanyId").FirstOrDefault();
                        var CompanyNameClaim = Claims.Where(x => x.Type == "CompanyName").FirstOrDefault();
                        var CompanyLogoClaims = Claims.Where(x => x.Type == "CompanyLogo").FirstOrDefault();
                        var CompanyUserCompany = user.CompanyUsers.FirstOrDefault().Company;
                        if (CompanyUserCompany.Id != CompanyID)
                        {
                            await UserManager.RemoveClaimAsync(user, CompanyClaim);
                            await UserManager.RemoveClaimAsync(user, CompanyNameClaim);
                            await UserManager.RemoveClaimAsync(user, CompanyLogoClaims);
                            await UserManager.AddClaimAsync(user, new Claim("CompanyId", CompanyUserCompany.Id.ToString()));
                            await UserManager.AddClaimAsync(user, new Claim("CompanyLogo", CompanyUserCompany.Logo));
                            await UserManager.AddClaimAsync(user, new Claim("CompanyName", CompanyUserCompany.Name));
                            CompanyID = CompanyUserCompany.Id;
                            await SignInManager.SignOutAsync();
                            await SignInManager.SignInAsync(user, true);
                        }
                    }
                    Company = await CompanyDL.GetCompany(CompanyID);
                }
                else if (user.CompanyUsers.Any())
                {
                    var CompanyUserCompany = user.CompanyUsers.FirstOrDefault().Company;
                    var CompanyClaim = Claims.Where(c => c.Type == "CompanyId").FirstOrDefault();
                    var CompanyNameClaim = Claims.Where(x => x.Type == "CompanyName").FirstOrDefault();
                    var CompanyLogoClaims = Claims.Where(x => x.Type == "CompanyLogo").FirstOrDefault();

                    if (CompanyClaim != null)
                        await UserManager.RemoveClaimAsync(user, CompanyClaim);
                    if (CompanyNameClaim != null)
                        await UserManager.RemoveClaimAsync(user, CompanyNameClaim);
                    if (CompanyLogoClaims != null)
                        await UserManager.RemoveClaimAsync(user, CompanyLogoClaims);

                    await UserManager.AddClaimAsync(user, new Claim("CompanyId", CompanyUserCompany.Id.ToString()));
                    await UserManager.AddClaimAsync(user, new Claim("CompanyLogo", CompanyUserCompany.Logo));
                    await UserManager.AddClaimAsync(user, new Claim("CompanyName", CompanyUserCompany.Name));
                    await SignInManager.SignOutAsync();
                    await SignInManager.SignInAsync(user, true);


                    Company = user.CompanyUsers.FirstOrDefault().Company;
                }

                if (user.TemporaryPassword)
                {
                    await SignInManager.SignOutAsync();
                    return RedirectToAction("ChangePassword", new { UserId = user.Id });
                }
                if (CommonClasses.Environment.environment.Contains("LongRange"))
                {
                    return RedirectToAction("NewOrder", "PointOfSale");
                }
                if (CustomerClaim != null)
                {
                    return RedirectToAction("Index", "CustomerProfile");
                }
                if (VendorClaim != null)
                {
                    return RedirectToAction("Index", "VendorProfile");
                }
                                
               
                return RedirectToAction("Index", "Customer", null);
            }
            catch (Exception ex)
            {
                var str = ex.InnerException;
                return RedirectToAction("Login", "Account", null);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserVM model, string Login, string ResetPassword)
        {
            if (model.UserName != null)
            {
                try
                {
                    var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
                    var userAgent = Request.Headers["User-Agent"].ToString();
                    if (result.Succeeded)
                    {
                        var user = await UserDL.GetUserbyUserName(model.UserName);
                        user = await UserDL.GetUserWithCompany(user.Id);
                        user.LastLoginDate = StringFormating.CurrentTime();
                        await UserDL.SetUserLoginDetaills(user, userAgent);
                        await UserDL.UpdateUser(user);
                        UserDL.ClearChangeTracker();
                        var Claims = await UserManager.GetClaimsAsync(user);
                        var Company = new Company();
                        var CustomerClaim = Claims.FirstOrDefault(x => x.Type == "Contact");
                        var VendorClaim = Claims.FirstOrDefault(x => x.Type == "VendorContact");
                        var NewUser = await UserDL.GetUser(user.Id);
                        if (Claims.Where(c => c.Type == "CompanyId").FirstOrDefault() != null)
                        {
                            var CompanyID = Int32.Parse(Claims.Where(c => c.Type == "CompanyId").FirstOrDefault().Value);
                            if (user.CompanyUsers.Any())
                            {
                                var CompanyClaim = Claims.Where(c => c.Type == "CompanyId").FirstOrDefault();
                                var CompanyNameClaim = Claims.Where(x => x.Type == "CompanyName").FirstOrDefault();
                                var CompanyLogoClaims = Claims.Where(x => x.Type == "CompanyLogo").FirstOrDefault();
                                var CompanyUserCompany = user.CompanyUsers.FirstOrDefault().Company;
                                if (CompanyUserCompany.Id != CompanyID)
                                {
                                    await UserManager.RemoveClaimAsync(NewUser, CompanyClaim);
                                    await UserManager.RemoveClaimAsync(NewUser, CompanyNameClaim);
                                    await UserManager.RemoveClaimAsync(NewUser, CompanyLogoClaims);
                                    await UserManager.AddClaimAsync(NewUser, new Claim("CompanyId", CompanyUserCompany.Id.ToString()));
                                    await UserManager.AddClaimAsync(NewUser, new Claim("CompanyLogo", CompanyUserCompany.Logo));
                                    await UserManager.AddClaimAsync(NewUser, new Claim("CompanyName", CompanyUserCompany.Name));
                                    CompanyID = CompanyUserCompany.Id;
                                    await SignInManager.SignOutAsync();
                                    await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
                                }
                            }
                            Company = await CompanyDL.GetCompany(CompanyID);
                        }
                        else if (user.CompanyUsers.Any())
                        {
                            var CompanyUserCompany = user.CompanyUsers.FirstOrDefault().Company;
                            var CompanyClaim = Claims.Where(c => c.Type == "CompanyId").FirstOrDefault();
                            var CompanyNameClaim = Claims.Where(x => x.Type == "CompanyName").FirstOrDefault();
                            var CompanyLogoClaims = Claims.Where(x => x.Type == "CompanyLogo").FirstOrDefault();

                            if (CompanyClaim != null)
                                await UserManager.RemoveClaimAsync(NewUser, CompanyClaim);
                            if (CompanyNameClaim != null)
                                await UserManager.RemoveClaimAsync(NewUser, CompanyNameClaim);
                            if (CompanyLogoClaims != null)
                                await UserManager.RemoveClaimAsync(NewUser, CompanyLogoClaims);

                            await UserManager.AddClaimAsync(NewUser, new Claim("CompanyId", CompanyUserCompany.Id.ToString()));
                            await UserManager.AddClaimAsync(NewUser, new Claim("CompanyLogo", CompanyUserCompany.Logo));
                            await UserManager.AddClaimAsync(NewUser, new Claim("CompanyName", CompanyUserCompany.Name));
                            await SignInManager.SignOutAsync();
                            await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);


                            Company = user.CompanyUsers.FirstOrDefault().Company;
                        }

                        if (user.TemporaryPassword)
                        {
                            await SignInManager.SignOutAsync();
                            return RedirectToAction("ChangePassword", new { UserId = user.Id });
                        }
                        if (CommonClasses.Environment.environment.Contains("LongRange"))
                        {
                            return RedirectToAction("NewOrder", "PointOfSale");
                        }
                        if (CustomerClaim != null)
                        {
                            return RedirectToAction("Index", "CustomerProfile");
                        }
                        if (VendorClaim != null)
                        {
                            return RedirectToAction("Index", "VendorProfile");
                        }
                        bool isSystemAdmin = Claims.Any(c => c.Type == "SystemAdmin" && c.Value == "True");

                        if (isSystemAdmin && Claims.Count == 1)
                        {
                            return RedirectToAction("Create", "Company");
                        }
                        return RedirectToAction("Index", "Customer", null);

                    }
                }
                catch (Exception ex)
                {
                    var exception = ex.InnerException;
                }

                return RedirectToAction("Login", "Account", new { InvalidLogin = true });

            }

            if (ResetPassword != null)
            {
                return RedirectToAction("ResetPassword");
            }

            return View(model);

        }

        [HttpGet]
        public IActionResult WaitingEmailVerification()
        {
            return View("WaitingEmailVerification");
        }

        [HttpGet]
        public async Task<IActionResult> Welcome(string Id)
        {
            try
            {
                var user = await UserDL.GetUser(Id);
                var cp = new UserVM
                {
                    Id = user.Id,
                    UserName = user.UserName
                };
                return View("Welcome", cp);
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, false);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword(string UserId)
        {
            var user = await UserDL.GetUser(UserId);
            var cp = new UserVM
            {
                Id = user.Id,
                UserName = user.UserName
            };
            return View("ChangePassword", cp);

        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(UserVM model, string ChangePassword)
        {
            if (ChangePassword != null)
            {
                if (ModelState.IsValid)
                {
                    var user = await UserDL.GetUser(model.Id);

                    if (user == null)
                    {
                        return NotFound();
                    }
                    user.PasswordHash = UserManager.PasswordHasher.HashPassword(user, model.Password);
                    var result = await UserManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        await UserDL.SetTemporaryPassword(model.Id, false);
                        await SignInManager.PasswordSignInAsync(user.UserName, model.Password, true, false);
                        var Claims = await UserManager.GetClaimsAsync(user);
                        var CustomerClaim = Claims.FirstOrDefault(x => x.Type == "Contact");
                        var VendorClaim = Claims.FirstOrDefault(x => x.Type == "VendorContact");
                        if (CustomerClaim != null)
                        {
                            return RedirectToAction("Index", "CustomerProfile");
                        }
                        if (VendorClaim != null)
                        {
                            return RedirectToAction("Index", "VendorProfile");
                        }
                        bool isSystemAdmin = Claims.Any(c => c.Type == "SystemAdmin" && c.Value == "True");

                        if (isSystemAdmin && Claims.Count == 1)
                        {
                            return RedirectToAction("Create", "Company");
                        }
                    }
                }
                else
                {
                    return View(model);
                }
            }
            return RedirectToAction("Index", "Customer");


        }

        [HttpGet]
        public IActionResult ResetPassword(bool PasswordSentViaText = false, bool PasswordSentViaEmail = false, bool NoMatchEmail = false, bool NoMatchText = false)
        {
            var model = new UserVM();
            model.PasswordSentViaText = PasswordSentViaText;
            model.PasswordSentViaEmail = PasswordSentViaEmail;
            model.NoMatchEmail = NoMatchEmail;
            model.NoMatchText = NoMatchText;
            return View("ResetPassword", model);

        }

        [HttpPost]

        public async Task<IActionResult> ResetPassword(UserVM model, string ResetPassword)
        {
            if (ResetPassword != null)
            {
                var user = await UserDL.GetUserByEmail(model.Email);
                if (user != null)
                {
                    var newpassword = CreatePassword();
                    user.PasswordHash = UserManager.PasswordHasher.HashPassword(user, newpassword);
                    user.TemporaryPassword = true;
                    var result = await UserManager.UpdateAsync(user);

                    email.Subject = "Your Temporary Password";
                    email.Message = "Your login is " + user.UserName + " Your temporary password is " + newpassword;
                    email.EmailTo = user.Email;
                    email.EmailFrom = CommonClasses.Environment.SiteEmail;

                    await email.SendMail();

                    return RedirectToAction("ResetPassword", new { PasswordSentViaEmail = true });
                }
                else
                {
                    return RedirectToAction("ResetPassword", new { NoMatchEmail = true });
                }
            }

            return RedirectToAction("Login");

        }

        static Random rd = new Random();
        internal static string CreatePassword()
        {
            const string Letters = "ABCDEFGHJKLMNOPQRSTUVWXYZ";
            const string numbers = "0123456789";
            char[] chars = new char[6];

            for (int i = 0; i < 5; i++)
            {
                chars[i] = Letters[rd.Next(0, Letters.Length)];
            }

            chars[5] = numbers[rd.Next(0, numbers.Length)];

            return new string(chars);
        }

    }

}

