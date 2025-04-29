using System.Collections.Generic;
using Data;

namespace Web
{
    public class TemplateWithTabsVM
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
        public bool Edit { get; set; }

        /* enums */

        /* ints */

        /* strings */
        public string CurrentTab { get; set; }


        /* dates */

        /* dropdowns */
    }
}
