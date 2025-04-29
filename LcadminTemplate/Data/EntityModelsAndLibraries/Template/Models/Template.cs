using CommonClasses;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using static Data.GeneralEnums;

namespace Data
{
    public class Template
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public Status Status { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public TemplateEnums.SampleDropdown Dropdown { get; set; }
        public TemplateEnums.SampleDropdown RadioSelect { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Currency { get; set; }
        public bool CheckBox { get; set; }
        public DateTime? Date { get; set; }

        public string Phone { get; set; }
        public string TextArea { get; set; }
        public string Image { get; set; }
        public string Image2 { get; set; }
        public string Note { get; set; }
        public string Signature { get; set; }
        public string BackGroundColor { get; set; }

        /* Time Fields */
        [NotMapped]
        public DateTime Time { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public string TimeStr { get { return StringFormating.TimeStr(Hour, Minute); } }
        public string TimeDisplay { get { return StringFormating.TimeDisplay(Hour, Minute); } }
    }
}
