using System.ComponentModel.DataAnnotations;

namespace Data
{
    public class StateIdMap
    {
        [Key]
        public int OldId { get; set; }
        public int NewId { get; set; }
    }
}