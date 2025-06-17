using System.Collections.Generic;
using Data;

namespace Web
{
    public class AreaOfInterestVM
    {
        /* Ajax */
        public string Action { get; set; }
        public bool AjaxUpdate { get; set; }
        public bool MobileApp { get; set; }
        public int Param { get; set; }

        /* Properties */
        public int CompanyId { get; set; }
        public bool ShowEditAreaOfInterest { get; set; }

        //Model
        public Area AreaOfInterest { get; set; }
        public List<Area> AreaOfInterests { get; set; }
        public Company Company { get; set; }
    }
}
