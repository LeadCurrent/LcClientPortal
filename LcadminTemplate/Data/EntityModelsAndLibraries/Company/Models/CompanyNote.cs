using static Data.CompanyEnums;

namespace Data
{
    public class CompanyNote : BaseModel
    {
        public NoteType NoteType { get; set; }
        public string Note { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
