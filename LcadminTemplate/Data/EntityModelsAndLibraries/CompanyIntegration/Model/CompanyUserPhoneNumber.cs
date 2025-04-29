using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class CompanyUserPhoneNumber
    {
        public int Id { get; set; }
        public int CompanyUserId { get; set; }
        public CompanyUser CompanyUser { get; set; }
        public int CompanyPhoneNumberId { get; set; }
        public CompanyPhoneNumber CompanyPhoneNumber { get; set; }
        public bool IsDefault { get; set; }

    }
}
