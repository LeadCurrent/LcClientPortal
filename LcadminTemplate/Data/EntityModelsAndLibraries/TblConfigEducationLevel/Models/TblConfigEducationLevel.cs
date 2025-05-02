using System;
using System.Collections.Generic;

namespace Data;

public partial class TblConfigEducationLevel
{
    public int Id { get; set; }

    public Guid Identifier { get; set; }

    public string Value { get; set; }

    public string Label { get; set; }
}
