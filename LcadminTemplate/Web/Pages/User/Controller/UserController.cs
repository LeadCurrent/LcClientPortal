using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Data;
using Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Project.Utilities;
using Microsoft.AspNetCore.Http;
using static Data.GeneralEnums;
using static Data.RoleEnums;
using static Web.RoleEnumDDLs;
using CommonClasses;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.Web;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Scheduler.Controllers
{
    //[Authorize(Policy = "AdminOrUsers")]
    public class UserController : Controller
    {
        public UserManager<User> UserManager { get; }
        public SignInManager<User> SignInManager { get; }
        public UserDataLibrary UserDL { get; }
        public CompanyRolesDataLibrary CompanyRolesDL { get; }
        public EmailDataLibrary EmailDL { get; }
        public CompanyDataLibrary CompanyDL { get; }
        private readonly RazorViewToStringRenderer viewRenderer;
        public CompanyIntegrationDataLibrary CompanyIntegrationDL { get; set; }
        public UserController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            UserDataLibrary UserDataLibrary,
            CompanyRolesDataLibrary CompanyRolesDataLibrary,
            CompanyDataLibrary CompanyDataLibrary,
            EmailDataLibrary EmailDataLibrary,
            RazorViewToStringRenderer RazorViewToStringRenderer,
            CompanyIntegrationDataLibrary CompanyIntegrationDataLibrary)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            UserDL = UserDataLibrary;
            CompanyDL = CompanyDataLibrary;
            EmailDL = EmailDataLibrary;
            CompanyRolesDL = CompanyRolesDataLibrary;
            viewRenderer = RazorViewToStringRenderer;
            CompanyIntegrationDL = CompanyIntegrationDataLibrary;
        }

        public async Task<UserVM> getUserListModel()
        {
            var UserVM = new UserVM();
            var CompanyId = Int32.Parse(User.Claims.Where(x => x.Type == "CompanyId").FirstOrDefault().Value);
            UserVM.CompanyUsers = await CompanyDL.GetCompanyUsers(CompanyId);
            if (HttpContext.Session.GetInt32("UserFilterStatus") != null)
            {
                UserVM.FilterStatus = (Status)HttpContext.Session.GetInt32("UserFilterStatus").Value;
                UserVM.CompanyUsers = UserVM.CompanyUsers.Where(x => x.Status == UserVM.FilterStatus).ToList();
            }
            else
                UserVM.CompanyUsers = UserVM.CompanyUsers.Where(cu => cu.Status == Status.Active).ToList();

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
            if (Action == "Apply Filters")
                HttpContext.Session.SetInt32("UserFilterStatus", (int)ViewModel.FilterStatus);

            if (Action == "Edit")
                return RedirectToAction("EditUser", new { Id = ViewModel.Param });

            if (Action == "Create")
                return RedirectToAction("Create");

            if (Action == "Get Mails")
            {
                var CompanyId = Int32.Parse(User.Claims.Where(x => x.Type == "CompanyId").FirstOrDefault().Value);
                var DefaultCompanyEmailAccount = await CompanyDL.GetDefaultOrFirstCompanyEmailAccount(CompanyId);
                string accessToken = await GoogleAPI.GetAccessTokenFromRefreshTokenAsync(DefaultCompanyEmailAccount.RefreshToken);
                //var AllMails = GoogleAPI.GetAllEmails(accessToken);
                //var MailsWithBody = GoogleAPI.GetAllEmailsWithBodies(accessToken);
                DateTime afterTimestamp = DefaultCompanyEmailAccount.LastSyncDate; // Example timestamp


                // var emails = GoogleAPI.GetAllEmailsWithBodies(accessToken, afterTimestamp);


            }
            if (Action == "Get Microsoft Mails")
            {

                var CompanyId = Int32.Parse(User.Claims.Where(x => x.Type == "CompanyId").FirstOrDefault().Value);
                var DefaultCompanyEmailAccount = await CompanyDL.GetDefaultOrFirstCompanyEmailAccount(CompanyId);
                string accessToken = await MicrosoftAPI.GetAccessTokenAsync(DefaultCompanyEmailAccount.RefreshToken);
                DateTime afterTimestamp = new DateTime(2024, 5, 14, 12, 30, 7); // Example timestamp

                //DateTime afterTimestamp = DateTime.Today.AddHours(8);

                // var emails = MicrosoftAPI.FetchEmailsAsync(accessToken, afterTimestamp);
            }
            var UserVM = await getUserListModel();

            var HTML = await viewRenderer.RenderViewToStringAsync("User/PartialViews/UserList_Partial", UserVM);
            return Json(new { isValid = true, html = HTML });
        }

        [HttpGet]
        public async Task<IActionResult> Create(UserVM ViewModel)
        {
            var Model = new UserVM();
            var companyUser = new CompanyUser();
            Model.CompanyId = Int32.Parse(User.Claims.Where(x => x.Type == "CompanyId").FirstOrDefault().Value);
            Model.CompanyUser = companyUser;
            Model.ShowEdit = true;            
            Model.CompanyUsers = await CompanyDL.GetCompanyUsers(Model.CompanyId);
            Model.CompanyUserRoles = await CompanyRolesDL.GetCompanyUserRoles(Model.CompanyId);
            var companyUserId = Model.CompanyUsers.FirstOrDefault(x => x.CompanyId == Model.CompanyId)?.Id;
            if (companyUserId != null)
            {
                Model.CompanyUserRoles = await CompanyRolesDL.GetCompanyUserRoles(companyUserId.Value);

            }
            Model.Roles = await CompanyRolesDL.GetRoles(Model.CompanyId);
            var systemAdminClaim = User.Claims.FirstOrDefault(x => x.Type == "SystemAdmin");
            {
                Model.IsSystemAdmin = true;
            }
            if (HttpContext.Session.GetString("MobileApp") != null)
                Model.MobileApp = true;
            if (!string.IsNullOrEmpty(ViewModel.ErrorMessage))
                Model.ErrorMessage = ViewModel.ErrorMessage;
            return View("CreateUser", Model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserVM ViewModel, string Action)
        {
            var HTML = "";
            if (Action == "Cancel")
                return RedirectToAction("Index", "User", "");

            if (Action == "Create")
            {
                if (ViewModel.Password != ViewModel.ConfirmPassword)
                {
                    ViewModel.ErrorMessage = "Passwords do not match";
                    HTML = await viewRenderer.RenderViewToStringAsync("User/PartialViews/CreateUser_Partial", ViewModel);
                    return RedirectToAction("Create", "User", ViewModel);
                }

                var NewUser = new User
                {
                    UserName = ViewModel.CompanyUser.User.UserName,
                    FirstName = ViewModel.CompanyUser.User.FirstName,
                    LastName = ViewModel.CompanyUser.User.LastName,
                    Email = ViewModel.CompanyUser.User.Email,
                    Status = Status.Active,
                    SystemAdmin = ViewModel.CompanyUser.User.SystemAdmin,
                    TemporaryPassword = true,
                    EmailConfirmed = true,
                    Phone = ViewModel.CompanyUser.User.Phone
                };

                var result = await UserManager.CreateAsync(NewUser, ViewModel.Password);
                if (!result.Succeeded)
                {
                    if (result.Errors.Any())
                        ViewModel.ErrorMessage = result.Errors.FirstOrDefault().Description;
                    else
                        ViewModel.ErrorMessage = "User was not successfully created";
                    return RedirectToAction("Create", "User", ViewModel);
                }
                else
                {
                    var CompnyId = Int32.Parse(User.Claims.Where(x => x.Type == "CompanyId").FirstOrDefault().Value);
                    var Company = await CompanyDL.GetCompany(CompnyId);
                    if (result.Succeeded)
                    {
                        //Claims
                        await UserManager.AddClaimAsync(NewUser, new Claim("CompanyId", CompnyId.ToString()));
                        if (NewUser.SystemAdmin)
                            await UserManager.AddClaimAsync(NewUser, new Claim("SystemAdmin", "True"));
                        var claimsPrincipal = await SignInManager.CreateUserPrincipalAsync(NewUser);

                        //CompanyUserRole
                        int CompanyUserId = await CompanyDL.CreateCompanyUser(CompnyId, NewUser.Id);

                        //Assign Roles
                        var RolesId = Request.Form["RoleGroups"];
                        if (!string.IsNullOrEmpty(RolesId))
                            foreach (var roleId in RolesId)
                            {
                                await CompanyRolesDL.CreateCompanyUserRole(CompanyUserId, Int32.Parse(roleId));
                                var Role = await CompanyRolesDL.GetRole(Int32.Parse(roleId));

                                var accessPermissions = Role.RolePermissions
                                    .Where(rp => (rp.JobAccess != JobAccess.NoAccess) || rp.Access != Access.NoAccess)
                                    .ToList();

                                foreach (var p in accessPermissions)
                                {
                                    string claimValue = $"{p.Permission}-{p.Access}";
                                    await UserDL.AddUserClaimAsync(NewUser.Id, claimValue, Role.Id.ToString());
                                }
                            }

                        return RedirectToAction("EditUser", new { Id = CompanyUserId });
                    }
                }
            }
            return RedirectToAction("Index", "User", "");
        }

        private async Task<UserVM> GetEditUserModel(int Id)
        {
            var VM = new UserVM();
            var CompanyId = Int32.Parse(User.Claims.Where(x => x.Type == "CompanyId").FirstOrDefault().Value);
            VM.CompanyUser = await CompanyDL.GetCompanyUser(Id);
            VM.User = await CompanyDL.GetUser(VM.CompanyUser.UserId);
            VM.User.userLogin = await CompanyDL.GetUserLogin(VM.CompanyUser.UserId);
            VM.Roles = await CompanyRolesDL.GetAvailableRoleForUser(VM.CompanyUser.User, CompanyId);
            VM.CompanyUserRoles = await CompanyRolesDL.GetCompanyUserRoles(CompanyId);
            VM.IsSystemAdmin = User.Claims.Any(c => c.Type == "SystemAdmin");

            var CompanyEmailAccounts = await CompanyIntegrationDL.GetCompanyEmailAccounts(CompanyId);
            var CompanyUserEmails = await UserDL.GetCompanyUserEmails(VM.CompanyUser.Id);

            VM.CompanyEmailAccounts = new List<CompanyEmailAccount>();

            CompanyUserEmails.ForEach(email =>
            {
                var EmailAccount = CompanyEmailAccounts.Where(x => x.Id == email.CompanyEmailAccountId).FirstOrDefault();
                if (EmailAccount != null)
                {
                    if (email.IsDefault == true)
                        EmailAccount.IsDefault = true;
                    else
                        EmailAccount.IsDefault = false;

                    VM.CompanyEmailAccounts.Add(EmailAccount);
                }
            });

            var CompanyPhoneNumber = await UserDL.GetCompanyPhoneNumber(CompanyId);
            var CompanyUserPhoneNumber = await UserDL.GetCompanyUserPhoneNumbers(VM.CompanyUser.Id);
            VM.CompanyPhoneNumbers = new List<CompanyPhoneNumber>();

            CompanyUserPhoneNumber.ForEach(email =>
            {
                var companyPhoneNumber = CompanyPhoneNumber.Where(x => x.Id == email.CompanyPhoneNumberId).FirstOrDefault();
                if (companyPhoneNumber != null)
                {
                    if (email.IsDefault == true)
                        companyPhoneNumber.IsDefault = true;
                    else
                        companyPhoneNumber.IsDefault = false;

                    VM.CompanyPhoneNumbers.Add(companyPhoneNumber);
                }
            });

            if (!CompanyUserPhoneNumber.Any())
                VM.CompanyPhoneNumbers = CompanyPhoneNumber;

            return VM;
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(int Id)
        {
            var VM = await GetEditUserModel(Id);
            if (HttpContext.Session.GetString("MobileApp") != null)
                VM.MobileApp = true;
            return View("EditUser", VM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserVM ViewModel, string Action)
        {
            var VM = new UserVM();
            if (Action == "Save Changes" || Action== "Delete User")
            {
                var user = await UserManager.FindByIdAsync(ViewModel.CompanyUser.UserId);
                var selectedCheckboxValues = Request.Form["SelectedRole"];
                await CompanyRolesDL.UpdateUserRole(user.Id);
                var roleDDL = RoleDDL();
                var Admin = false;
                var SystemAdmin = false;
                foreach (var item in selectedCheckboxValues)
                {
                    if (roleDDL.Any(ddlItem => ddlItem.Value == item))
                    {
                        if (item == "1")
                        {
                            Admin = true;
                        }
                        else if (item == "2")
                        {
                            SystemAdmin = true;
                        }
                    }
                }
                user.UserName = ViewModel.CompanyUser.User.UserName;
                user.FirstName = ViewModel.CompanyUser.User.FirstName;
                user.LastName = ViewModel.CompanyUser.User.LastName;
                user.Email = ViewModel.CompanyUser.User.Email;
                user.Admin = Admin;
                user.SystemAdmin = SystemAdmin;
                user.Status = ViewModel.CompanyUser.User.Status;
                user.Phone = ViewModel.CompanyUser.User.Phone;
                if(Action== "Delete User")
                {
                    user.IsDeleted = true;
                }
                var claims = await UserManager.GetClaimsAsync(user);
                var AdminClaim = claims.Where(c => c.Type == "Admin").FirstOrDefault();
                var SystemAdminClaim = claims.Where(c => c.Type == "SystemAdmin").FirstOrDefault();

                if (user.Admin)
                {
                    if (AdminClaim == null)
                        await UserManager.AddClaimAsync(user, new Claim("Admin", "True"));
                }
                else
                {
                    if (AdminClaim != null)
                        await UserManager.RemoveClaimAsync(user, AdminClaim);
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
                if (Action == "Delete User")
                {
                    return RedirectToAction("Index");
                }
                ViewModel.UpdateSuccessful = true;
                var CompnyId = Int32.Parse(User.Claims.Where(x => x.Type == "CompanyId").FirstOrDefault().Value);
                ViewModel.CompanyUserRoles = await CompanyRolesDL.GetCompanyUserRoles(CompnyId);
            }
            if(Action== "Edit User")
            {
                VM = await GetEditUserModel(ViewModel.CompanyUser.Id);
                VM.ShowEdit = true;
            }
            if (Action == "Update User Status")
            {
                var user = await UserManager.FindByIdAsync(ViewModel.CompanyUser.UserId);

                user.Status = ViewModel.CompanyUser.User.Status;
                await UserManager.UpdateAsync(user);
                await CompanyRolesDL.UpdateUserStatus(ViewModel.CompanyUser.Id, user.Status);

                if (ViewModel.CompanyUser.User.Status == Status.Inactive)
                {
                    var CompanyUserRoles = await UserDL.GetCompanyUserRoles(ViewModel.CompanyUser.Id);
                    foreach (var role in CompanyUserRoles)
                    {
                        var Role = await UserDL.GetRole(role.RoleId);

                        if (Role.RoleName == "Admin")
                            await UserDL.RemoveUserClaimAsync(user.Id, "Admin", Role.Id.ToString());
                        else
                        {
                            var userClaims = await UserManager.GetClaimsAsync(user);
                            var claimsToRemove = userClaims.Where(x => x.Value == Role.Id.ToString()).ToList();

                            if (claimsToRemove.Any())
                                await UserManager.RemoveClaimsAsync(user, claimsToRemove);
                        }

                        await CompanyRolesDL.RemoveCompanyUserRole(role.Id);
                    }

                    if (user.UserName == User.Identity.Name)
                    {
                        await SignInManager.SignOutAsync();
                        await SignInManager.SignInAsync(user, true);
                    }
                }

                VM = await GetEditUserModel(ViewModel.CompanyUser.Id);
                VM.UpdateSuccessful = true;
            }

            if (Action == "Change Password")
            {
                return RedirectToAction("ChangePassword", new { Id = ViewModel.CompanyUser.Id });
            }
            if(Action== "Cancel Edit")
            {
                return RedirectToAction("EditUser", new { Id = ViewModel.CompanyUser.Id });
            }
            if (Action == "Cancel")
            {
                return RedirectToAction("Index");
            }
            var CompanyId = Int32.Parse(User.Claims.Where(x => x.Type == "CompanyId").FirstOrDefault().Value);

            if (Action == "Connect to my gmail account")
            {
                if (ViewModel.LastSyncDate.HasValue)
                {
                    HttpContext.Session.SetString("LastSyncDate", ViewModel.LastSyncDate.Value.ToString("o"));
                }
                HttpContext.Session.SetInt32("CompanyUserId", ViewModel.CompanyUser.Id);
                return RedirectToAction("GoogleAuthentication");
            }
            if (Action == "Connect to my microsoft account")
            {
                if (ViewModel.LastSyncDate.HasValue)
                {
                    HttpContext.Session.SetString("LastSyncDate", ViewModel.LastSyncDate.Value.ToString("o"));
                }
                HttpContext.Session.SetInt32("CompanyUserId", ViewModel.CompanyUser.Id);
                return RedirectToAction("MicrosoftAuthentication");
            }

            if (Action == "Connect to my other account")
            {
                HttpContext.Session.SetInt32("CompanyUserId", ViewModel.CompanyUser.Id);

                 // await UserDL.CreateCompanyEmailAccountAndCompanyUserEmail(CompanyId, 3, ViewModel.OtherAccountName, ViewModel.OtherAccountEmail, null ,ViewModel.CompanyUser.Id);

            }
            if (Action == "Connect to Phone Number")
            {
                  await UserDL.CreateCompanyPhoneNoAndCompanyUserPhoneNo(CompanyId,ViewModel.ContactName,ViewModel.CompanyUser.Id , ViewModel.PhoneNumber);

            }
            if (Action == "Remove Email Account")
            {
                await UserDL.DeleteUserEmail(ViewModel.Param);
                return RedirectToAction("EditUser", new { Id = ViewModel.CompanyUser.Id });
            }
            if (Action == "Add Account")
            {
                await UserDL.AddUserEmail(ViewModel.Param, ViewModel.CompanyUser.Id);
                return RedirectToAction("EditUser", new { Id = ViewModel.CompanyUser.Id });

            }
            if (Action == "Add Phone Number")
            {
                await UserDL.AddUserCompanyPhoneNumber(ViewModel.Param, ViewModel.CompanyUser.Id);
                return RedirectToAction("EditUser", new { Id = ViewModel.CompanyUser.Id });

            }
            if (Action == "Set Default Email")
            {
                await UserDL.SetDefaultEmail(ViewModel.Param);
                return RedirectToAction("EditUser", new { Id = ViewModel.CompanyUser.Id });
            }
            if (Action == "Set Default Phone Number")
            {
                await UserDL.SetDefaultUserPhoneNumber(ViewModel.Param);
                return RedirectToAction("EditUser", new { Id = ViewModel.CompanyUser.Id });
            }
            if (Action == "Remove Default Email Account")
            {
                await UserDL.DeleteUserEmail(ViewModel.Param);
                return RedirectToAction("EditUser", new { Id = ViewModel.CompanyUser.Id });
            }
            if (Action == "Remove Phone Number")
            {
                await UserDL.DeleteUserPhoneNumber(ViewModel.Param);
                return RedirectToAction("EditUser", new { Id = ViewModel.CompanyUser.Id });
            }
            if (Action == "Show Other Account Fields")
            {
                VM.ShowAddOtherAccount = true;

            }
            if (Action == "Show Add Phone Number")
            {
                VM.ShowAddPhoneNumber = true;
            }
            if (Action == "Cancel Add other Account")
            {
                VM.ShowAddOtherAccount = false;
                VM.ShowAddPhoneNumber = false;

            }
            if (Action == "Show Add LastSyncDate Google")
            {
                VM.ShowAddStartSyncDate = true;
                VM.ConnectGmailAccount = true;
            }
            if (Action == "Show Add LastSyncDate Microsoft")
            {
                VM.ShowAddStartSyncDate = true;
                VM.ConnectMicrosoftAccount = true;
            }
            if (Action == "Go Back")
            {
                VM.ShowExistingAccount = false;
                VM.ShowExistingPhoneNumber = false;
            }
            var CompanyEmailAccounts = await CompanyIntegrationDL.GetCompanyEmailAccounts(CompanyId);
            var CompanyUserEmails = await UserDL.GetCompanyUserEmails(ViewModel.CompanyUser.Id);
            VM.CompanyEmailAccounts = new List<CompanyEmailAccount>();
            foreach (var email in CompanyUserEmails)
            {
                var EmailAccount = CompanyEmailAccounts.Where(x => x.Id == email.CompanyEmailAccountId).FirstOrDefault();
                if (EmailAccount != null)
                {
                    if (email.IsDefault == true)
                    {
                        EmailAccount.IsDefault = true;
                    }
                    else
                    {
                        EmailAccount.IsDefault = false;
                    }
                    VM.CompanyEmailAccounts.Add(EmailAccount);
                }
            }
            var CompanyPhoneNumber = await UserDL.GetCompanyPhoneNumber(CompanyId);
            var CompanyUserPhoneNumber = await UserDL.GetCompanyUserPhoneNumbers(ViewModel.CompanyUser.Id);
            VM.CompanyPhoneNumbers = new List<CompanyPhoneNumber>();
            foreach (var email in CompanyUserPhoneNumber)
            {
                var companyPhoneNumber = CompanyPhoneNumber.Where(x => x.Id == email.CompanyPhoneNumberId).FirstOrDefault();
                if (companyPhoneNumber != null)
                {
                    if (email.IsDefault == true)
                    {
                        companyPhoneNumber.IsDefault = true;
                    }
                    else
                    {
                        companyPhoneNumber.IsDefault = false;
                    }
                    VM.CompanyPhoneNumbers.Add(companyPhoneNumber);
                }
            }
            if (Action == "Connect to Existing accounts")
            {
                VM = await GetEditUserModel(ViewModel.CompanyUser.Id);
                var companyId = Int32.Parse(User.Claims.Where(x => x.Type == "CompanyId").FirstOrDefault().Value);
                var companyEmailAccounts = await CompanyIntegrationDL.GetCompanyEmailAccounts(companyId);
                var companyUserEmails = await UserDL.GetCompanyUserEmails(ViewModel.CompanyUser.Id);
                VM.CompanyEmailAccounts = await CompanyIntegrationDL.GetCompanyEmailAccounts(CompanyId);
                //CompanyUserEmails = CompanyUserEmails.Where(x => x.CompanyUserId != ViewModel.CompanyUser.Id).ToList(); 
                foreach (var email in companyEmailAccounts)
                {
                    var EmailAccount = companyUserEmails.Where(x => x.CompanyEmailAccountId == email.Id).FirstOrDefault();
                    if (EmailAccount != null)
                    {
                        VM.CompanyEmailAccounts.Remove(email);
                    }
                }
                VM.ShowExistingAccount = true;
            }
            if (Action == "Connect to Existing PhoneNumber")
            {
                VM = await GetEditUserModel(ViewModel.CompanyUser.Id);
                VM.ShowExistingPhoneNumber = true;
            }

            VM.CompanyUser = await CompanyDL.GetCompanyUser(ViewModel.CompanyUser.Id);
            VM.AllSelectedRoles = new List<int>();
            if (VM.CompanyUser.User.Admin)
            {
                VM.AllSelectedRoles.Add(1);
            }
            if (VM.CompanyUser.User.SystemAdmin)
            {
                VM.AllSelectedRoles.Add(2);
            }

            if (Action == "Update Role")
            {
                var UserToUpdate = await UserDL.GetCurrentUserById(ViewModel.CompanyUser.UserId);
                var CompanyUserRole = await CompanyRolesDL.CreateCompanyUserRole(ViewModel.CompanyUser.Id, ViewModel.SelectedRole);
                var Role = await CompanyRolesDL.GetRole(CompanyUserRole.RoleId);

                var accessPermissions = Role.RolePermissions
                    .Where(rp => (rp.JobAccess != JobAccess.NoAccess) || rp.Access != Access.NoAccess)
                    .ToList();

                foreach (var p in accessPermissions)
                {
                    string claimValue = $"{p.Permission}-{p.Access}";
                    await UserDL.AddUserClaimAsync(UserToUpdate.Id, claimValue, Role.Id.ToString());
                }

                if (UserToUpdate.UserName == User.Identity.Name)
                {
                    await SignInManager.SignOutAsync();
                    await SignInManager.SignInAsync(UserToUpdate, true);
                }

                VM = await GetEditUserModel(ViewModel.CompanyUser.Id);
            }

            if(Action == "Delete User")
            {
                await UserDL.DeleteUser(ViewModel.CompanyUser.Id, User.Identity.Name);
                HttpContext.Session.Clear();
                return RedirectToAction("Index");
            }
            if (Action == "Show History")
            {
                VM = await GetEditUserModel(ViewModel.CompanyUser.Id);
                VM.ShowHistory = true;
                VM.ShowEdit = ViewModel.ShowEdit;
            }
            if (Action == "Go Back")
            {
                VM = await GetEditUserModel(ViewModel.CompanyUser.Id);
                VM.ShowHistory = false;
                VM.ShowEdit = ViewModel.ShowEdit;
            }
            if (Action == "Remove Role")
            {
                var UserToUpdate = await UserDL.GetCurrentUserById(ViewModel.CompanyUser.UserId);
                var CompanyUserRole = await CompanyRolesDL.GetCompanyUserRole(ViewModel.Param);
                var Role = await CompanyRolesDL.GetRole(CompanyUserRole.RoleId);
                await CompanyRolesDL.RemoveCompanyUserRole(ViewModel.Param);

                var AccessPermissions = Role.RolePermissions.Where(x => x.Access == Access.ViewOnly || x.Access == Access.EditAndView
                && x.JobAccess != JobAccess.NoAccess).ToList();

                if (AccessPermissions.Any())
                    foreach (var p in AccessPermissions)
                    {
                        var claimValue = $"{p.Permission}-{p.Access}";
                        await UserDL.RemoveUserClaimAsync(UserToUpdate.Id, claimValue, Role.Id.ToString());
                    }

                if (UserToUpdate.UserName == User.Identity.Name)
                {
                    await SignInManager.SignOutAsync();
                    await SignInManager.SignInAsync(UserToUpdate, true);
                }

                VM = await GetEditUserModel(ViewModel.CompanyUser.Id);
            }
            VM.UpdateSuccessful = ViewModel.UpdateSuccessful;

            var HTML = await viewRenderer.RenderViewToStringAsync("User/PartialViews/EditUser_Partial", VM);
            return Json(new { isValid = true, html = HTML });
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword(int Id)
        {
            var VM = new UserVM();
            VM.CompanyUser = await CompanyDL.GetCompanyUser(Id);
            if (HttpContext.Session.GetString("MobileApp") != null)
                VM.MobileApp = true;
            return View("ChangePassoword", VM);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(UserVM ViewModel, string Action)
        {
            if (Action == "Change Password")
            {
                if (ModelState.ContainsKey(nameof(ViewModel.AjaxUpdate)))                
                    ModelState.Remove(nameof(ViewModel.AjaxUpdate));
                

                if (ModelState.IsValid)
                {
                    if (ViewModel.Password != ViewModel.ConfirmPassword)
                    {
                        ViewModel.PasswordDoesNotMatch = true;
                        return View("ChangePassoword", ViewModel);
                    }

                    var companyuser = await CompanyDL.GetCompanyUser(ViewModel.CompanyUser.Id);

                    if (companyuser.User == null)
                    {
                        return NotFound();
                    }
                    companyuser.User.PasswordHash = UserManager.PasswordHasher.HashPassword(companyuser.User, ViewModel.Password);
                    var result = await UserManager.UpdateAsync(companyuser.User);
                    if (result.Succeeded)
                    {
                        await UserDL.SetTemporaryPassword(companyuser.UserId, true);
                    }
                    return RedirectToAction("EditUser", "User", new { Id = ViewModel.CompanyUser.Id });
                }
                else
                {
                    return View(ViewModel);
                }
            }
            return RedirectToAction("EditUser", "User", new { Id = ViewModel.CompanyUser.Id });


        }

        public ActionResult GoogleAuthentication()
        {
            string state = Guid.NewGuid().ToString("N");

            string redirectUri = Url.Action("GoogleCallback", "User", null, HttpContext.Request.Scheme);
            string authUrl = GoogleAPI.GetAuthorizationUrl(redirectUri, state);

            return Redirect(authUrl);
        }

        public async Task<IActionResult> MicrosoftAuthentication()
        {
            try
            {
                string redirectUri = Url.Action("MicrosoftAuthCallback", "User", null, HttpContext.Request.Scheme);
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
                if (code != null)
                {
                    string redirectUri = Url.Action("GoogleCallback", "User", null, HttpContext.Request.Scheme);
                    var CompanyId = Int32.Parse(User.Claims.Where(x => x.Type == "CompanyId").FirstOrDefault().Value);
                    var StoredDate = HttpContext.Session.GetString("LastSyncDate");
                    DateTime LastSyncDate = DateTime.Parse(StoredDate);
                    var CompanyUserId = (int)HttpContext.Session.GetInt32("CompanyUserId");

                    // Get the authorization code flow and exchange the code for tokens
                    var refreshtoken = await GoogleAPI.GetRefreshToken(code, redirectUri);

                    if (refreshtoken != null)
                    {
                        string accessToken = await GoogleAPI.GetAccessTokenFromRefreshTokenAsync(refreshtoken);
                        var userinfo = GoogleAPI.GetUserInfo(accessToken);
                        var IsAccountPresent = await CompanyIntegrationDL.CheckCompanyAccount(CompanyId, userinfo.Email);

                        if (IsAccountPresent)
                        {
                            await UserDL.UpdateRefreshtokenAndCreateCompanyUserEmail(CompanyId, userinfo.Email, refreshtoken, CompanyUserId);
                        }
                        else
                        {
                            await UserDL.CreateCompanyEmailAccountAndCompanyUserEmail(CompanyId, 1, userinfo.Name, userinfo.Email, refreshtoken, CompanyUserId, LastSyncDate);
                        }
                    }
                    return RedirectToAction("EditUser", "User", new { Id = CompanyUserId });
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
                string redirectUri = Url.Action("MicrosoftAuthCallback", "User", null, HttpContext.Request.Scheme);

                var refreshtoken = await MicrosoftAPI.GetAccessTokenAndRefreshFromCodeTokenAsync(code, redirectUri);
                var CompanyId = Int32.Parse(User.Claims.Where(x => x.Type == "CompanyId").FirstOrDefault().Value);
                var StoredDate = HttpContext.Session.GetString("LastSyncDate");
                DateTime LastSyncDate = DateTime.Parse(StoredDate);
                var CompanyUserId = (int)HttpContext.Session.GetInt32("CompanyUserId");

                //var CompanyUser = await CompanyUserDL.GetCompanyUserByUser(User.Identity.Name);


                if (refreshtoken != null)
                {
                    var accesstoken = await MicrosoftAPI.GetAccessTokenAsync(refreshtoken);
                    var userinfo = await MicrosoftAPI.GetUserEmailAsync(accesstoken);
                    var IsAccountPresent = await CompanyIntegrationDL.CheckCompanyAccount(CompanyId, userinfo.Email);
                    if (IsAccountPresent)
                    {
                        await UserDL.UpdateRefreshtokenAndCreateCompanyUserEmail(CompanyId, userinfo.Email, refreshtoken, CompanyUserId);
                    }
                    else
                    {
                        await UserDL.CreateCompanyEmailAccountAndCompanyUserEmail(CompanyId, 2, userinfo.Name, userinfo.Email, refreshtoken, CompanyUserId, LastSyncDate);
                    }

                }
                return RedirectToAction("EditUser", "User", new { Id = CompanyUserId });
            }
            return RedirectToAction("Index");
        }

    }
}
