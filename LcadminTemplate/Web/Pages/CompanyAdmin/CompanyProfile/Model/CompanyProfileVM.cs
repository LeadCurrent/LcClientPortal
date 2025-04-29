using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using Data;
using System.Linq;
using static Data.GeneralEnums;

namespace Web
{
    public class CompanyProfileVM
    {
        /* Ajax */
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
        public bool ShowUpdateServices { get; set; }
        public bool NewService { get; set; }
        public string NewServiceName { get; set; }
        public int CompanyId { get; set; }
        public int CompanyEmailAccountId { get; set; }
        public string TaxRateId { get; set; }
        public string TaxCodeId { get; set; }
        public string RoleName { get; set; }
        public string Logo { get; set; }
        public string CurrentTab { get; set; }
        public bool ShowEditAccountHolder { get; set; }
        public bool ShowEditBillingContact { get; set; }
        public bool ShowEditSupportContact { get; set; }
        public bool ShowChangePlan { get; set; }
        public bool ShowAddSocialMedia { get; set; }
        public int SelectedAccountHolderId { get; set; }
        public int SelectedBillingContactId { get; set; }
        public int SelectedSupportContactId { get; set; }
        public bool UpdateCCInfo { get; set; }
        public bool UpdateACHInfo { get; set; }
        public bool UpdateCloudliInfo { get; set; }
        public bool ShowAddOtherAccount { get; set; }
        public bool ShowAddStartSyncDate { get; set; }
        public bool ConnectMicrosoftAccount { get; set; }
        public bool ConnectGmailAccount { get; set; }
        public string OtherAccountName { get; set; }
        public string OtherAccountEmail { get; set; }
        public string PhoneNumber { get; set; }
        public string ContactName {  get; set; }
        public bool AddNewContact { get; set; }
        public int EditContactId { get; set; }

        public DateTime? LastSyncDate { get; set; }
        public  bool ShowAddPhoneNumber {  get; set; }


        /* Models */
        public User UserProfile { get; set; }
        public CompayUserRole CompayUserRole { get; set; }
        public Role Role { get; set; }
        public CompanyContact CompanyContact { get; set; }
        //public List<ProductCategory> ProductCategories { get; set; }
        public List<RolePermission> RolePermissions { get; set; }
        public List<CompanyEmailAccount> CompanyEmailAccounts { get; set; }
        public List<CompanyPhoneNumber> CompanyPhoneNumbers { get; set; }

        public List<SelectService> SelectedServices { get; set; }
        public Company Company { get; set; }
        public List<CompanyUser> CompanyUsers { get; set; }
        public List<Role> Roles { get; set; }
        public List<CompayUserRole> CompayUserRoles { get; set; }  

        public List<SelectListItem> StaffDDL
        {
            get
            {
                var DDL = new List<SelectListItem>();
                var itemx = new SelectListItem();
                itemx.Value = "0";
                itemx.Text = "";
                DDL.Add(itemx);

                if (CompanyUsers != null)
                    foreach (var i in CompanyUsers.Where(x => x.Status == Status.Active))
                    {
                        var item = new SelectListItem();
                        item.Value = i.Id.ToString();
                        item.Text = i.User.FullName;
                        DDL.Add(item);
                    }
                return DDL;
            }
        }

        //public int AdditionalUsers
        //{
        //    get
        //    {
        //        var user = 0;
        //        if (Company != null && CompanyUsers?.Count > 0)
        //        {
        //            user = (int)(CompanyUsers.Count - (Company.CompanyIncludedUsers != null ? Company.CompanyIncludedUsers : Company.Plan.StaffIncluded));
        //            return user < 0 ? 0 : user;
        //        }
        //        return 0;
        //    }
        //}

    //    public decimal AdditionalUsersTotal
    //    {
    //        get
    //        {
    //            if (Company != null && CompanyUsers?.Count > 0)
    //            {
    //                return (decimal)(AdditionalUsers * (Company.CompanyAdditionalUserCost != null ? Company.CompanyAdditionalUserCost : Company.Plan.AdditionalStaff));
    //            }
    //            return 0;
    //        }
    //    }
    }
    
    public class SelectService
    {
        public int ServiceId { get; set; }
        public int ContractorServiceId { get; set; }
        public string Service { get; set; }
        public bool Selected { get; set; }
        public bool PreviouslySelected { get; set; }
    }
}


