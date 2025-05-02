using System;
using System.Collections.Generic;

namespace Data;

public partial class VwOffersApi
{
    public int Id { get; set; }
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
