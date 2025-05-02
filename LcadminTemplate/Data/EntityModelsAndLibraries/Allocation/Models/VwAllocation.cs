using System;
using System.Collections.Generic;

namespace Data;

public partial class VwAllocation
{
    public int Id { get; set; }
    public string SchoolName { get; set; }

    public string ClientName { get; set; }

    public string OfferType { get; set; }

    public string SourceName { get; set; }

    public int AllocationId { get; set; }

    public bool AllocationActive { get; set; }

    public decimal AllocationCpl { get; set; }

    public bool AllocationMcap { get; set; }

    public int AllocationMcapamt { get; set; }
}
