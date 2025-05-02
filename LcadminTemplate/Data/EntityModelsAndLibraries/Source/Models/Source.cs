using System;
using System.Collections.Generic;

namespace Data;

public partial class Source
{
    public int Id { get; set; }

    public string Name { get; set; }

    public bool Active { get; set; }

    public string Apikey { get; set; }

    public string Lcsourceid { get; set; }

    public string Lcsiteid { get; set; }

    public string Accesskey { get; set; }
    public int CompanyId { get; set; }
    public Company Company { get; set; }
    public virtual ICollection<Allocation> Allocations { get; set; } = new List<Allocation>();

    public virtual ICollection<Sourceip> Sourceips { get; set; } = new List<Sourceip>();
}
