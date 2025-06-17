using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data;

public partial class Program
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Copy { get; set; }

    public int? oldId { get; set; }
    public virtual ICollection<Degreeprogram> Degreeprograms { get; set; } = new List<Degreeprogram>();

    public virtual ICollection<Programarea> ProgramAreas { get; set; } = new List<Programarea>();

    public virtual ICollection<Programinterest> Programinterests { get; set; } = new List<Programinterest>();
    [NotMapped]
    public virtual ICollection<Area> Areas { get; set; } = new List<Area>();
    [NotMapped]

    public virtual ICollection<Level> Levels { get; set; } = new List<Level>();
    [NotMapped]

    public virtual ICollection<Interest> Interests { get; set; } = new List<Interest>();
}
