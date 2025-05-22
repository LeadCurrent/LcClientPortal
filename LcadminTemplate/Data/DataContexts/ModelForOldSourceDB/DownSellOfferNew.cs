using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataContexts.ModelForSourceDB
{
    public class DownSellOfferNew
    {
        public int Id { get; set; }

        public int Clientid { get; set; }

        public string Formurl { get; set; }

        public bool Active { get; set; }

        public int Priority { get; set; }

        public bool Dcap { get; set; }

        public int Dcapamt { get; set; }

        public bool Mcap { get; set; }

        public int Mcapamt { get; set; }

        public bool Wcap { get; set; }

        public int Wcapamt { get; set; }

        public string Type { get; set; }

        public string Transferphone { get; set; }

        public bool IncludeUscitizens { get; set; }

        public bool IncludePermanentResidents { get; set; }

        public bool IncludeGreenCardHolders { get; set; }

        public bool IncludeNonCitizens { get; set; }

        public bool IncludeInternet { get; set; }

        public bool IncludeNoInternet { get; set; }

        public bool IncludeMilitary { get; set; }

        public bool IncludeNonMilitary { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool MondayActive { get; set; }

        public DateTime MondayStartTime { get; set; }

        public DateTime MondayEndTime { get; set; }

        public bool TuesdayActive { get; set; }

        public DateTime TuesdayStartTime { get; set; }

        public DateTime TuesdayEndTime { get; set; }

        public bool WednesdayActive { get; set; }

        public DateTime WednesdayStartTime { get; set; }

        public DateTime WednesdayEndTime { get; set; }

        public bool ThursdayActive { get; set; }

        public DateTime ThursdayStartTime { get; set; }

        public DateTime ThursdayEndTime { get; set; }

        public bool FridayActive { get; set; }

        public DateTime FridayStartTime { get; set; }

        public DateTime FridayEndTime { get; set; }

        public bool SaturdayActive { get; set; }

        public DateTime SaturdayStartTime { get; set; }

        public DateTime SaturdayEndTime { get; set; }

        public bool SundayActive { get; set; }

        public DateTime SundayStartTime { get; set; }

        public DateTime SundayEndTime { get; set; }

        public string Identifier { get; set; }

        public int Minage { get; set; }

        public int Maxage { get; set; }

    }
}
