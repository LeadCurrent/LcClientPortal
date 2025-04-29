using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data
{
    public class Role
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public Company Company { get; set; }
        public int CompanyId { get; set; }
        public List<RolePermission> RolePermissions { get; set; }
    }
}
