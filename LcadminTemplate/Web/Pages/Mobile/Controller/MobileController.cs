using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Data;
using Project.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using static Data.RoleEnums;
using CommonClasses;

namespace Web.Controllers
{

    public class MobileController : Controller
    {
        
        public UserManager<User> UserManager { get; }

        RazorViewToStringRenderer viewRenderer;
        ExceptionLogger exceptionLogger { get; }
        public SignInManager<User> SignInManager { get; }
        public UserDataLibrary UserDL { get; }
        public CompanyDataLibrary CompanyDL { get; }
        public CompanyRolesDataLibrary CompanyRolesDL { get; }

        public MobileController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RazorViewToStringRenderer RazorViewToStringRenderer,
            ExceptionLogger ExceptionLogger,
            UserDataLibrary UserDataLibrary,
            CompanyDataLibrary CompanyDataLibrary,
            CompanyRolesDataLibrary CompanyRolesDataLibrary
        )
        {
            UserManager = userManager;
            UserDL = UserDataLibrary;
            SignInManager = signInManager;
            exceptionLogger = ExceptionLogger;
            viewRenderer = RazorViewToStringRenderer;
            CompanyDL = CompanyDataLibrary;
            CompanyRolesDL = CompanyRolesDataLibrary;
        }

        public async Task<UserData> getUser(string UserName, string Password)
        {
            var UD = new UserData();
            var result = await SignInManager.PasswordSignInAsync(UserName, Password, false, false);
            if (!result.Succeeded)
            {
                return UD;
            }
            else
            {
                var user = await UserDL.SetAuthorizationCode(UserName);
                await SignInManager.SignInAsync(user, true);
                if (user != null)
                {
                    UD.Valid = true;
                    UD.Name = user.FullName;
                    UD.UserName = UserName;
                    UD.AuthorizationCode = user.MobileAuthorizationCode;
                    var claims = await UserManager.GetClaimsAsync(user);
                    if (claims.Where(x => x.Type == "SystemAdmin").Any())
                        UD.SystemAdmin = true;
                    //if (claims.Where(x => x.Type == "JobDetailsOnly").Any())
                    //    UD.JobDetailsOnly = true;

                    var companyId = Int32.Parse(User.Claims.Where(x => x.Type == "CompanyId").FirstOrDefault().Value);
                    var company = await CompanyDL.GetBasicCompany(companyId);
                    UD.CompanyName = company.Name;
                    UD.Logo = company.Logo;
                    var companyUser = await UserDL.GetUsersForCompany(companyId);

                    var compUser = companyUser.FirstOrDefault(x => x.UserId == user.Id && x.CompanyId == companyId);
                    if (compUser != null)
                    {
                        var CompanyUserRole = await CompanyRolesDL.GetCompanyUserRoleByCompanyUserId(compUser.Id);
                        if (CompanyUserRole?.Role?.RolePermissions != null)
                        {
                            //var userDataProperties = typeof(UserData).GetProperties();
                            var viewPermissions = CompanyUserRole.Role.RolePermissions
                                .Where(rp => rp.Access == Access.ViewOnly || rp.Access == Access.EditAndView
                                || rp.JobAccess == JobAccess.JobDetailsOnly || rp.JobAccess == JobAccess.JobDetailsQuoteAndInvoice)
                                .ToList();

                            foreach (var Perm in viewPermissions)
                                Perm.SortOrder = PermissionSort(Perm.Permission);

                            viewPermissions = viewPermissions.OrderBy(x => x.SortOrder).ToList();

                            UD.SideNav = new SideNav();
                            UD.BottomNav = new BottomNav();
                            UD.QuickTasks = new QuickTasks();
                            UD.SideNav.Pages = new List<Page>();
                            UD.BottomNav.Pages = new List<Page>();
                            UD.QuickTasks.Pages = new List<Page>();
                            

                            

                            foreach (var prop in viewPermissions)
                            {
                                if (prop.Permission == Permission.Customers ||
                                    prop.Permission == Permission.Email ||
                                    prop.Permission == Permission.Documents)
                                {
                                    var Page = new Page
                                    {
                                        DisplayName = PermissionDesc(prop.Permission),
                                        PageUrl = CommonClasses.Environment.url() + "Mobile/LoadPage?Page=" + prop.Permission.ToString() + "&AuthorizationCode=" + user.MobileAuthorizationCode
                                    };
                                    UD.SideNav.Pages.Add(Page);
                                }

                                if (prop.Permission == Permission.CompanyProfile ||
                                 prop.Permission == Permission.Integration ||
                                   prop.Permission == Permission.CompanyRoles ||
                                 prop.Permission == Permission.CompanyAccount ||
                                 prop.Permission == Permission.ContactForm ||
                                 prop.Permission == Permission.Users)
                                {
                                    var page = UD.SideNav.Pages.Where(x => x.DisplayName == "Company Admin").FirstOrDefault();
                                    if (page == null)
                                    {
                                        page = new Page
                                        {
                                            DisplayName = "Company Admin",
                                            SubPages = new List<SubPage>()
                                        };
                                        UD.SideNav.Pages.Add(page);
                                    }
                                    var SubPage = new SubPage
                                    {
                                        DisplayName = PermissionDesc(prop.Permission),
                                        PageUrl = CommonClasses.Environment.url() + "Mobile/LoadPage?Page=" + prop.Permission.ToString() + "&AuthorizationCode=" + user.MobileAuthorizationCode
                                    };
                                    page.SubPages.Add(SubPage);
                                }

                                if (prop.Permission == Permission.Customers ||
                                    prop.Permission == Permission.Email)
                                {
                                    var Page = new Page
                                    {
                                        DisplayName = PermissionDesc(prop.Permission),
                                        PageUrl = CommonClasses.Environment.url() + "Mobile/LoadPage?Page=" + prop.Permission.ToString() + "&AuthorizationCode=" + user.MobileAuthorizationCode
                                    };
                                    UD.BottomNav.Pages.Add(Page);
                                }

                                if (prop.Permission == Permission.Customers)
                                {
                                    var Page = new Page
                                    {
                                        DisplayName = "Create Customer",
                                        PageUrl = CommonClasses.Environment.url() + "Mobile/LoadPage?Page=" + prop.Permission.ToString() + "&AuthorizationCode=" + user.MobileAuthorizationCode
                                    };
                                    UD.QuickTasks.Pages.Add(Page);
                                }
                            }
                        }
                    }
                }
                return UD;
            }
        }

