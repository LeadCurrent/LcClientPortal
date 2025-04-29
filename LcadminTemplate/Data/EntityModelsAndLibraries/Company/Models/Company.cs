using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using static Data.CompanyEnums;

namespace Data
{
    public class 
        Company: BaseModel
    {

        /* Lists */
        public CompanyTimeZone CompanyTimeZone { get; set; }
        public List<CompanyNote> CompanyNotes { get; set; }
        public List<CompanyUser> CompanyUsers { get; set; }
        public List<Customer> Customers { get; set; }
        public List<CompanyContact> CompanyContacts { get; set; }
        public List<CompanyEmailAccount> CompanyEmailAccounts { get; set; }
        public List<CompanyPhoneNumber> CompanyPhoneNumbers { get; set; }

        

        /* Strings */
        public string Website { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string APIKey { get; set; }

        /* Enums */
        public GeneralEnums.State State { get; set; }
       
        /* Bool */
        [NotMapped]
        public bool Selected { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Logo { get; set; }

    }
}
