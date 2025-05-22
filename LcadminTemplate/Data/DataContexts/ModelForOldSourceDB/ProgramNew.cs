using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataContexts.ModelForSourceDB
{
    public class ProgramNew
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Copy { get; set; }
        
        public virtual ICollection<Degreeprogram> Degreeprograms { get; set; } = new List<Degreeprogram>();

        public virtual ICollection<Programarea> Programareas { get; set; } = new List<Programarea>();

        public virtual ICollection<Programinterest> Programinterests { get; set; } = new List<Programinterest>();
    }
}
