using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using Data;
using System.Linq;
using static Data.GeneralEnums;

namespace Web
{
    public class CompanyAccountVM
    {
        /* Ajax */
        public string Action { get; set; }
        public int RemoveId { get; set; }
        public bool AjaxUpdate { get; set; }
        public bool MobileApp { get; set; }
        public int Param { get; set; }

        /* Properties */
        public int CompanyId { get; set; }

        public string CurrentTab { get; set; }
        public bool ShowChangePlan { get; set; }



        /* Models */
           
        //public List<ProductCategory> ProductCategories { get; set; }
      
        public Company Company { get; set; }
        public List<CompanyUser> CompanyUsers { get; set; }
            
    }

}


