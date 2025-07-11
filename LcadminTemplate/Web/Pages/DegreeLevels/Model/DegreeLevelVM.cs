using Data;
using System.Collections.Generic;

namespace Web
{
    public class DegreeLevelVM
    {

        /* Ajax */
        public string Action { get; set; }
        public bool AjaxUpdate { get; set; }
        public bool MobileApp { get; set; }
        public bool ShowEditDegreeLevel { get; set; }
        public int Param { get; set; }

        /* Properties */
        public int CompanyId { get; set; }
        public string DegreeLevelSearch { get; set; }

        //Model
        public Level DegreeLevel { get; set; }
        public List<Level> DegreeLevels { get; set; }
        public Company Company { get; set; }

    }
}
