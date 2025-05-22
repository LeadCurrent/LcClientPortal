using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataContexts.ModelForOldSourceDB
{
    public class TblConfigEducationLevelsNew
    {
        public int Id { get; set; }

        public Guid Identifier { get; set; }

        public string Value { get; set; }

        public string Label { get; set; }
    }
}
