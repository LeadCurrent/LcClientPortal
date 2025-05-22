using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataContexts.ModelForSourceDB
{
    public partial class OffersNew
    {
        public int Id { get; set; }

        public int Schoolid { get; set; }

        public int Clientid { get; set; }

        public string Url { get; set; }

        public bool Active { get; set; }

        public decimal Rpl { get; set; }

        public bool Dcap { get; set; }

        public int Dcapamt { get; set; }

        public bool Mcap { get; set; }

        public int Mcapamt { get; set; }

        public bool Wcap { get; set; }

        public int Wcapamt { get; set; }

        public string Type { get; set; }

        public bool Militaryonly { get; set; }

        public bool Nomilitary { get; set; }

        public string Transferphone { get; set; }

        public string Lccampaignid { get; set; }

        public bool Archive { get; set; }

        public string End_Client { get; set; }

        public decimal cec_rplA { get; set; }

        public decimal cec_rplB { get; set; }

        public decimal cec_rplC { get; set; }

        public decimal cec_rplD { get; set; }

        public decimal cec_rplE { get; set; }

        public decimal cec_rplF { get; set; }

        public decimal cec_rplG { get; set; }

        public string Delivery_Identifier { get; set; }

        public string Delivery_Name { get; set; }       

        public virtual ICollection<Allocation> Allocations { get; set; } = new List<Allocation>();

        public virtual Client Client { get; set; }

        public virtual ICollection<Offertargeting> Offertargetings { get; set; } = new List<Offertargeting>();

        public virtual Scholls School { get; set; }
       
    }
}
