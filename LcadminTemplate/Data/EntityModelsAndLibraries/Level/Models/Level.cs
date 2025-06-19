using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data;

public partial class Level
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Copy { get; set; }
    public int? oldId { get; set; }  

    [NotMapped]
    public bool IsChecked{ get; set; }

    public virtual ICollection<Degreeprogram> Degreeprograms { get; set; } = new List<Degreeprogram>();
}
