using System.ComponentModel.DataAnnotations.Schema;
using static Data.RoleEnums;

namespace Data
{
    public class RolePermission
    {
        public int Id { get; set; }
        public Permission Permission { get; set; }
        public JobAccess JobAccess { get; set; }
        public Access Access { get; set; }
        public Role Role { get; set; }
        public int RoleId { get; set; }
        [NotMapped]
        public int SortOrder { get; set; }
    }
}
