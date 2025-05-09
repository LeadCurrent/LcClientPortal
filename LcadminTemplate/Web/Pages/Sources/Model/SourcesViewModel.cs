using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System;
using System.Linq;
using Data;

namespace Web
{
    public class SourcesViewModel
    {
        /* Ajax */
        public string Action { get; set; }
        public string DivToUpdate { get; set; }
        public int Param { get; set; }
        public int Param2 { get; set; }
        public int NewProductCategoryId { get; set; }
        public bool AjaxUpdate { get; set; }
        public bool MobileApp { get; set; }

        /* Models */
        public User User { get; set; }
        public Source Source { get; set; }
        public Company Company { get; set; } 
        public List<Source> Sources { get; set; }

        /* int */
        public int ActiveCount { get; set; }
        public int InactiveCount { get; set; }

        public bool ShowNoListAvailable { get; set; }
        public bool ShowEditSources { get; set; }

        /*string */
        public string SelectedSourceName { get; set; }

        /* DDLs */
    }
}


