using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data
{
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

        [NotMapped]
        public string CustomTargetingNotes
        {
            get => Targeting;
            set => Targeting = value;
        }

        public string StandardTargetingNotes
        {
            get
            {
                var notes = new List<string>();

                if (Startdate >= DateTime.Today)
                {
                    notes.Add($"NEXT start date is {Startdate:MM/dd/yyyy}");
                }

                if (Minage > 0)
                {
                    if (Maxage > 0)
                        notes.Add($"MUST be between {Minage} and {Maxage} years old");
                    else
                        notes.Add($"MUST be older than {Minage} years old");
                }
                else if (Maxage > 0)
                {
                    notes.Add($"MUST be younger than {Maxage} years old");
                }

                if (Minhs > 0)
                {
                    if (Maxhs > 0)
                        notes.Add($"MUST have graduated high school between {Minhs} and {Maxhs}");
                    else
                        notes.Add($"MUST have graduated high school on or after {Minhs}");
                }
                else if (Maxhs > 0)
                {
                    notes.Add($"MUST have graduated high school on or before {Maxhs}");
                }

                return string.Join("<br/>", notes);
            }
        }

    }
}
