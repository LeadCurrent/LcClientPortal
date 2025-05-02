using System;
using System.Collections.Generic;

namespace Data;

public partial class VwVendorAllocation
{
    public int Id { get; set; }
    public decimal Cpl { get; set; }

    public bool Dcap { get; set; }

    public int Dcapamt { get; set; }

    public bool Mcap { get; set; }

    public int Mcapamt { get; set; }

    public bool Wcap { get; set; }

    public int Wcapamt { get; set; }

    public decimal CecCplA { get; set; }

    public decimal CecCplB { get; set; }

    public decimal CecCplC { get; set; }

    public decimal CecCplD { get; set; }

    public decimal CecCplE { get; set; }

    public decimal CecCplF { get; set; }

    public bool Active { get; set; }

    public string Type { get; set; }

    public bool Militaryonly { get; set; }

    public bool Nomilitary { get; set; }

    public string Transferphone { get; set; }

    public string DeliveryIdentifier { get; set; }

    public string DeliveryName { get; set; }

    public string Name { get; set; }

    public string Logo100 { get; set; }

    public int? Minage { get; set; }

    public int? Maxage { get; set; }

    public int? Minhs { get; set; }

    public int? Maxhs { get; set; }

    public string Notes { get; set; }

    public string Shortcopy { get; set; }

    public string Targeting { get; set; }

    public string Accreditation { get; set; }

    public string Highlights { get; set; }

    public string Alert { get; set; }

    public string Disclosure { get; set; }

    public string TcpaText { get; set; }

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

    public decimal OtCecCplA { get; set; }

    public decimal OtCecCplB { get; set; }

    public decimal OtCecCplC { get; set; }

    public decimal OtCecCplD { get; set; }

    public decimal OtCecCplE { get; set; }

    public decimal OtCecCplF { get; set; }

    public decimal OtCecCplG { get; set; }

    public bool SourceActive { get; set; }

    public bool AllocationActive { get; set; }

    public string Accesskey { get; set; }

    public int Offerid { get; set; }
    public int Sourceid { get; set; }

    public int Clientid { get; set; }
}
