using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using static Data.GeneralEnums;

namespace Data
{
    public class CompanyUser
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public Company Company { get; set; }
        public int CompanyId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public string UserId { get; set; }
        public Status Status { get; set; }
        public bool CompanyAdmin { get; set; }
        public bool Deleted { get; set; }
        [NotMapped]
        public bool Selected { get; set; }
        public List<CompayUserRole> CompanyUserRoles { get; set; }
    }
}