        public async Task<UserData> ForgotPassword(string PhoneNumber)
        {
            var UD = new UserData();
            var result = await UserDL.SetForgotPasswordCode(PhoneNumber);
            if (result != "Invalid")
            {
                var user = await UserDL.GetUserByPhone(PhoneNumber);
                user = await UserDL.SetAuthorizationCode(user.UserName);
                if (user != null)
                {
                    UD.Valid = true;
                    UD.Name = user.FullName;
                    UD.UserName = user.UserName;
                    UD.AuthorizationCode = user.MobileAuthorizationCode;
                    UD.ResetPasswordCode = user.ForgotPasswordCode;
                    return UD;
                }
            }
            return UD;
        }

        public async Task<String> ResetPassword(string AuthorizationCode, string Password)
        {
            var UD = new UserData();
            var user = await UserDL.GetUserFromAuthorizationCode(AuthorizationCode);
            if (user != null)
            {
                user.PasswordHash = UserManager.PasswordHasher.HashPassword(user, Password);
                var result = await UserManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    await UserDL.SetTemporaryPassword(user.Id, false);
                    return "Success";
                }
            }

            return "Invalid";
        }

        public async Task CheckLogin(string AuthorizationCode)
        {
            if (HttpContext.Session.GetString("login") == null)
            {
                var user = await UserDL.GetUserFromAuthorizationCode(AuthorizationCode);
                await SignInManager.SignInAsync(user, true);
                HttpContext.Session.SetString("login", "true");
            }
            if (!User.Identity.IsAuthenticated)
            {
                var user = await UserDL.GetUserFromAuthorizationCode(AuthorizationCode);
                await SignInManager.SignInAsync(user, true);
            }
        }

        [HttpGet]
        public async Task<IActionResult> LoadPage(string Page, string AuthorizationCode)
        {
            try
            {
                await CheckLogin(AuthorizationCode);
                if (Page == "Customers") return RedirectToAction("Index", "Customer", new { MobileApp = true });
                if (Page == "Leads") return RedirectToAction("Index", "Leads", new { MobileApp = true });
                if (Page == "Vendors") return RedirectToAction("Index", "Vendor", new { MobileApp = true });
                if (Page == "Contacts") return RedirectToAction("Index", "Contact", new { MobileApp = true });
                if (Page == "Email") return RedirectToAction("Index", "UserEmail", new { MobileApp = true });
                if (Page == "Messages") return RedirectToAction("Index", "UserMessage", new { MobileApp = true });
                if (Page == "Events") return RedirectToAction("Index", "Event", new { MobileApp = true });
                if (Page == "Communications") return RedirectToAction("Index", "Message", new { MobileApp = true });
                if (Page == "CommunicationTemplates") return RedirectToAction("Index", "CommunicationTemplate", new { MobileApp = true });
                if (Page == "Campaigns") return RedirectToAction("Index", "Campain", new { MobileApp = true });
                if (Page == "EmailFooter") return RedirectToAction("Index", "EmailFooter", new { MobileApp = true });
                if (Page == "EmailHeader") return RedirectToAction("Index", "EmailHeader", new { MobileApp = true });
                if (Page == "Video") return RedirectToAction("Index", "Video", new { MobileApp = true });
                if (Page == "Calendar") return RedirectToAction("Index", "EmailCalendar", new { MobileApp = true });
                if (Page == "Jobs") return RedirectToAction("Index", "Jobs", new { MobileApp = true });
                if (Page == "Agreements") return RedirectToAction("Index", "Agreements", new { MobileApp = true });
                if (Page == "Tasks") return RedirectToAction("Index", "Tasks", new { MobileApp = true });
                if (Page == "Documents") return RedirectToAction("Index", "Document", new { MobileApp = true });
                if (Page == "ProductCategories") return RedirectToAction("Index", "ProductCategory", new { MobileApp = true });
                if (Page == "Products") return RedirectToAction("Index", "ProductCategory", new { MobileApp = true });
                if (Page == "Packages") return RedirectToAction("Index", "Package", new { MobileApp = true });
                if (Page == "Subscriptions") return RedirectToAction("Index", "Subscription", new { MobileApp = true });
                if (Page == "UnpaidInvoices") return RedirectToAction("Index", "UnpaidInvoices", new { MobileApp = true });
                if (Page == "PaidInvoices") return RedirectToAction("Index", "PaidInvoices", new { MobileApp = true });
                if (Page == "Funding") return RedirectToAction("Index", "Funding", new { MobileApp = true });
                if (Page == "CompanyProfile") return RedirectToAction("Index", "CompanyProfile", new { MobileApp = true });
                if (Page == "CompanyRoles") return RedirectToAction("Index", "CompanyRoles", new { MobileApp = true });
                if (Page == "Users") return RedirectToAction("Index", "User", new { MobileApp = true });
                if (Page == "CourseManagement") return RedirectToAction("Index", "CourseManagement", new { MobileApp = true });
                if (Page == "Courses") return RedirectToAction("Index", "Course", new { MobileApp = true });
                if (Page == "Facilities") return RedirectToAction("Index", "Facilities", new { MobileApp = true });
                if (Page == "Spaces") return RedirectToAction("Index", "Space", new { MobileApp = true });
                if (Page == "CalendarTypes") return RedirectToAction("Index", "CalendarTypes", new { MobileApp = true });
                if (Page == "ClosedDates") return RedirectToAction("Index", "ClosedDates", new { MobileApp = true });
                if (Page == "Feedback") return RedirectToAction("Index", "Feedback", new { MobileApp = true });
                if (Page == "GettingStarted") return RedirectToAction("Index", "GettingStarted", new { MobileApp = true });
                if (Page == "CompanyAccount") return RedirectToAction("Index", "Company", new { MobileApp = true });
                if (Page == "PointOfSaleNewOrder") return RedirectToAction("Index", "PointOfSale", new { MobileApp = true });
                if (Page == "Product") return RedirectToAction("StoreItems", "PointOfSale", new { MobileApp = true });
                if (Page == "PointOfSalePackage") return RedirectToAction("PackageLists", "PointOfSale", new { MobileApp = true });
                if (Page == "PointOfSaleUnfufilledOrders") return RedirectToAction("UnfulfilledOrdersList", "PointOfSale", new { MobileApp = true });
                if (Page == "PointOfSaleCompletedOrders") return RedirectToAction("CompletedOrdersList", "PointOfSale", new { MobileApp = true });
                if (Page == "Inventory") return RedirectToAction("GetInventoryReport", "PointOfSale", new { MobileApp = true });
                if (Page == "Integration") return RedirectToAction("Index", "CompanyIntegration", new { MobileApp = true });
                if (Page == "ContactForm") return RedirectToAction("Index", "ContactForm", new { MobileApp = true });
                return RedirectToAction("Index", "Customer", new { MobileApp = true });



            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name);
            }
        }


    }
    public class UserData
    {
        public bool Valid { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string AuthorizationCode { get; set; }
        public string ResetPasswordCode { get; set; }
        public string CompanyName { get; set; }
        public string Logo { get; set; }
        public bool SystemAdmin { get; set; }
        public SideNav SideNav { get; set; }
        public BottomNav BottomNav { get; set; }
        public QuickTasks QuickTasks { get; set; }
    }

    public class SideNav
    {
        public List<Page> Pages { get; set; }  
    }
    public class BottomNav
    {
        public List<Page> Pages { get; set; }
    }
    public class QuickTasks
    {
        public List<Page> Pages { get; set; }
    }
    public class Page
    {
        public string DisplayName { get; set; }
        public string PageUrl { get; set; }
        public List<SubPage> SubPages { get; set; }
    }
    public class SubPage
    {
        public string DisplayName { get; set; }
        public string PageUrl { get; set; }
    }
}