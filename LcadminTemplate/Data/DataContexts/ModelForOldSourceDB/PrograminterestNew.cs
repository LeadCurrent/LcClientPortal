using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataContexts.ModelForSourceDB
{
    public class PrograminterestNew
    {
        public int Id { get; set; }

        public int Programid { get; set; }

        public int Interestid { get; set; }

        public virtual Interest Interest { get; set; }

        public virtual Program Program { get; set; }
       
    }
}
