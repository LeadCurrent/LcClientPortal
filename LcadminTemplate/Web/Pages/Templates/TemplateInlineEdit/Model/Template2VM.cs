using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using static Data.GeneralEnums;
using Data;

namespace Web
{
    public class Template2VM
    {
        /* Ajax */
        public string Action { get; set; }
        public int Param { get; set; }
        public bool AjaxUpdate { get; set; }
        public bool MobileApp { get; set; }
        /* Models */
        public Template Template { get; set; }
        public List<Template> Templates { get; set; }

        /* bool Params */
        public bool UpdateSuccessful { get; set; }
        public bool ShowCreateNew { get; set; }

        /* enums */
        public TemplateEnums.SampleDropdown FilterDropdown { get; set; }
        public Status TemplateStatus { get; set; }

        /* ints */
        public int EditTemplateId { get; set; }

        /* strings */
        public string FilterName { get; set; }

        /* dates */

        /* dropdowns */
        public List<SelectListItem> TemplatesDDL
        {
            get
            {
                var DDL = new List<SelectListItem>();
                var item1 = new SelectListItem();
                item1.Value = "0";
                item1.Text = "";
                DDL.Add(item1);

                if (Templates != null)
                    foreach (var i in Templates)
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
