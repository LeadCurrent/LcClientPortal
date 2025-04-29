using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using static Data.GeneralEnums;

namespace Data
{
    public class User : IdentityUser
    {
        public DateTime UpdateDate { get; set; }
        public Boolean SystemAdmin { get; set; }
        public Boolean Admin { get; set; }
        public Boolean PropertyOwner { get; set; }
        public Boolean Developer { get; set; }
        public Status Status { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }      
        public Boolean TemporaryPassword { get; set; }
        public bool IsDeleted { get; set; }
        public string Phone { get; set; }
        public string MobileAuthorizationCode { get; set; }
        public string ForgotPasswordCode { get; set; }
        public int SelectedCompanyId { get; set; }  
        public DateTime LastLoginDate { get; set; }

        [NotMapped]        
        public CompanyUser CompanyUser { get; set; }
        [NotMapped]
        public int? CompanyUserId { get; set; }
        [NotMapped]
        public string CompanyName { get; set; }
      
        public List<CompanyUser> CompanyUsers { get; set; }
        
        public List<UserLoginHistory> userLogin { get; set; }

        public string FullName
        {
            get
            {
                return FirstName + ' ' + LastName;
            }
        }

    }
}
