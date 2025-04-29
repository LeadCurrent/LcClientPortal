using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data
{
    public class StaffGroupDivision : BaseModel
    {
        public StaffGroup StaffGroup { get; set; }
        public int StaffGroupId { get; set; }
       
    }
}

