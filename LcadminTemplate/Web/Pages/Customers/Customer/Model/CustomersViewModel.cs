
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System;
using System.Linq;
using Data;
using static Data.GeneralEnums;

namespace Web
{
    public class CustomersViewModel
    {
        /* Ajax */
        public string Action { get; set; }
        public string DivToUpdate { get; set; }
        public int Param { get; set; }
        public int Param2 { get; set; }
        public int NewProductCategoryId { get; set; }
        public bool AjaxUpdate { get; set; }
        public bool MobileApp { get; set; }
        public bool ViewAgreement { get; set; }
        public bool CreateNewAgreegrementSection { get; set; }
        public bool ShowCreateNewInvoice { get; set; }
        public bool PaymentProfileFailed { get; set; }
        public int UpdatePaymentProfileId { get; set; }

        /* Models */
        public Customer Customer { get; set; }
        public User User { get; set; }
        public CustomerNote CustomerNote { get; set; }
        public Document CustomerDocument { get; set; }
        public Company Company { get; set; }

        /* Lists */
        public List<Customer> Customers { get; set; }
        public List<Customer> Customers2 { get; set; }
        public List<CompanyUser> CompanyUsers { get; set; }

        /* Int */
        public int EmailMessageId {  get; set; }

        /* String */
        public string NewTaskComment { get; set; }
        public string SelectedCompanyName { get; set; }
        public string SelectedCityState { get; set; }
        public string SelectedContacted { get; set; }
        public string CurrentTab { get; set; }
        public string ButtonText { get; set; }
        public string ButtonUrl { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string CurrentNotesTab { get; set; }
        public string AddNote { get; set; }
        public string SelectedPaymentOption { get; set; }
        public int SelectedAgreementType { get; set; }
        public int SelectedContactId { get; set; }
        public string Signature { get; set; }
        public string ReportBase64String { get; set; }
        public int SelectedAgreement { get; set; }
        public int SelectedVersion { get; set; }


        /* Bool */
        public bool UpdateSuccessful { get; set; }
        public bool ShowNewNote { get; set; }
        public bool ShowEditNote { get; set; }
        public bool CreateNewAgreegrement { get; set; }
        public bool Unsigned { get; set; }
   
        public bool ShowNewContact { get; set; }
        public bool ShowEditContact { get; set; }
        public bool ShowEditCustomer { get; set; }
        public bool ShowCreateTask { get; set; }
        public bool ShowEditTask { get; set; }
        public bool ShowCreateCompanyButtons { get; set; }
        public bool ShowEditInvoice { get; set; }
        public bool AddPaymentMethod { get; set; }
        public bool UpdatePaymentMethod { get; set; }
        public bool ShowAddCustomerSubscription { get; set; }
        public bool PasswordInvalid { get; set; }
        public bool Emails {  get; set; }
        public bool ShowEditDocument {  get; set; }
        public bool ShowNewDocument {  get; set; }
        public bool ShowCreateAgreement {  get; set; }

        /* DateTime */
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        /* DDLs */
        public List<SelectListItem> UsersDDL
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

        //For Apply Filters on Sales Lead


        public List<SelectListItem> CityStateDDL
        {
            get
            {
                if (Customers2 == null)
                    return null;
                var CityDDL = new List<SelectListItem>();
                var sl = new SelectListItem();
                sl.Text = "";
                sl.Value = "";
                CityDDL.Add(sl);
                foreach (var item in Customers)
                {
                    var sli = new SelectListItem();
                    if (!string.IsNullOrEmpty(item.City))
                    {
                        sli.Text = item.City;   // +", "+ item.State;
                        sli.Value = item.City;  // + ", " + item.State;
                        if (CityDDL.Where(wh => wh.Text == sli.Text).Count() == 0)
                        {
                            CityDDL.Add(sli);
                        }
                    }

                }
                return CityDDL;
            }

        }
        public List<SelectListItem> DayOfMonthDDL
        {
            get
            {
                return new List<SelectListItem>()
                  {
                       new SelectListItem() { Text = "1st", Value = "1"},
                       new SelectListItem() { Text = "2nd", Value = "2"},
                       new SelectListItem() { Text = "3rd", Value = "3"},
                       new SelectListItem() { Text = "4th", Value = "4"},
                       new SelectListItem() { Text = "5th", Value = "5"},
                       new SelectListItem() { Text = "6th", Value = "6"},
                       new SelectListItem() { Text = "7th", Value = "7"},
                       new SelectListItem() { Text = "8th", Value = "8"},
                       new SelectListItem() { Text = "9th", Value = "9"},
                       new SelectListItem() { Text = "10th", Value = "10"},
                       new SelectListItem() { Text = "11th", Value = "11"},
                       new SelectListItem() { Text = "12th", Value = "12"},
                       new SelectListItem() { Text = "13th", Value = "13"},
                       new SelectListItem() { Text = "14th", Value = "14"},
                       new SelectListItem() { Text = "15th", Value = "15"},
                       new SelectListItem() { Text = "16th", Value = "16"},
                       new SelectListItem() { Text = "17th", Value = "17"},
                       new SelectListItem() { Text = "18th", Value = "18"},
                       new SelectListItem() { Text = "19th", Value = "19"},
                       new SelectListItem() { Text = "20th", Value = "20"},
                       new SelectListItem() { Text = "21st", Value = "21"},
                       new SelectListItem() { Text = "22nd", Value = "22"},
                       new SelectListItem() { Text = "23rd", Value = "23"},
                       new SelectListItem() { Text = "24th", Value = "24"},
                       new SelectListItem() { Text = "25th", Value = "25"},
                       new SelectListItem() { Text = "26th", Value = "26"},
                       new SelectListItem() { Text = "27th", Value = "27"},
                       new SelectListItem() { Text = "28th", Value = "28"}
                 };
            }
        }
        public List<SelectListItem> ContactedDDl
        {
            get
            {
                return new List<SelectListItem>()
                {
                    new SelectListItem() { Text = "", Value = ""},
                    new SelectListItem() { Text = "Yes", Value = "1"},
                    new SelectListItem() { Text = "No", Value = "0"}
                };
            }
        }


        public List<SelectListItem> PaymentOptionsDDL
        {
            get
            {
                var DDL = new List<SelectListItem>
        {
            new SelectListItem { Value = "Credit Card", Text = "Pay with Credit Card" },
            new SelectListItem { Value = "Check", Text = "Pay with Check" },
            new SelectListItem { Value = "Cash", Text = "Pay with Cash" }
        };

                return DDL;
            }
        }


    }
}
