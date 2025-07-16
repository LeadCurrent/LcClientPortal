using System.Collections.Generic;
using Data;

namespace Web
{
    public class StudentInterestVM
    {
        /* Ajax */
        public string Action { get; set; }
        public bool AjaxUpdate { get; set; }
        public bool MobileApp { get; set; }
        public int Param { get; set; }

        /* Properties */
        public int CompanyId { get; set; }
        public string InterestSearch { get; set; }
        public bool ShowEditStudentInterest { get; set; }

        //Model
        public Interest StudentInterest { get; set; }
        public List<Interest> StudentInterests { get; set; }
        public Company Company { get; set; }
    }
}
