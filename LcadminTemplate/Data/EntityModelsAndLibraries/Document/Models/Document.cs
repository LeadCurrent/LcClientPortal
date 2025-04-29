using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using static Data.DocumentEnums;

namespace Data
{
    public class Document : BaseModel
    {

        /* Foreign Keys */
        public int? CustomerId { get; set; }
        public Customer Customer { get; set; }
        public int? CompanyId { get; set; }
        public Company Company { get; set; }


        /* Folders Foreign Keys */
        public int? CompanySubFolderId { get; set; }
        public CompanyFolder CompanyFolder { get; set; }

        /* enums */
        public DocumentFileType DocumentFileType { get; set; }
        public IconType IconType { get; set; }


        /* string */
        public string DocumentName { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string MediaFileURL { get; set; }
        public string LinkURL { get; set; }

        /* bool */
        public bool ControlDocument { get; set; }
        public bool PrimaryVideo { get; set; }

        /* int */
        public int CurrentVersionNumber { get; set; }

        /* Lists */
        public List<DocumentVersion> DocumentVersions { get; set; }

        /* Not Mapped fields */
        [NotMapped]
        public List<CompanyFolder> SubFolderDocuments { get; set; }
        [NotMapped]
        public CompanyFolder ParentFolder { get; set; }

        /* Calculated fields */
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
