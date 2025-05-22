using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataContexts.ModelForSourceDB
{
    public partial class PostalcodesNew
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string City { get; set; }

        public int Stateid { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }


        public virtual ICollection<Campus> Campuses { get; set; } = new List<Campus>();

        public virtual ICollection<Campuspostalcode> Campuspostalcodes { get; set; } = new List<Campuspostalcode>();
    }

}
