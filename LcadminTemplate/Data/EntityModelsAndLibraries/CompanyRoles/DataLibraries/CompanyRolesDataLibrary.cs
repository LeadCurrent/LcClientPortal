using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;
using static Data.CompanyEnums;
using static Data.GeneralEnums;
using static Data.RoleEnums;

namespace Data
{
    public class CompanyRolesDataLibrary
    {
        public DataContext context { get; }
        static readonly char[] padding = { '=' };

        public CompanyRolesDataLibrary(DataContext Context)
        {
            context = Context;
        }

        public async Task<List<CompayUserRole>> GetCompanyUserRoles(int CompanyId)
        {
            return await context.CompayUserRole.Include(x => x.CompanyUser).Include(x => x.Role).Where(x => x.CompanyUser.CompanyId == CompanyId).ToListAsync();
            }

        public async Task<CompayUserRole> GetCompanyUserRole(int CompanyUserRoleId)
        {
            var cur = await context.CompayUserRole
                .Include(r => r.Role)
                .Where(x => x.Id == CompanyUserRoleId).FirstOrDefaultAsync();

            return cur;
        }

        public async Task UpdateUserStatus(int CompanyUserId, Status status)
        {
            context.ChangeTracker.Clear();
            var cu = await context.CompanyUser.FirstOrDefaultAsync(x => x.Id == CompanyUserId);

            if (cu != null)
            {
                cu.Status = status;

                context.Entry(cu).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<Role>> GetAvailableRoleForUser(User user, int CompanyId)
        {
            var companyUserRoles = await context.CompayUserRole
                .Include(x => x.Role)
                .Include(x => x.CompanyUser)
                .Where(x => x.CompanyUser.UserId == user.Id && x.CompanyUser.CompanyId == CompanyId)
                .ToListAsync();

            var roles = await context.Role
                .Where(x => x.CompanyId == CompanyId)
                .ToListAsync();

            var assignedRoleIds = companyUserRoles.Select(x => x.Role.Id);

            return roles.Where(role => !assignedRoleIds.Contains(role.Id)).ToList();
        }
        public async Task<List<Role>> GetRoles(int CompanyId)
        {
            return await context.Role.Where(x => x.CompanyId == CompanyId).ToListAsync();
        }
        public async Task UpdateRole(Role Role, string CurrentUser)
        {
            context.ChangeTracker.Clear();
            var user = await context.User.Where(u => u.UserName == CurrentUser).FirstOrDefaultAsync();
            var CurrentRole = await context.Role.Where(s => s.Id == Role.Id).FirstOrDefaultAsync();
            context.Entry(CurrentRole).State = EntityState.Detached;
            context.Entry(Role).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task UpdateUserRole(string UserId)
        {
            var ExistingUser = await context.User.Where(x => x.Id == UserId).FirstOrDefaultAsync();
            if (ExistingUser != null)
            {
                ExistingUser.Admin = false;
                ExistingUser.SystemAdmin = false;
            }
            context.Entry(ExistingUser).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task<Role> CreateRole(string roleName, int companyId)
        {
            context.ChangeTracker.Clear();
            var role = new Role
            {
                CompanyId = companyId,
                RoleName = roleName
            };

            await context.Role.AddAsync(role);
            await context.SaveChangesAsync();

            if (role.RoleName == "Admin")
            {
                var newRolePermissions = Enum.GetValues(typeof(Permission))
                    .Cast<Permission>()
                    .Select(permission => new RolePermission
                    {
                        RoleId = role.Id,
                        Permission = permission,
                        Access = Access.EditAndView,
                    }).ToList();

                await context.RolePermission.AddRangeAsync(newRolePermissions);
                await context.SaveChangesAsync();
            }
            else
            {
                var newRolePermissions = Enum.GetValues(typeof(Permission))
                    .Cast<Permission>()
                    .Select(permission => new RolePermission
                    {
                        RoleId = role.Id,
                        Permission = permission,
                        Access = Access.NoAccess,
                    }).ToList();

                await context.RolePermission.AddRangeAsync(newRolePermissions);
                await context.SaveChangesAsync();
            }

            return role;
        }


        public async Task<Role> CreateAdminRole(string roleName, int companyId)
        {
            context.ChangeTracker.Clear();
            var role = new Role();
            role.CompanyId = companyId;
            role.RoleName = roleName;
            await context.Role.AddAsync(role);
            await context.SaveChangesAsync();

            if (role.RoleName == "Admin")
            {
                var newRolePermissions = new List<RolePermission>();

                foreach (var permission in Enum.GetValues(typeof(Permission)).Cast<Permission>())
                {
                    RolePermission newRolePermission = null;

                    if (role.RoleName == "Admin")
                    {
                        newRolePermission = new RolePermission
                        {
                            RoleId = role.Id,
                            Permission = permission,
                            //Create = true,
                            //Edit = true,
                            //View = true
                        };
                    }

                    if (newRolePermission != null)
                    {
                        newRolePermissions.Add(newRolePermission);
                    }
                }
                await context.RolePermission.AddRangeAsync(newRolePermissions);
                await context.SaveChangesAsync();
            }

            return role;
        }
        public async Task<List<RolePermission>> CreateOrUpdateRolePermissions(int roleId, List<RolePermission> rolePermissions)
        {
            var existingRolePermissions = await context.RolePermission.Where(rp => rp.RoleId == roleId).ToListAsync();
            foreach (var newRolePermission in rolePermissions)
            {
                var existingPermission = existingRolePermissions.FirstOrDefault(rp => rp.Permission == newRolePermission.Permission);
                if (existingPermission != null)
                {
                    existingPermission.Permission = newRolePermission.Permission;
                    context.Entry(existingPermission).State = EntityState.Modified;
                }
                else
                {
                    newRolePermission.RoleId = roleId;
                    context.RolePermission.Add(newRolePermission);
                }
            }

            await context.SaveChangesAsync();
            return await context.RolePermission.Where(rp => rp.RoleId == roleId).AsNoTracking().ToListAsync();
        }

        public async Task<RolePermission> GetPermission(int Id)
        {
            return await context.RolePermission.Where(rp => rp.Id == Id).FirstOrDefaultAsync();
        }
        public async Task<RolePermission> UpdatePermission(List<RolePermission> Permissions, int roleId, int Id)
        {
            context.ChangeTracker.Clear();
            var permission = Permissions.Where(rp => rp.Id == Id).FirstOrDefault();
            permission.RoleId = roleId;
            context.Entry(permission).State = EntityState.Modified;
            await context.SaveChangesAsync();

            return permission;
        }
        public async Task<List<RolePermission>> GetRolePermissionsForRole(int roleId)
        {
            var existingRolePermissions = await context.RolePermission
                .Where(rp => rp.RoleId == roleId)
                .ToListAsync();

            return existingRolePermissions;
        }
        public async Task<Role> GetRole(int roleId)
        {
            return await context.Role.Include(x => x.RolePermissions).Where(x => x.Id == roleId).FirstOrDefaultAsync();
        }
        public async Task<List<CompayUserRole>> GetCompanyUserRoleByRoleId(int RoleId)
        {
            var cur = await context.CompayUserRole
                .Include(r => r.Role)
                    .ThenInclude(r => r.RolePermissions)
                .Include(r => r.CompanyUser)
                    .ThenInclude(r => r.User)
                .Where(x => x.RoleId == RoleId).ToListAsync();

            return cur;
        }

        public async Task<CompayUserRole> GetCompanyUserRoleByCompanyUserId(int CompanyUserId)
        {
            var cur = await context.CompayUserRole
                .Include(r => r.Role)
                    .ThenInclude(r => r.RolePermissions)
                .Where(x => x.CompanyUserId == CompanyUserId).FirstOrDefaultAsync();

            return cur;
        }

        public async Task<List<CompayUserRole>> GetCompanyUserRoleByUser(string UserName)
        {
            return await context.CompayUserRole
                .Include(x => x.Role)
                .Include(x => x.CompanyUser)
                    .ThenInclude(x => x.User)
                .Where(x => x.CompanyUser.User.UserName == UserName).ToListAsync();
        }

        public async Task<List<int>> GetUserRoleIdsByUserNameAsync(string userName)
        {
            var userRoleIds = await context.CompayUserRole
                .Where(cur => cur.CompanyUser.User.UserName == userName)
                .Select(cur => cur.RoleId)
                .ToListAsync();

            return userRoleIds;
        }

        public async Task<CompayUserRole> CreateCompanyUserRole(int companyUserId, int roleId)
        {
            try
            {
                context.ChangeTracker.Clear();

                var companyUserRole = new CompayUserRole
                {
                    CompanyUserId = companyUserId,
                    RoleId = roleId
                };

                await context.CompayUserRole.AddAsync(companyUserRole);
                await context.SaveChangesAsync();

                return companyUserRole;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the CompanyUserRole.", ex);
            }
        }

        public async Task RemoveCompanyUserRole(int CompanyUserRoleId)
        {
            context.ChangeTracker.Clear();
            var CompanyUserRole = await context.CompayUserRole.Where(x => x.Id == CompanyUserRoleId).FirstOrDefaultAsync();
            context.CompayUserRole.Remove(CompanyUserRole);
            await context.SaveChangesAsync();
        }
    }
}
