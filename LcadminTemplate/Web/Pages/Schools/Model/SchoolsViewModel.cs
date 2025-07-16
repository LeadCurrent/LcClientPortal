using System.Collections.Generic;
using System;
using System.Linq;
using Data;

namespace Web
{
    public class SchoolsViewModel
    {
        public string Action { get; set; }
        public string DivToUpdate { get; set; }
        public int Param { get; set; }
        public int Param2 { get; set; }
        public int NewProductCategoryId { get; set; }
        public string SelectedCampusType { get; set; }
        public bool AjaxUpdate { get; set; }
        public bool MobileApp { get; set; }

        /* Models */
        public User User { get; set; }
        public Scholls Scholl { get; set; }
        public Campus Campus { get; set; }
        public Client Client { get; set; }
        public Company Company { get; set; }
        public List<Scholls> Schools { get; set; }

        public Scholls School { get; set; }
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
        public virtual List<Group> Groups { get; set; } = new List<Group>();

        public virtual ICollection<Schoolhighlight> Schoolhighlights { get; set; } = new List<Schoolhighlight>();

        public virtual ICollection<Schoolstart> Schoolstarts { get; set; } = new List<Schoolstart>();
        public bool ShowNoListAvailable { get; set; }
        public bool ShowEditSchools { get; set; }

        /*string */
        public string SelectedSchoolName { get; set; }

        public List<int> SelectedGroupIds => Schoolgroups.Select(g => g.Groupid).ToList();

    }
}
