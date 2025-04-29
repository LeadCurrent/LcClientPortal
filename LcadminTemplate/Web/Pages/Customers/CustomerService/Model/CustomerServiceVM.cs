
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System;
using System.Linq;
using Data;
using static Data.GeneralEnums;


namespace Web
{
    public class CustomerServiceVM
    {
        /* Ajax */
        public string Action { get; set; }
        public string DivToUpdate { get; set; }
        public int Param { get; set; }
        public int Param2 { get; set; }
        public bool AjaxUpdate { get; set; }
        public bool BlockPostBack { get; set; }
        public bool MobileApp { get; set; }


        /* Models */
        public Customer Customer { get; set; }
        public User User { get; set; }


        public Company Company { get; set; }
        

        /* Enums */

        public Status FilterStatus { get; set; }

        /* Lists */

        /* Int */


        /* String */

        public string CurrentTab { get; set; }
        public string ButtonText { get; set; }
        public string FilterName { get; set; }
        public string ButtonUrl { get; set; }
        public string Password { get; set; }

        public string AccountCode { get; set; }

        /* Bool */

        public bool ShowNewContact { get; set; }
        public bool ShowEditContact { get; set; }
        public bool ShowEditCustomer { get; set; }

        public bool ShowCreateCompanyButtons { get; set; }

     

        public bool ShowModelPopup { get; set; }

        /* DateTime */

        /* DDLs */

        //For Apply Filters on Sales Lead





    }
}
