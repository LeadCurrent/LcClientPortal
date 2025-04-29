using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class UnreadEmailSummaryView
    {
        public int Id { get; set; }
        public int CompanyUserId { get; set; }
        public CompanyUser CompanyUser { get; set; }
        public int AllEmails { get; set; }

        public int AllContacts { get; set; }

        public int Customers { get; set; }

        public int Leads { get; set; }

        public int Vendors { get; set; }

        public int Notifications { get; set; }

        public int NonContacts { get; set; }

        public int MyCompany { get; set; }
    }
}
