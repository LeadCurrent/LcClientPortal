using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataContexts.ModelForSourceDB
{
    public class CampusdegreeNew
    {
        public int Id { get; set; }

        public int Campusid { get; set; }

        public int Degreeid { get; set; }

        public string Name { get; set; }

        public string Copy { get; set; }

        public bool Active { get; set; }

        public string Clientid { get; set; }
       
        public virtual ICollection<Allocationcampusdegree> Allocationcampusdegrees { get; set; } = new List<Allocationcampusdegree>();

        public virtual Degreeprogram Degree { get; set; }
    }
}
