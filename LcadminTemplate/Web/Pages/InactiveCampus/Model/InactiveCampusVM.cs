using Data;
using System.Collections.Generic;

namespace Web
{
    public class InactiveCampusVM
    {
        /* Ajax */
        public string Action { get; set; }
        public bool AjaxUpdate { get; set; }
        public bool MobileApp { get; set; }
        public int Param { get; set; }

        /* Properties */
        public int CompanyId { get; set; }
        public string CampusSearch{ get; set; }

        public Campus Campus { get; set; }
        public List<Campus> InactiveCampuses { get; set; }
    }
}
