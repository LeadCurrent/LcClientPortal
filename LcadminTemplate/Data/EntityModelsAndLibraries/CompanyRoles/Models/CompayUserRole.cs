using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data
{
    public class CompayUserRole
    {
        public int Id { get; set; }
        public Role Role { get; set; }
        public int RoleId { get; set; }
        public CompanyUser CompanyUser { get; set; }
        public int CompanyUserId { get; set; }
    }
}
