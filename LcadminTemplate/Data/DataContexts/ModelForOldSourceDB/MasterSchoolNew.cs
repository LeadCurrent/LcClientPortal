using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataContexts.ModelForSourceDB
{
    public class MasterSchoolNew
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        public virtual ICollection<MasterSchoolMapping> MasterSchoolMappings { get; set; } = new List<MasterSchoolMapping>();
    }
}
