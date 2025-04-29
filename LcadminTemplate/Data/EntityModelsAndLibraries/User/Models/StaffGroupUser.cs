using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data
{
    public class StaffGroupUser : BaseModel
    {
        public StaffGroup StaffGroup { get; set; }
        public int StaffGroupId { get; set; }
        public CompanyUser CompanyUser { get; set; }
        public int CompanyUserId { get; set; }
    }
}
