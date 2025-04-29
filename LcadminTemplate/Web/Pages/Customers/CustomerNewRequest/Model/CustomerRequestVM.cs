
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System;
using System.Linq;
using Data;
using static Data.GeneralEnums;


namespace Web
{
    public class CustomerRequestVM
    {
        /* Ajax */
        public string Action { get; set; }
        public string DivToUpdate { get; set; }
        public int Param { get; set; }
        public int Param2 { get; set; }
        public bool AjaxUpdate { get; set; }
        public bool MobileApp { get; set; }



        /* Models */
        public Customer Customer { get; set; }
        public User User { get; set; }
        public Company Company { get; set; }

        /* Enums */



        /* Lists */
        public List<Company> Companys { get; set; }
        public List<Customer> Customers { get; set; }

        /* Int */

        public int CustomerId { get; set; }
        public int CompanyId {  get; set; } 

        /* String */






        /* Bool */

        public bool ShowCustomerInfo { get; set; }
        public bool UpdateSuccessful { get; set; }


        /* DateTime */

        /* DDLs */

        
        public List<SelectListItem> CustomerDDL
        {
            get
            {
                var DDL = new List<SelectListItem>();

                var item1 = new SelectListItem();
                item1.Value = "0";
                item1.Text = "";
                DDL.Add(item1);

                if (Customers != null)
                    foreach (var i in Customers)
                    {
                        var item = new SelectListItem();
                        item.Value = i.Id.ToString();
                        item.Text = i.Name;
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
