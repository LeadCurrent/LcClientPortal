using System;
using System.Collections.Generic;

namespace Data;

public partial class VwAllocationsApi
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
}
