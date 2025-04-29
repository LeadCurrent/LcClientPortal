using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class CompanyUserEmail
    {
        public int Id { get; set; }
        public int CompanyUserId { get; set; }
        public CompanyUser CompanyUser { get; set; }
        public int CompanyEmailAccountId { get; set; }
        public CompanyEmailAccount CompanyEmailAccount { get; set; }
        public bool IsDefault { get; set; }
    }
}
