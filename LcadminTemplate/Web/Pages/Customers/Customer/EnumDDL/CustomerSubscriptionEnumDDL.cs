using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Web
{
    public static class CustomerSubscriptionEnumDDL
    {
      

        public static List<SelectListItem> CashCheckDDL()
        {
            var ddl = new List<SelectListItem>()
                  {
                       new SelectListItem() { Text = "Cash", Value = "1"},
                       new SelectListItem() { Text = "Check", Value = "2"}
                 };
            return ddl;
        }
        public static List<SelectListItem> PaymentMethodOnFileCashCheckDDL()
        {
            var ddl = new List<SelectListItem>()
                  {
                       new SelectListItem() { Text = "Payment Method On File", Value = "0"},
                       new SelectListItem() { Text = "Cash", Value = "1"},
                       new SelectListItem() { Text = "Check", Value = "2"}
                 };
            return ddl;
        }
        public static List<SelectListItem> FrequencyDayOfTheWeekDDL()
        {
            var ddl = new List<SelectListItem>()
                  {
                       new SelectListItem() { Text = "", Value = "0"},
                       new SelectListItem() { Text = "Monday", Value = "1"},
                       new SelectListItem() { Text = "Tuesday", Value = "2"},
                       new SelectListItem() { Text = "Wednesday", Value = "3"},
                       new SelectListItem() { Text = "Thursday", Value = "4"},
                       new SelectListItem() { Text = "Friday", Value = "5"},
                       new SelectListItem() { Text = "Saturday", Value = "6"},
                       new SelectListItem() { Text = "Sunday", Value = "7"},
                 };
            return ddl;
        }

        public static List<SelectListItem> FrequencyMonthDDL()
        {
            var ddl = new List<SelectListItem>()
                  {
                       new SelectListItem() { Text = "January", Value = "0"},
                       new SelectListItem() { Text = "February", Value = "1"},
                       new SelectListItem() { Text = "March", Value = "2"},
                       new SelectListItem() { Text = "April", Value = "3"},
                       new SelectListItem() { Text = "May", Value = "4"},
                       new SelectListItem() { Text = "June", Value = "5"},
                       new SelectListItem() { Text = "July", Value = "6"},
                       new SelectListItem() { Text = "August", Value = "7"},
                       new SelectListItem() { Text = "September", Value = "8"},
                       new SelectListItem() { Text = "October", Value = "9"},
                       new SelectListItem() { Text = "November", Value = "10"},
                       new SelectListItem() { Text = "December", Value = "11"},
                 };
            return ddl;
        }

        public static List<SelectListItem> BillingStartDDL()
        {
            var ddl = new List<SelectListItem>()
                  {
                       new SelectListItem() { Text = "Immediately", Value = "0"},
                       new SelectListItem() { Text = "Specify Start Date", Value = "1"}
                 };
            return ddl;
        }


        public static List<SelectListItem> BillingEndDDL()
        {
            var ddl = new List<SelectListItem>()
                  {
                       new SelectListItem() { Text = "Never", Value = "0"},
                       new SelectListItem() { Text = "Specify End Date", Value = "1"},
                       
                 };
            return ddl;
        }

       
    }
}

