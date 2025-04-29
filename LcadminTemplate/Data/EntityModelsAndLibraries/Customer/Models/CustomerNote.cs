using static Data.CompanyEnums;

namespace Data
{
    public class CustomerNote : BaseModel
    {
        public string Note { get; set; }
        public Customer Customer { get; set; }
        public int CustomerId { get; set; }
        public NoteType NoteType { get; set; }
    }
}
