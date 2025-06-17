using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataContexts.ModelForOldSourceDB
{
    public class SearchportalsNew
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public bool Active { get; set; }

        public int Rank { get; set; }

        public string Name { get; set; }

        public bool Transfers { get; set; }

        public bool Leads { get; set; }

        public virtual ICollection<Portaltargeting> Portaltargetings { get; set; } = new List<Portaltargeting>();

    }
}
