using System;
using System.Collections.Generic;

namespace Data;

public partial class PortalStates
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Abbr { get; set; }

    public string Country { get; set; }

    public string Timezone { get; set; }

    public int? oldId { get; set; }

    public virtual ICollection<Campus> Campuses { get; set; } = new List<Campus>();
}
