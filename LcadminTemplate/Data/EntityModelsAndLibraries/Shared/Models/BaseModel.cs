using System;
using static Data.GeneralEnums;

namespace Data
{
    public class BaseModel
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public Status Status { get; set; }
    }
}
