using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataContexts.ModelForSourceDB
{
    public class SchoolgroupNew
    {
        public int Id { get; set; }

        public int Groupid { get; set; }

        public int Schoolid { get; set; }

        public virtual Group Group { get; set; }

        public virtual Scholls School { get; set; }
    }
}
