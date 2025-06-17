using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityModelsAndLibraries.School.Models
{
    public class SchoolGroupsIdMap
    {
        [Key]
        public int Id { get; set; }
        public int OldId { get; set; }
        public int NewId { get; set; }
        public Company Company { get; set; }
        public int CompanyId { get; set; }
    }
}
