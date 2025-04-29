using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Data
{
    public class CompanyContact : BaseModel
    {
        /* Basic Info */
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public int CompanyUserId { get; set; }
        public CompanyUser CompanyUser { get; set; }

        public bool PrimaryContact { get; set; }
    }
}
