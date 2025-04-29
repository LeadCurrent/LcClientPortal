using System.Collections.Generic;
namespace Data
{
    public class CompanyFolder : BaseModel
    {
        public string Name { get; set; }
        public List<Document> Documents { get; set; }
        public List<FolderAccess> FolderAccesses { get; set; }
        public int ParentFolderId { get; set; } 
        public Company Company { get; set; }
        public int CompanyId { get; set; }
        public int Order {  get; set; } 
        public bool AllUserAccess {  get; set; }    
        public bool RestrictAccess {  get; set; }   

    }
}
