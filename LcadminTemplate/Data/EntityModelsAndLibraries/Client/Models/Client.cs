using System;
using System.Collections.Generic;

namespace Data;

public partial class Client
{
    public int Id { get; set; }

    public string Name { get; set; }

    public bool Active { get; set; }

    public bool Direct { get; set; }
    public int CompanyId { get; set; }
    public Company Company { get; set; }
    public virtual ICollection<Eduapi> Eduapis { get; set; } = new List<Eduapi>();

    public virtual ICollection<Offer> Offers { get; set; } = new List<Offer>();
}
