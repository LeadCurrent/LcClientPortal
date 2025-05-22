using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataContexts.ModelForSourceDB
{
    public class ProgramareaNew
    {
        public int Id { get; set; }

        public int Programid { get; set; }

        public int Areaid { get; set; }

        public virtual Area Area { get; set; }

        public virtual Program Program { get; set; }
    }
}
