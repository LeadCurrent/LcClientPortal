using System;
using System.Collections.Generic;

namespace Data;

public partial class PrepingLog
{
    public int? Id { get; set; }

    public string PrepingName { get; set; }

    public string Response { get; set; }

    public DateTime? Date { get; set; }

    public string Email { get; set; }

    public string PingDetail { get; set; }
}
