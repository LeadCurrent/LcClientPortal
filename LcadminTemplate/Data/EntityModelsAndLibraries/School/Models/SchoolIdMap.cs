using System.ComponentModel.DataAnnotations;

namespace Data
{
    public class SchoolIdMap
    {
        [Key]
        public int OldId { get; set; }

        public int NewId { get; set; }
    }
}
