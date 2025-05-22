using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataContexts.ModelForSourceDB
{
    public class AllocationNew
    {
        public int Id { get; set; }

        public int Offerid { get; set; }

        public int Sourceid { get; set; }

        public bool Active { get; set; }

        public string Identifier { get; set; }

        public decimal Cpl { get; set; }

        public bool Dcap { get; set; }

        public int Dcapamt { get; set; }

        public bool Mcap { get; set; }

        public int Mcapamt { get; set; }

        public bool Wcap { get; set; }

        public int Wcapamt { get; set; }

        public string Transferphone { get; set; }

        public bool Cec_IncludeA { get; set; }

        public bool Cec_IncludeB { get; set; }

        public bool Cec_IncludeC { get; set; }

        public bool Cec_IncludeD { get; set; }

        public bool Cec_IncludeE { get; set; }

        public bool Cec_IncludeF { get; set; }

        public bool Cec_IncludeG { get; set; }

        public decimal Cec_CplA { get; set; }

        public decimal Cec_CplB { get; set; }

        public decimal Cec_CplC { get; set; }

        public decimal Cec_CplD { get; set; }

        public decimal Cec_CplE { get; set; }

        public decimal Cec_CplF { get; set; }

        public decimal Cec_CplG { get; set; }
      
        public virtual ICollection<Allocationcampusdegree> Allocationcampusdegrees { get; set; } = new List<Allocationcampusdegree>();

        public virtual ICollection<Allocationcampus> Allocationcampuses { get; set; } = new List<Allocationcampus>();

        public virtual Offer Offer { get; set; }

        public virtual Source Source { get; set; }
    }
}
