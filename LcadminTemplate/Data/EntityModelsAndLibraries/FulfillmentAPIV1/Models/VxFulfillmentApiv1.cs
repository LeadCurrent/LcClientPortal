using System;
using System.Collections.Generic;

namespace Data;

public partial class VxFulfillmentApiv1
{
    public int Id { get; set; } 
    public int AllocationId { get; set; }

    public int OfferId { get; set; }

    public int SourceId { get; set; }

    public string OfferType { get; set; }

    public string Vendor { get; set; }

    public bool VendorStatus { get; set; }

    public decimal VendorCpl { get; set; }

    public int VendorMonthlyCap { get; set; }

    public int VendorDailyCap { get; set; }

    public string ClientName { get; set; }

    public string SchoolName { get; set; }

    public bool OfferStatus { get; set; }

    public int OfferMonthlyCap { get; set; }

    public int OfferDailyCap { get; set; }

    public int OfferWeeklyCap { get; set; }

    public decimal ClientRpl { get; set; }

    public int? LeadsDelivered { get; set; }
}
