using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataContexts.ModelForSourceDB
{
    public class CampuspostalcodeNew
    {
        public int Id { get; set; }

        public int Campusid { get; set; }

        public int Postalcodeid { get; set; }
       
        public virtual Campus Campus { get; set; }

        public virtual Postalcode Postalcode { get; set; }
    }
}
