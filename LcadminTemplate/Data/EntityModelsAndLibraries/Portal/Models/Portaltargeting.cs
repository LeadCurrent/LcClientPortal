using System;
using System.Collections.Generic;

namespace Data;

public partial class Portaltargeting
{
    public int Id { get; set; }

    public int Portalid { get; set; }

    public bool CitizenIncludeUscitizens { get; set; }

    public bool CitizenIncludePermanentResidents { get; set; }

    public bool CitizenIncludeGreenCardHolders { get; set; }

    public bool CitizenIncludeOther { get; set; }

    public bool InternetIncludeInternet { get; set; }

    public bool InternetIncludeNoInternet { get; set; }

    public bool MilitaryIncludeMilitary { get; set; }

    public bool MilitaryIncludeNonMilitary { get; set; }

    public int StudentMinHighSchoolGradYear { get; set; }

    public int StudentMaxHighSchoolGradYear { get; set; }

    public int StudentMinAge { get; set; }

    public int StudentMaxAge { get; set; }

    public bool LeadIpAddressRequired { get; set; }

    public bool MondayActive { get; set; }

    public DateTime MondayStart { get; set; }

    public DateTime TuesdayStart { get; set; }

    public DateTime WednesdayStart { get; set; }

    public DateTime ThursdayStart { get; set; }

    public DateTime FridayStart { get; set; }

    public DateTime SaturdayStart { get; set; }

    public DateTime SundayStart { get; set; }

    public bool TuesdayActive { get; set; }

    public bool WednesdayActive { get; set; }

    public bool ThursdayActive { get; set; }

    public bool FridayActive { get; set; }

    public bool SaturdayActive { get; set; }

    public bool SundayActive { get; set; }

    public DateTime MondayEnd { get; set; }

    public DateTime TuesdayEnd { get; set; }

    public DateTime WednesdayEnd { get; set; }

    public DateTime ThursdayEnd { get; set; }

    public DateTime FridayEnd { get; set; }

    public DateTime SaturdayEnd { get; set; }

    public DateTime SundayEnd { get; set; }

    //public virtual Searchportal Portal { get; set; }
}
