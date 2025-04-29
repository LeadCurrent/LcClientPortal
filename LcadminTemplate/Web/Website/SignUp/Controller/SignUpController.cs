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
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using CommonClasses;


namespace Web
{

    public class SignUpController : Controller
    {
        
        private readonly RazorViewToStringRenderer viewRenderer;
        Email Emailer { get; }
        public UserManager<User> UserManager { get; }
        public UserDataLibrary UserDL { get; }
        public CompanyRolesDataLibrary CompanyRolesDL { get; }
        public SignInManager<User> SignInManager { get; }
        public CompanyDataLibrary CompanyDL { get; }
        public ExceptionLogger exceptionLogger { get; }

        public SignUpController(
            Email Email,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            UserDataLibrary UserDataLibrary,
            RazorViewToStringRenderer RazorViewToStringRenderer,
            CompanyDataLibrary CompanyDataLibrary,
            CompanyRolesDataLibrary CompanyRolesDataLibrary,
            ExceptionLogger _exceptionLogger
            )
        {
            Emailer = Email;
            UserManager = userManager;
            SignInManager = signInManager;
            CompanyDL = CompanyDataLibrary;
            viewRenderer = RazorViewToStringRenderer;
            UserDL = UserDataLibrary;
            exceptionLogger = _exceptionLogger;
            CompanyRolesDL = CompanyRolesDataLibrary;
        }

        [HttpGet]
        public ActionResult Index(string message)
        {
            ViewBag.Message = message;
            return View("Register");
        }

        [HttpPost]
        public async Task<IActionResult> Register(SignUpVM ViewModel, string Action)
         {
            try
            {
                if (Action == "Create")
                {
                    var IsUserExists = await UserDL.GetUserbyUserName(ViewModel.SignupEmail);
                    if (IsUserExists == null)
                    {
                        var NewUser = new User
                        {
                            UserName = ViewModel.SignupEmail,                           
                            Email = ViewModel.SignupEmail,
                            Phone = ViewModel.SignupPhone,
                            TemporaryPassword = true,
                            EmailConfirmed = true,
                        };
                        var Password = "LeadCurrnet123";
                        var result = await UserManager.CreateAsync(NewUser, Password);
                        if (result.Succeeded)
                        {
                            //company
                            var Company = await CompanyDL.CreateCompany(ViewModel.Company, NewUser);
                            var CompanyUserId = await CompanyDL.CreateCompanyUser(Company.Id, NewUser.Id);

                            //Role
                            var AdminRole = await CompanyRolesDL.CreateRole("Admin", Company.Id);
                            var CompanyUserRole = await CompanyRolesDL.CreateCompanyUserRole(CompanyUserId, AdminRole.Id);

                            //claims
                            await UserDL.AddUserClaimAsync(NewUser.Id, "CompanyId", Company.Id.ToString());
                            await UserDL.AddUserClaimAsync(NewUser.Id, "Plan", "Default");
                            await UserDL.AddUserClaimAsync(NewUser.Id, "Admin", "True");
                            foreach (var p in AdminRole.RolePermissions)
                                await UserDL.AddUserClaimAsync(NewUser.Id, p.Permission.ToString(), AdminRole.Id.ToString());
                            var claimsPrincipal = await SignInManager.CreateUserPrincipalAsync(NewUser);

                            //SignIn
                            await SignInManager.PasswordSignInAsync(ViewModel.SignupEmail, "LeadCurrent123", false, false);

                            var Email = new EmailTemplate();
                            Email.Subject = "Lead Current - Email Verification";
                            Email.ButtonURL = CommonClasses.Environment.url() + "Account/Welcome?Id=" + NewUser.Id;
                            Email.ButtonText = "Verify Email";
                            Email.CompanyName = "Lead Current";
                            Email.CompanyEmail = "support@LeadCurrent.com";
                            Email.Company = new Company();
                            Email.EmailBody = Task.Run(() => viewRenderer.RenderViewToStringAsync("Templates/EmailTemplate/Partials/CompanyOnBoardingVerification_Partial", Email)).Result;
                            var EmailHTML = Task.Run(() => viewRenderer.RenderViewToStringAsync("Templates/EmailTemplate/SystemAdminEmailTemplate", Email)).Result;
                            var EmailTo = NewUser.Email;
                            var EmailToName = NewUser.FullName;
                            await Emailer.SendMail();

                            return RedirectToAction("WaitingEmailVerification", "Account");
                        }
                    }
                    else
                    {
                        return RedirectToAction("Index", "SignUp", new { message = "Email already in use. Please choose another email address." });

                    }
                }
                    
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, ViewModel.AjaxUpdate, ViewModel.Action, System.Text.Json.JsonSerializer.Serialize(ViewModel));
            }
        }

        [HttpGet]
        public ActionResult ContactUs()
        {
            return View();
        }

        //[HttpPost]
        //public async Task<IActionResult> ContactUs(SignUpVM ViewModel, string Action)
        //{
        //    if (Action == "Send Feedback To Admin")
        //    {
        //        await CompanyDL.AddDetailsToContactUs(ViewModel.ContactUs, User.Identity.Name);
        //        var CompanyId = Int32.Parse(User.Claims.Where(x => x.Type == "CompanyId").FirstOrDefault().Value);
        //        var Company = await CompanyDL.GetCompany(CompanyId);

        //        if (ViewModel.ContactUs != null)
        //        {
        //            var SaleContact = new SaleContact
        //            {
        //                Name = ViewModel.ContactUs.FullName,
        //                Email = ViewModel.ContactUs.Email,
        //                PhoneNumber = ViewModel.ContactUs.Phone,
        //            };

        //            var SalesCompany = new SaleCompany
        //            {
        //                CompanyName = ViewModel.ContactUs.CompanyName,
        //                SaleContacts = new List<SaleContact> { SaleContact}
        //            };

        //            await SaleCompanyDL.CreateSaleCompany(SalesCompany);


        //            Emailer.Subject = "Contact from " + ViewModel.ContactUs.CompanyName;
        //            Emailer.Message = "<b>Full Name: </b>" + ViewModel.ContactUs.FullName + "<br>";
        //            Emailer.Message += "<b>Email: </b>" + ViewModel.ContactUs.Email + "<br>";
        //            Emailer.Message += "<b>Phone: </b>" + ViewModel.ContactUs.Phone + "<br>";
        //            Emailer.Message += "<b>Company Name: </b>" + ViewModel.ContactUs.CompanyName + "<br>";
        //            Emailer.Message += "<b>Message: </b>" + ViewModel.ContactUs.Message + "<br>";
        //            Emailer.EmailTo = "contact@proworkflow.app";
        //            Emailer.EmailFrom = "james@proworkflow.app";
        //            Emailer.EmailFromDispaly = "Pro Workflow Contact";
        //            await Emailer.SendMail();

        //        }
        //    }

        //    return RedirectToAction("Login", "Account");
        //}

    }

}

