using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataContexts.ModelForSourceDB
{
    public class MasterSchoolMappingNew
    {
        public int Id { get; set; }

        public int master_schools_id { get; set; }

        public string Identifier { get; set; }
        

        //public virtual MasterSchool MasterSchools { get; set; }
    }
}
