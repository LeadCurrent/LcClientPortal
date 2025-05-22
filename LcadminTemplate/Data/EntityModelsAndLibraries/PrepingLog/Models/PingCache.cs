using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Data;

public partial class PingCache
{
    public int Id { get; set; }

    public string PingSignature { get; set; }

    public bool? Allowed { get; set; }

    public DateTime Date { get; set; }

    public string Email { get; set; }

    public string Phone { get; set; }

    public string PingResponse { get; set; }

    public int SourceId { get; set; }
    public int? oldId { get; set; }
    public int CompanyId { get; set; }
    public Company Company { get; set; }
    public virtual Source Source { get; set; }
}
