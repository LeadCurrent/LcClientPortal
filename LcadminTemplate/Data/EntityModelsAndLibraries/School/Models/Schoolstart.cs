using System;
using System.Collections.Generic;

namespace Data;

public partial class Schoolstart
{
    public int Id { get; set; }

    public int Schoolid { get; set; }

    public DateTime Date { get; set; }

    public string Name { get; set; }

    public virtual School School { get; set; }
}
