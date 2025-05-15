using System.ComponentModel.DataAnnotations;

namespace Data
{
    public class OfferIdMap
    {
        [Key]
        public int OldId { get; set; }
        public int NewId { get; set; }
    }
}
