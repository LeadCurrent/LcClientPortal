using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class SendEmail
    {
        public int Id { get; set; }
        public int CompanyEmailAccountId { get; set; }
        public CompanyEmailAccount CompanyEmailAccount { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsSent { get; set; }

        public int OriginalMessageId { get; set; }
        public List<SendEmailRecipient> SendEmailRecipients { get; set; }

        [NotMapped]
        public bool IsChecked { get; set; }
    }
}
