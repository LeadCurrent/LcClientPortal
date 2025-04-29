using System;
using System.Collections.Generic;
using Data;
using static Data.GeneralEnums;

namespace Web
{
    public class TemplateVM
    {
        /* Ajax */
        public string Action { get; set; }
        public int Param { get; set; }
        public bool AjaxUpdate { get; set; }
        public string DivToUpdate { get; set; }
        public bool BlockPostBack { get; set; }
        public bool ShowModelPopup { get; set; }
        public bool MobileApp { get; set; }
        public bool ShowErrorMsg { get; set; }

        /* Models */
        public Template Template { get; set; }
        public List<Template> Templates { get; set; }
        public List<TemplateMultiSelect> AllSelectedTemplates { get; set; }

        /* bool Params */
        public bool UpdateSuccessful { get; set; }

        /* enums */
        public Status TemplateStatus { get; set; }

        /* ints */
        public TimeOnly SelectedTime { get; set; }

        /* strings */
        public string FilterName { get; set; }
        public string FilterEmail { get; set; }
        public string Image { get; set; }
        public string Image2 { get; set; }
        public string AddNote { get; set; }
        public string Signature { get; set; }

        /* dates */

        /* dropdowns */

    }
}
