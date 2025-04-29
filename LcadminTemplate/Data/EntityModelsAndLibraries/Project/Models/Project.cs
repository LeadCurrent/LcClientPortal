using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data
{
    public class Project : BaseModel
    {
        public string JurisdictionCode { get; set; }
        public string ProjectName { get; set; }
        public string ProjectLongName { get; set; }
    }
}
