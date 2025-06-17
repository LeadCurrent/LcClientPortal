using System;
using System.Collections.Generic;

namespace Data;

public partial class Scholls
{
    public int Id { get; set; }


    public string Name { get; set; }

    public string Abbr { get; set; }

    public string Website { get; set; }

    public string Logo100 { get; set; }

    public int? Minage { get; set; }

    public int? Maxage { get; set; }

    public int? Minhs { get; set; }

    public int? Maxhs { get; set; }

    public string Notes { get; set; }

    public string Shortcopy { get; set; }

    public string Targeting { get; set; }

    public string Accreditation { get; set; }

    public string Highlights { get; set; }

    public string Alert { get; set; }

    public DateTime Startdate { get; set; }

    public int Scoreadjustment { get; set; }

    public bool Militaryfriendly { get; set; }

    public string Disclosure { get; set; }

    public string Schoolgroup { get; set; }

    public int? oldId { get; set; }
    public string TcpaText { get; set; }
    public virtual ICollection<Campus> Campuses { get; set; } = new List<Campus>();

    public virtual ICollection<Offer> Offers { get; set; } = new List<Offer>();

    public virtual ICollection<Schoolgroup> Schoolgroups { get; set; } = new List<Schoolgroup>();

    public virtual ICollection<Schoolhighlight> Schoolhighlights { get; set; } = new List<Schoolhighlight>();

    public virtual ICollection<Schoolstart> Schoolstarts { get; set; } = new List<Schoolstart>();
}
