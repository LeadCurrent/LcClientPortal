using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data
{
    public class StaffGroup : BaseModel
    {
        public string Name { get; set; }
        public List<StaffGroupUser> StaffGroupUsers { get; set; }
        public List<StaffGroupDivision> StaffGroupDivisions { get; set; }
        public Company Company { get; set; }
        public int CompanyId { get; set; }
     
        public bool Deleted { get; set; }
    }
}
