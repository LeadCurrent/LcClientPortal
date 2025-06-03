using Data;
using System.Collections.Generic;

namespace Web
{
    public class ProgramVM
    {
        /* Ajax */
        public string Action { get; set; }
        public bool AjaxUpdate { get; set; }
        public bool MobileApp { get; set; }
        public int Param { get; set; }

        /* Properties */
        public int CompanyId { get; set; }
        public bool ShowEdit { get; set; }

        //Model
        public Program Program { get; set; }
        public List<Program> Programs { get; set; }

    }
}
