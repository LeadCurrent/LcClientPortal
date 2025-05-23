﻿using System;
using System.Collections.Generic;

namespace Data;

public partial class Group
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Copy { get; set; }

    public virtual ICollection<Schoolgroup> Schoolgroups { get; set; } = new List<Schoolgroup>();
}
