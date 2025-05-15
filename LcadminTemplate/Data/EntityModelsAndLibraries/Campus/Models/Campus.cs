using System;
using System.Collections.Generic;

namespace Data;

public partial class Campus
{
    public int Id { get; set; }

    public int Schoolid { get; set; }

    public string Name { get; set; }

    public string Address { get; set; }

    public string City { get; set; }

    public int? Stateid { get; set; }

    public int? Postalcodeid { get; set; }

    public string Campustype { get; set; }

    public bool Active { get; set; }

    public string Copy { get; set; }

    public string Clientid { get; set; }

    public int? oldId { get; set; }
    public int? CompanyId { get; set; }
    public Company Company { get; set; }

    public virtual ICollection<Campuspostalcode> Campuspostalcodes { get; set; } = new List<Campuspostalcode>();

    public virtual Postalcode Postalcode { get; set; }

    public virtual School School { get; set; }

    public virtual PortalStates PortalStates { get; set; }
}
