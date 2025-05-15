using System.ComponentModel.DataAnnotations;

namespace Data
{
    public class SourceIdMap
    {
        [Key]
        public int OldId { get; set; }
        public int NewId { get; set; }
    }
}
