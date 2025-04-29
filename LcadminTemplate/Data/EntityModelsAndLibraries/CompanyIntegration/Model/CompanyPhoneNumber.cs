using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class CompanyPhoneNumber
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsDefault { get; set; }
        public bool AllStaffAccess { get; set; }


    }
}
