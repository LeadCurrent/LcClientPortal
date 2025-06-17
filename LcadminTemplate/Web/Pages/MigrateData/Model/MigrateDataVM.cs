using System.Collections.Generic;
using System;
using System.Linq;
using Data;

namespace Web
{
    public class MigrateDataVM
    {
        public string Action { get; set; }
        public string DivToUpdate { get; set; }
        public int Param { get; set; }
        public int Param2 { get; set; }
        public int NewProductCategoryId { get; set; }
        public bool AjaxUpdate { get; set; }
        public bool MobileApp { get; set; }
    }
}
