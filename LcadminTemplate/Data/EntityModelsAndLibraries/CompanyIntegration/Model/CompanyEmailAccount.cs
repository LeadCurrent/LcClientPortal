using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Data.CompanyEnums;

namespace Data
{
    public class CompanyEmailAccount
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public EmailType EmailType { get; set; }
        public string RefreshToken { get; set; }
        public bool IsDefault { get; set; }
        public bool AllStaffAccess { get; set; }
        public DateTime LastSyncDate { get; set; }
        public string CalendarColor { get; set; }

        [NotMapped]
        public string Error { get; set; }

    }
}
