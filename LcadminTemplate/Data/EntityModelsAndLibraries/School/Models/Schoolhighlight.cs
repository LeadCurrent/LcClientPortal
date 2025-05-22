using System;
using System.Collections.Generic;

namespace Data;

public partial class Schoolhighlight
{
    public int Id { get; set; }

    public int Schoolid { get; set; }

    public int Number { get; set; }

    public string Text { get; set; }

    public virtual Scholls School { get; set; }
}
