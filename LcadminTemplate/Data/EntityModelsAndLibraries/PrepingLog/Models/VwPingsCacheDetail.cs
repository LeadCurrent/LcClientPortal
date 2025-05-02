using System;
using System.Collections.Generic;

namespace Data;

public partial class VwPingsCacheDetail
{
    public int Id { get; set; }
    public bool? Allowed { get; set; }

    public DateTime Date { get; set; }

    public string Email { get; set; }

    public string Phone { get; set; }

    public string PingResponse { get; set; }

    public int SourceId { get; set; }

    public string Name { get; set; }

    public string Accesskey { get; set; }

    public string PingSignature { get; set; }

    public string SchoolName { get; set; }
}
