using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Threading.Tasks;
using static Data.CompanyEnums;

namespace Data
{ 
    public  class SendEmailRecipient
    {
        public int Id { get; set; }
        public int SendEmailId { get; set; }
        public SendEmail SendEmail { get; set; }
        public string Email { get; set; }
        public RecipientType RecipientType { get; set; }
    }
}
