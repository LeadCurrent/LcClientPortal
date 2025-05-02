using System;
using System.Collections.Generic;

namespace Data;

public partial class VwAllSubmission
{
    public int Id { get; set; }
    public string Submissiontype { get; set; }

    public int? Offerid { get; set; }

    public int? Sourceid { get; set; }

    public DateOnly? Submisiondate { get; set; }
}
