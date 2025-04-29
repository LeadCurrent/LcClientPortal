using Data;
using DocumentFormat.OpenXml.Math;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Web;

namespace Data
{
    public class CompanyViewModel
    {
        /* Ajax */
        public string Action { get; set; }
        public int Param { get; set; }
        public bool AjaxUpdate { get; set; }
        public string StringParam { get; set; }
        public bool MobileApp { get; set; }
        public bool CustomerView { get; set; }

        /* Models */
        public Company Company { get; set; }
        public User User { get; set; }
        public Customer Customer { get; set; }


        public CompanyContact CompanyContact { get; set; }
        /* Lists */
        public List<Company> Companys { get; set; }
        public List<CompanyUser> CompanyUsers { get; set; }
        public List<User> Users { get; set; }
        public List<CompanyNote> CompanyNotes { get; set; }
        /* Strings */
        public string EmailHTML { get; set; }
        public string ButtonURL { get; set; }
        public string Password { get; set; }
        public string AddNote { get; set; }
        public string AdditionalUserName { get; } = "Additional Users";
        public string FilterName { get; set; }
        public string CurrentTab { get; set; }
        public string InvalidUser { get; set; }
        public string ReportBase64String { get; set; }
        [NotMapped]
        public string InvalidOfferCode { get; set; }

        public string SelectedPaymentOption { get; set; }
        /* Bool */
        public bool UpdateSuccessful { get; set; }
        public bool SystemAdmin { get; set; }
        public bool ShowEditCompanyDetails { get; set; }
        public bool ShowEditAccountHolder { get; set; }
        public bool ShowEditBillingContact { get; set; }
        public bool ShowEditSupportContact { get; set; }
        public bool ShowPlan { get; set; }
        public bool ShowEnterprisePlan { get; set; }
        public bool UpdateCCInfo { get; set; }
        public bool UpdateACHInfo { get; set; }
        public bool UpdateCloudliInfo { get; set; }
        public bool HasBillingInforation { get; set; }
        public bool PaymentUnsuccessful { get; set; }
        public bool ShowUpdateBilling { get; set; }
        public bool ShowAddNote { get; set; }
        public bool AddNewContact { get; set; }
        public bool PrimaryContact { get; set; }
        public bool BillingContact { get; set; }
        public bool SupportContact { get; set; }
        public bool BillingDateUpdated { get; set; }
        /* Int */
        public int EditContactId { get; set; }
        public int CompanyUserId { get; set; }
        public int SelectedCompanyId { get; set; }
        public int SelectedUser { get; set; }
        public int SelectedAccountHolderId { get; set; }
        public int SelectedBillingContactId { get; set; }
        public int SelectedSupportContactId { get; set; }
        public int FilterStatus { get; set; }
        public int PlanQuantity { get; } = 1;
        public int CompanyCustomerId { get; set; }
        /* DDLs */
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
        public List<SelectListItem> StatusDDL
        {
            get
            {
                var DDL = new List<SelectListItem>();

                var item2 = new SelectListItem();
                item2.Value = "0";
                item2.Text = "Active";
                DDL.Add(item2);

                var item3 = new SelectListItem();
                item3.Value = "1";
                item3.Text = "Inactive";
                DDL.Add(item3);

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

        //public decimal AdditionalUsersTotal
        //{
        //    get
        //    {
        //        if (Company != null && CompanyUsers?.Count > 0 && Company.Plan != null)
        //        {
        //            if (AdditionalUsers == 0)
        //                return 0;
        //            else
        //            {
        //                if ((Decimal)Company.Plan.AdditionalStaff > 0)
        //                    return AdditionalUsers * (Decimal)Company.Plan.AdditionalStaff;
        //            }
        //        }
        //        return 0;
        //    }
        //}

    }
}

