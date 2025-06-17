using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataContexts.ModelForSourceDB
{
    public class GroupNew
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Copy { get; set; }

        public virtual ICollection<Schoolgroup> Schoolgroups { get; set; } = new List<Schoolgroup>();
        
    }
}
