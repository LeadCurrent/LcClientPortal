using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataContexts.ModelForSourceDB
{
    public partial class States
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Abbr { get; set; }

        public string Country { get; set; }

        public string Timezone { get; set; }
      

        public virtual ICollection<Campus> Campuses { get; set; } = new List<Campus>();
    }

}
