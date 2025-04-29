
namespace Data
{
    public  class FolderAccess :BaseModel
    {
        public CompanyFolder CompanyFolder { get; set; }
        public int CompanyFolderId {  get; set; }
        public Role Role { get; set; }
        public int RoleId { get; set; }
    }
}
