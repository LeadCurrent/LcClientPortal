using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Data.CompanyEnums;

namespace Data
{ 
    public class EmailRecipient
    {
        public int Id { get; set; }
        public int EmailMessageId { get; set; }
        public EmailMessage EmailMessage { get; set; }
        public RecipientType RecipientType { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
