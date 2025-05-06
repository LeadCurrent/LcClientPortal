using Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Web
{
    public class CompanyIntegrationVM
    {
        public string Action { get; set; }
        public string StringParam { get; set; }
        public int RemoveId { get; set; }
        public bool AjaxUpdate { get; set; }
        public bool MobileApp { get; set; }
        public int Param { get; set; }
        public int Param1 { get; set; }
        public string Param2 { get; set; }
        public bool UpdateSuccessful { get; set; }

        /* Properties */      
        public int CompanyId { get; set; }
        public int CompanyEmailAccountId { get; set; }
        public int CompanyUserId { get; set; }

        public string TaxRateId { get; set; }
        public string TaxCodeId { get; set; }
     
        public string CurrentTab { get; set; }    
    
        public bool ShowAddOtherAccount { get; set; }
        public bool ShowAddStartSyncDate { get; set; }
        public bool ConnectMicrosoftAccount { get; set; }
        public bool ConnectGmailAccount { get; set; }
        public bool ShowQuickbooks { get; set; }
        public string OtherAccountName { get; set; }
        public string OtherAccountEmail { get; set; }
        public string PhoneNumber { get; set; }
        public string ContactName { get; set; }
        public bool EditEmailAccount { get; set; }
        public bool EditPhoneNumber { get; set; }
        public bool AllStaffAccess { get; set; }

        public DateTime? LastSyncDate { get; set; }
        public bool ShowAddPhoneNumber { get; set; }


        /* Models */
        public User UserProfile { get; set; } 

        public List<CompanyEmailAccount> CompanyEmailAccounts { get; set; }
        public List<CompanyUserEmail> CompanyUserEmails { get; set; }
        public Company Company { get; set; }
        public CompanyEmailAccount CompanyEmailAccount { get; set; }
        public List<CompanyUser> CompanyUsers { get; set; }

        public List<SelectListItem> CompanyUserDDL
        {
            get
            {
                var DDL = new List<SelectListItem>();
                var item1 = new SelectListItem();
                item1.Value = "0";
                item1.Text = "";
                DDL.Add(item1);

                if (CompanyUsers != null)
                    foreach (var i in CompanyUsers)
                    {
                        var item = new SelectListItem();
                        item.Value = i.Id.ToString();
                        item.Text = i.User.FullName;
                        DDL.Add(item);
                    }
                return DDL;
            }
        }
    }
}

