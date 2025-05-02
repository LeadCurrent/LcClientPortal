using System;
using System.Collections.Generic;

namespace Data;

public partial class VwFullOfferDetailsApi
{
    public int Id { get; set; }
    public int AllocationId { get; set; }

    public int AllocationSourceId { get; set; }

    public bool AllocationActive { get; set; }

    public string AllocationIdenttifier { get; set; }

    public decimal AllocationCpl { get; set; }

    public bool AllocationHasDailyCap { get; set; }

    public int AllocationDailyCapAmt { get; set; }

    public bool AllocationHasMonthlyCap { get; set; }

    public int AllocationMonthlyCapAmt { get; set; }

    public bool AllocationHasWeeklyCap { get; set; }

    public int AllocationWeeklyCapAmt { get; set; }

    public string SourceName { get; set; }

    public bool SourceActive { get; set; }

    public string SourceApikey { get; set; }

    public string SourceOsAccesskey { get; set; }

    public int OfferId { get; set; }

    public int SchoolId { get; set; }

    public int ClientId { get; set; }

    public bool OfferActive { get; set; }

    public decimal OfferRpl { get; set; }

    public bool OfferHasDailyCap { get; set; }

    public int OfferDailyCapAmt { get; set; }

    public bool OfferHasMonthlyCap { get; set; }

    public int OfferMonthlyCapAmt { get; set; }

    public bool OfferHasWeeklyCap { get; set; }

    public int OfferWeeklyCapAmt { get; set; }

    public string OfferType { get; set; }

    public string OfferDeliveryIdentifier { get; set; }

    public string OfferDeliveryName { get; set; }

    public string ClientName { get; set; }

    public bool ClientActive { get; set; }

    public string OfferName { get; set; }

    public int? OfferMinAge { get; set; }

    public int? OfferMaxAge { get; set; }

    public int? OfferMinHsgradyr { get; set; }

    public int? OfferMaxHsgradyr { get; set; }
}
