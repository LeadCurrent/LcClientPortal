using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataContexts.ModelForOldSourceDB
{
    public class OffertargetingNew
    {
        public int Id { get; set; }

        public int Offerid { get; set; }

        public bool Citizen_IncludeUscitizens { get; set; }

        public bool Citizen_IncludePermanentResidents { get; set; }

        public bool Citizen_IncludeGreenCardHolders { get; set; }

        public bool Citizen_IncludeOther { get; set; }

        public bool Internet_IncludeInternet { get; set; }

        public bool Internet_IncludeNoInternet { get; set; }

        public bool Military_IncludeMilitary { get; set; }

        public bool Military_IncludeNonMilitary { get; set; }

        public int Student_MinHighSchoolGradYear { get; set; }

        public int Student_MaxHighSchoolGradYear { get; set; }

        public int Student_MinAge { get; set; }

        public int Student_MaxAge { get; set; }

        public bool Lead_IpAddressRequired { get; set; }

        public bool Monday_Active { get; set; }

        public DateTime Monday_Start { get; set; }

        public DateTime Tuesday_Start { get; set; }

        public DateTime Wednesday_Start { get; set; }

        public DateTime Thursday_Start { get; set; }

        public DateTime Friday_Start { get; set; }

        public DateTime Saturday_Start { get; set; }

        public DateTime Sunday_Start { get; set; }

        public bool Tuesday_Active { get; set; }

        public bool Wednesday_Active { get; set; }

        public bool Thursday_Active { get; set; }

        public bool Friday_Active { get; set; }

        public bool Saturday_Active { get; set; }

        public bool Sunday_Active { get; set; }

        public DateTime Monday_End { get; set; }

        public DateTime Tuesday_End { get; set; }

        public DateTime Wednesday_End { get; set; }

        public DateTime Thursday_End { get; set; }

        public DateTime Friday_End { get; set; }

        public DateTime Saturday_End { get; set; }

        public DateTime Sunday_End { get; set; }

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

        public virtual Offer Offer { get; set; }
    }
}
