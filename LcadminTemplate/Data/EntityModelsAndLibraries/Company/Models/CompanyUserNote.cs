
using static Data.CompanyEnums;

namespace Data
{
    public class CompanyUserNote : BaseModel
    {
        public NoteType NoteType { get; set; }
        public string Note { get; set; }
        public int CompanyUserId { get; set; }
        public CompanyUser CompanyUser { get; set; }
    }
}
