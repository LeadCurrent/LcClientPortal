using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data;

public partial class Offertargeting
{
    public int Id { get; set; }

    public int Offerid { get; set; }

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

    public bool CecIncludeA { get; set; }

    public bool CecIncludeB { get; set; }

    public bool CecIncludeC { get; set; }

    public bool CecIncludeD { get; set; }

    public bool CecIncludeE { get; set; }

    public bool CecIncludeF { get; set; }

    public bool CecIncludeG { get; set; }

    public decimal CecCplA { get; set; }

    public decimal CecCplB { get; set; }

    public decimal CecCplC { get; set; }

    public decimal CecCplD { get; set; }

    public decimal CecCplE { get; set; }

    public decimal CecCplF { get; set; }

    public decimal CecCplG { get; set; }

    public virtual Offer Offer { get; set; }
    public int? CompanyId { get; set; }
    public Company Company { get; set; }
    public int? oldId { get; set; }

    // --- Monday ---
    [NotMapped]
    public int MondayStartHour { get; set; }

    [NotMapped]
    public int MondayStartMinute { get; set; }

    [NotMapped]
    public int MondayEndHour { get; set; }

    [NotMapped]
    public int MondayEndMinute { get; set; }

    // --- Tuesday ---
    [NotMapped]
    public int TuesdayStartHour { get; set; }

    [NotMapped]
    public int TuesdayStartMinute { get; set; }

    [NotMapped]
    public int TuesdayEndHour { get; set; }

    [NotMapped]
    public int TuesdayEndMinute { get; set; }

    // --- Wednesday ---
    [NotMapped]
    public int WednesdayStartHour { get; set; }

    [NotMapped]
    public int WednesdayStartMinute { get; set; }

    [NotMapped]
    public int WednesdayEndHour { get; set; }

    [NotMapped]
    public int WednesdayEndMinute { get; set; }

    // --- Thursday ---
    [NotMapped]
    public int ThursdayStartHour { get; set; }

    [NotMapped]
    public int ThursdayStartMinute { get; set; }

    [NotMapped]
    public int ThursdayEndHour { get; set; }

    [NotMapped]
    public int ThursdayEndMinute { get; set; }

    // --- Friday ---
    [NotMapped]
    public int FridayStartHour { get; set; }

    [NotMapped]
    public int FridayStartMinute { get; set; }

    [NotMapped]
    public int FridayEndHour { get; set; }

    [NotMapped]
    public int FridayEndMinute { get; set; }

    // --- Saturday ---
    [NotMapped]
    public int SaturdayStartHour { get; set; }

    [NotMapped]
    public int SaturdayStartMinute { get; set; }

    [NotMapped]
    public int SaturdayEndHour { get; set; }

    [NotMapped]
    public int SaturdayEndMinute { get; set; }

    // --- Sunday ---
    [NotMapped]
    public int SundayStartHour { get; set; }

    [NotMapped]
    public int SundayStartMinute { get; set; }

    [NotMapped]
    public int SundayEndHour { get; set; }

    [NotMapped]
    public int SundayEndMinute { get; set; }


}
