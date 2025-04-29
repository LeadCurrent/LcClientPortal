using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static Data.GeneralEnums;
using Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace Web
{
    public class UserVM
    {
        /*Ajax*/
        public string Action { get; set; }
        public int  Param { get; set; }
        public string StringParam { get; set; }
        public bool AjaxUpdate { get; set; }
        public bool BlockPostBack { get; set; }
        public int RemoveId { get; set; }
        public bool MobileApp { get; set; }
        /*Models*/
        public User User { get; set; }
        public CompanyUser CompanyUser { get; set; }

        public List<User> Users { get; set; }
        public List<Company> Companys { get; set; }
        public List<CompanyUser> CompanyUsers { get; set; }
        public List<int> AllSelectedRoles { get; set; }
        public List<CompayUserRole> CompanyUserRoles { get; set; }
        public List<CompanyEmailAccount> CompanyEmailAccounts { get; set; }
        public List<CompanyEmailAccount> UserEmailAccounts { get; set; }
        public List<CompanyUserEmail> CompanyUserEmail { get; set; }
        public List<Role> Roles { get; set; }
        public List<CompanyPhoneNumber> CompanyPhoneNumbers { get; set; }
      
        public Company Company { get; set; }

        /* strings */
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public string Id { get; set; }
        public string UserName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string FilterName { get; set; }
        public string OtherAccountName { get; set; }
        public string OtherAccountEmail { get; set; }
        public string[] CompanyRoleGroup { get; set; }
        public string PhoneNumber { get; set; }
        public string ContactName {  get; set; }
        public bool ShowHistory {  get; set; }

        /*bool*/
        public bool PasswordInvalid { get; set; }
        public bool InvalidLogin { get; set; }
        public bool PasswordDoesNotMatch { get; set; }
        public bool UpdateSuccessful { get; set; }
        public bool CreateSuccessful { get; set; }
        public bool PasswordSentViaText { get; set; }
        public bool PasswordSentViaEmail { get; set; }
        public bool NoMatchEmail { get; set; }
        public bool NoMatchText { get; set; }
        public bool RememberMe { get; set; }
        public bool SendTempPasswordViaEmail { get; set; }
        public bool SendTempPasswordViaText { get; set; }
        public string ErrorMessage { get; set; }
        public Status FilterStatus { get; set; }
        public bool ShowAddOtherAccount { get; set; }
        public bool ConnectMicrosoftAccount {  get; set; }
        public bool ConnectGmailAccount {  get; set; }
        public bool ShowAddStartSyncDate { get; set; }
        public bool ShowExistingAccount { get; set; }
        public bool isAdmin { get; set; }
        public bool passwordsent { get; set; }
        public bool nomatch { get; set; }
        public bool IsSystemAdmin { get; set; }
        public bool ShowAddPhoneNumber { get; set; }
        public bool ShowExistingPhoneNumber { get; set; }
        public bool ShowEdit { get; set; }

        /*int*/
        public int CompanyId { get; set; }
        public int SelectedRole { get; set; }

        public DateTime? LastSyncDate { get; set; }

        public List<SelectListItem> RoleDDL
        {
            get
            {
                var DDL = new List<SelectListItem>();
                var item1 = new SelectListItem();
                item1.Value = "0";
                item1.Text = "";
                DDL.Add(item1);

                if (Roles != null)
                    foreach (var i in Roles)
                    {
                        var item = new SelectListItem();
                        item.Value = i.Id.ToString();
                        item.Text = i.RoleName;
                        DDL.Add(item);
                    }
                return DDL;
            }
        }

      

        public List<SelectListItem> CompanyDDL
        {
            get
            {
                var DDL = new List<SelectListItem>();
                var item1 = new SelectListItem();
                item1.Value = "0";
                item1.Text = "";
                DDL.Add(item1);

                if (Companys != null)
                    foreach (var i in Companys)
                    {
                        var item = new SelectListItem();
                        item.Value = i.Id.ToString();
                        item.Text = i.Name;
                        DDL.Add(item);
                    }
                return DDL;
            }
        }
    }
}
