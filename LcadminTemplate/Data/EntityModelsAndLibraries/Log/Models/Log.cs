using System;
using System.Collections.Generic;

namespace Data;

public partial class Log
{
    public int Id { get; set; }

    public string Csfile { get; set; }

    public string Error { get; set; }

    public string Exceptionmsg { get; set; }

    public string Innerexception { get; set; }

    public DateTime Date { get; set; }
}
