using System;
using System.Collections.Generic;

namespace Data;

public partial class DownSellOfferPostalCode
{
    public int Id { get; set; }

    public int DownSellOfferId { get; set; }

    public int Postalcodeid { get; set; }
    public int? oldId { get; set; }

    public int? CompanyId { get; set; }
    public Company Company { get; set; }
}
