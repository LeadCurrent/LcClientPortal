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
using static Data.RoleEnums;
using CommonClasses;
namespace Web
{
    [Authorize(Policy = "AdminOrCompanyRoles")]

    public class CompanyRolesController : Controller
    {
        
        private readonly RazorViewToStringRenderer viewRenderer;
        
        public ExceptionLogger exceptionLogger { get; }
        public CompanyRolesDataLibrary CompanyRolesDL { get; }
        public UserDataLibrary UserDL { get; }
        public UserManager<User> UserManager { get; }
        public SignInManager<User> SignInManager { get; }

        public CompanyRolesController(
            RazorViewToStringRenderer RazorViewToStringRenderer,
            CompanyRolesDataLibrary CompanyRolesDataLibrary,
            UserDataLibrary UserDataLibrary,
            EmailDataLibrary EmailDataLibrary,
            UserManager<User> userManager,
            
             ExceptionLogger _exceptionLogger,
             SignInManager<User> signInManager
        )
        {
            viewRenderer = RazorViewToStringRenderer;
            CompanyRolesDL = CompanyRolesDataLibrary;
            UserDL = UserDataLibrary;
            UserManager = userManager;
            
            exceptionLogger = _exceptionLogger;
            SignInManager = signInManager;
        }

        public async Task<CompanyRolesVM> getModel()
        {
            var CompanyId = Int32.Parse(User.Claims.Where(x => x.Type == "CompanyId").FirstOrDefault().Value);
            var Model = new CompanyRolesVM();
            Model.Roles = await CompanyRolesDL.GetRoles(CompanyId);
            Model.CompanyId = CompanyId;
            return Model;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var Model = await getModel();
                if (HttpContext.Session.GetString("MobileApp") != null)
                {
                    Model.MobileApp = true;
                }
                Model.CurrentTab = "Integrations";
                return View("Integration", Model);
                return View("UserRole", Model);

            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, false);
            }
        }


        [HttpPost]
        public async Task<IActionResult> UserRole(int companyId, string Action, CompanyRolesVM ViewModel)
        {
            try
            {


                if (Action == "Show Create Role")
                {
                    return RedirectToAction("CreateUserRole");
                }
                if (Action == "Show Edit Role")
                {
                    return RedirectToAction("EditRole", new { RoleId = ViewModel.Param });
                }
                var Model = await getModel();
                return View("UserRole", Model);

            }

            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, false);
            }
        }


        [HttpGet]
        public async Task<IActionResult> CreateUserRole()
        {
            try
            {
                var Model = await getModel();
                if (HttpContext.Session.GetString("MobileApp") != null)
                {
                    Model.MobileApp = true;
                }
                return View("CreateUserRole", Model);
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, false);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserRole(CompanyRolesVM ViewModel, String Action)
        {
            try
            {
                if (Action == "Create")
                {
                    var Role = await CompanyRolesDL.CreateRole(ViewModel.RoleName, ViewModel.CompanyId);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, false);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(int RoleId)
        {
            try
            {
                var model = await getModel();
                model.Role = await CompanyRolesDL.GetRole(RoleId);
                var RPExisting = await CompanyRolesDL.GetRolePermissionsForRole(model.Role.Id);

                var existingRolePermissions = new List<RolePermission>();
                foreach (var permission in RPExisting)
                {
                    if (RoleEnums.PermissionIncluded(permission.Permission))
                        existingRolePermissions.Add(permission);
                }

                var newRolePermissions = new List<RolePermission>();

                foreach (var permission in Enum.GetValues(typeof(Permission)).Cast<Permission>())
                {
                    if (RoleEnums.PermissionIncluded(permission))
                    {
                        if (!existingRolePermissions.Where(x => x.Permission == permission).Any())
                        {
                            RolePermission newRolePermission;

                            if (model.Role.RoleName == "Admin")
                            {
                                newRolePermission = new RolePermission
                                {
                                    RoleId = model.Role.Id,
                                    Permission = permission,
                                    //Create = true,
                                    //Edit = true,
                                    //View = true
                                };
                            }
                            else
                            {
                                newRolePermission = new RolePermission
                                {
                                    RoleId = model.Role.Id,
                                    Permission = permission,
                                    //Create = false,
                                    //Edit = false,
                                    //View = false
                                };
                            }

                            newRolePermissions.Add(newRolePermission);
                        }
                    }

                }
                if (newRolePermissions.Any())
                {
                    await CompanyRolesDL.CreateOrUpdateRolePermissions(model.Role.Id, newRolePermissions);
                    existingRolePermissions = await CompanyRolesDL.GetRolePermissionsForRole(model.Role.Id);
                }
                model.RolePermissions = existingRolePermissions;



                foreach (var permission in model.RolePermissions)
                {
                    permission.SortOrder = PermissionSort(permission.Permission);
                }
                model.RolePermissions = model.RolePermissions.OrderBy(x => x.SortOrder).ToList();

                if (HttpContext.Session.GetString("MobileApp") != null)
                {
                    model.MobileApp = true;
                }



                return View("EditUserRole", model);
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, false);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditUserRole(CompanyRolesVM ViewModel, String Action)
        {
            try
            {
                var Model = new CompanyRolesVM();

                if (Action == "Cancel")
                    return RedirectToAction("Index");

                if (Action == "Update DropDown Permissions")
                {
                    var user = await UserDL.GetCurrentUser(User.Identity.Name);
                    if (ViewModel.RolePermissions != null)
                    {
                        var rolePermission = await CompanyRolesDL.UpdatePermission(ViewModel.RolePermissions, ViewModel.Role.Id, ViewModel.Param);
                        string claimToAdd = null;

                            if (rolePermission.Access == Access.ViewOnly)
                                claimToAdd = $"{rolePermission.Permission}-{Access.ViewOnly}";
                            else if (rolePermission.Access == Access.EditAndView)
                                claimToAdd = $"{rolePermission.Permission}-{Access.EditAndView}";

                        var UsersWithSameRole = await CompanyRolesDL.GetCompanyUserRoleByRoleId(ViewModel.Role.Id);

                        foreach (var userRole in UsersWithSameRole)
                        {
                            var currentUser = userRole.CompanyUser.User;
                            var userClaims = await UserManager.GetClaimsAsync(currentUser);

                            var claimToRemove = userClaims.FirstOrDefault(x =>
                            (x.Type.Contains(rolePermission.Permission.ToString()) && x.Value == rolePermission.RoleId.ToString()));

                            if (claimToRemove != null)
                                await UserDL.RemoveUserClaimAsync(currentUser.Id, claimToRemove.Type, claimToRemove.Value);

                            if (claimToAdd != null)
                                await UserManager.AddClaimAsync(currentUser, new Claim(claimToAdd, userRole.RoleId.ToString()));
                        }

                        Model.Role = await CompanyRolesDL.GetRole(ViewModel.Role.Id);
                        Model.RolePermissions = await CompanyRolesDL.GetRolePermissionsForRole(ViewModel.Role.Id);

                        await SignInManager.SignOutAsync();
                        await SignInManager.SignInAsync(user, true);
                    }
                }

                if (Action == "Update")
                {
                    await CompanyRolesDL.UpdateRole(ViewModel.Role, User.Identity.Name);
                    Model.Role = await CompanyRolesDL.GetRole(ViewModel.Role.Id);
                    Model.RolePermissions = await CompanyRolesDL.GetRolePermissionsForRole(ViewModel.Role.Id);
                    Model.UpdateSuccessful = true;
                }

                var HTML = Task.Run(() => viewRenderer.RenderViewToStringAsync("CompanyRoles/PartialViews/EditUserRole_Partial", Model)).Result;
                return Json(new { isValid = true, html = HTML });
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, false);
            }
        }
    }
}