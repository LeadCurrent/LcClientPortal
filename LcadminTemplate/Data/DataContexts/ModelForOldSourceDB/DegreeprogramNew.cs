using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataContexts.ModelForSourceDB
{
    public class DegreeprogramNew
    {
        public int Id { get; set; }

        public int Levelid { get; set; }

        public int Programid { get; set; }

        public string Copy { get; set; }
        
        public virtual ICollection<Campusdegree> Campusdegrees { get; set; } = new List<Campusdegree>();

        public virtual Level Level { get; set; }

        public virtual Program Program { get; set; }
    }
}
