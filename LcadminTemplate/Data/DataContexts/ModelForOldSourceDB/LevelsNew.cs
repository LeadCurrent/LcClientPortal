using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataContexts.ModelForSourceDB
{
    public class LevelsNew
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Copy { get; set; }


        public virtual ICollection<Degreeprogram> Degreeprograms { get; set; } = new List<Degreeprogram>();
    }
}
