
namespace Data
{
    public class DocumentVersion : BaseModel
    {
        public int DocumentId { get; set; }
        public Document Document { get; set; }

        public int VersionNumber { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string MediaFileURL { get; set; }
        public string LinkURL { get; set; }
        public string FileType
        {
            get
            {
                if (FileName != null)
                    if (FileName.Contains("."))
                        return FileName.Split(".")[1];

                return "";
            }
        }

    }
}
