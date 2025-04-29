using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class EmailModel
    {
        public string Id { get; set; }
        public int CompanyEmailAccountId { get; set; }
        public CompanyEmailAccount CompanyEmailAccount { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public DateTime? ReceivedDateTime { get; set; }
        public string Subject { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }      
        public bool HasAttachments { get; set; }
        public string Importance { get; set; }
        public bool IsRead { get; set; }
        public bool IsDraft { get; set; }
        public string InternetMessageId { get; set; }
        public List<EmailRecipient> EmailRecipients { get; set; }
    }
}
