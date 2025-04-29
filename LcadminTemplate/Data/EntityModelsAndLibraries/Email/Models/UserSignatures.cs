using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class UserSignatures
    {
        public int Id { get; set; }
        public string UserId {  get; set; }
        public int CompanyEmailAccountId { get; set; }
        public string Signature {  get; set; }


    }
}
