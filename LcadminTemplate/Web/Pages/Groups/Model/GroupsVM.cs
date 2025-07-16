using Data;
using System.Collections.Generic;

namespace Web
{
    public class GroupsVM
    {
        /* Ajax */
        public string Action { get; set; }
        public bool AjaxUpdate { get; set; }
        public bool MobileApp { get; set; }
        public int Param { get; set; }

        /* Properties */
        public int CompanyId { get; set; }
        public string GroupSearch { get; set; }
        public Group Group { get; set; }
        public List<Group> Groups { get; set; }
    }
}
