﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data;

public partial class Campus
{
    public int Id { get; set; }

    public int Schoolid { get; set; }

    public string Name { get; set; }

    public string Address { get; set; }

    public string City { get; set; }

    public int? PortalStatesid { get; set; }

    public int? Postalcodeid { get; set; }

    public string Campustype { get; set; }

    public bool Active { get; set; }

    public string Copy { get; set; }

    public string Clientid { get; set; }

    [NotMapped]
    public int ZipCount { get; set; }

    public int? oldId { get; set; }
    public virtual ICollection<Campuspostalcode> Campuspostalcodes { get; set; } = new List<Campuspostalcode>();

    public virtual Postalcode Postalcode { get; set; }

    public virtual Scholls School { get; set; }

    public virtual PortalStates PortalStates { get; set; }
}
