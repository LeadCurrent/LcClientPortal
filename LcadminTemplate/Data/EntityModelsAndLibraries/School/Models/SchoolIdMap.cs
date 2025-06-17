using System.ComponentModel.DataAnnotations;

namespace Data
{
    public class SchoolIdMap
    {
        [Key]
        public int Id { get; set; }
        public int OldId { get; set; }

        public int NewId { get; set; }
        public Company Company { get; set; }
        public int CompanyId { get; set; }
    }
}
