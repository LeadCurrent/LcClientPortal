﻿using System;
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
    public int MondayStartHour => MondayStart.Hour;

    [NotMapped]
    public int MondayStartMinute => MondayStart.Minute;

    [NotMapped]
    public int MondayEndHour => MondayEnd.Hour;

    [NotMapped]
    public int MondayEndMinute => MondayEnd.Minute;

    // --- Tuesday ---
    [NotMapped]
    public int TuesdayStartHour => TuesdayStart.Hour;

    [NotMapped]
    public int TuesdayStartMinute => TuesdayStart.Minute;

    [NotMapped]
    public int TuesdayEndHour => TuesdayEnd.Hour;

    [NotMapped]
    public int TuesdayEndMinute => TuesdayEnd.Minute;

    // --- Wednesday ---
    [NotMapped]
    public int WednesdayStartHour => WednesdayStart.Hour;

    [NotMapped]
    public int WednesdayStartMinute => WednesdayStart.Minute;

    [NotMapped]
    public int WednesdayEndHour => WednesdayEnd.Hour;

    [NotMapped]
    public int WednesdayEndMinute => WednesdayEnd.Minute;

    // --- Thursday ---
    [NotMapped]
    public int ThursdayStartHour => ThursdayStart.Hour;

    [NotMapped]
    public int ThursdayStartMinute => ThursdayStart.Minute;

    [NotMapped]
    public int ThursdayEndHour => ThursdayEnd.Hour;

    [NotMapped]
    public int ThursdayEndMinute => ThursdayEnd.Minute;

    // --- Friday ---
    [NotMapped]
    public int FridayStartHour => FridayStart.Hour;

    [NotMapped]
    public int FridayStartMinute => FridayStart.Minute;

    [NotMapped]
    public int FridayEndHour => FridayEnd.Hour;

    [NotMapped]
    public int FridayEndMinute => FridayEnd.Minute;

    // --- Saturday ---
    [NotMapped]
    public int SaturdayStartHour => SaturdayStart.Hour;

    [NotMapped]
    public int SaturdayStartMinute => SaturdayStart.Minute;

    [NotMapped]
    public int SaturdayEndHour => SaturdayEnd.Hour;

    [NotMapped]
    public int SaturdayEndMinute => SaturdayEnd.Minute;

    // --- Sunday ---
    [NotMapped]
    public int SundayStartHour => SundayStart.Hour;

    [NotMapped]
    public int SundayStartMinute => SundayStart.Minute;

    [NotMapped]
    public int SundayEndHour => SundayEnd.Hour;

    [NotMapped]
    public int SundayEndMinute => SundayEnd.Minute;

}
