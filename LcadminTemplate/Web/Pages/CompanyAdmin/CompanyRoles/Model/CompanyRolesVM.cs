using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using Data;
using System.Linq;
using static Data.GeneralEnums;

namespace Web
{
    public class CompanyRolesVM
    {
        /* Ajax */
        public string Action { get; set; }
        public string StringParam { get; set; }
        public int RemoveId { get; set; }
        public bool AjaxUpdate { get; set; }
        public bool MobileApp { get; set; }
        public int Param { get; set; }
        public bool UpdateSuccessful { get; set; }

        /* Properties */
        public int CompanyId { get; set; }
        public string RoleName { get; set; }


        /* Models */
        public Role Role { get; set; }
        public List<RolePermission> RolePermissions { get; set; }
        public Company Company { get; set; }
        public List<Role> Roles { get; set; }
       
       
    }
    
   
}


