using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class EmailMessage
    {
        public int Id { get; set; }
        public int CompanyEmailAccountId { get; set; }
        public CompanyEmailAccount CompanyEmailAccount { get; set; }
        public DateTime ReceivedDateTime { get; set; }
        public DateTime DownloadDateTime { get; set; }
        public string MessageId { get; set; }
        public string Subject { get; set; }
        public string ShortSubject
        {
            get
            {
                if (Subject != null)
                {
                    if (Subject.Length > 60)
                        return Subject.Substring(0, 60) + "...";
                }
                return Subject;
            }
        }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public bool HasAttachments { get; set; }
        public string Importance { get; set; }
        public bool IsRead { get; set; }
        public bool IsDraft { get; set; }
        public string InternetMessageId { get; set; }

        public bool ReplySent {  get; set; }
        public string ReplySentBy {  get; set; }
        public DateTime? ReplySentOn { get; set; }

        [NotMapped]
        public bool IsChecked { get; set; }
        [NotMapped]
        public bool IsCustomer { get; set; }
        [NotMapped]
        public bool IsContact { get; set; }
        [NotMapped]
        public bool IsVendor { get; set; }
        [NotMapped]
        public bool IsLead { get; set; }
        [NotMapped]
        public bool IsTask {  get; set; } 
        [NotMapped]
        public bool IsNotificationEmail {  get; set; }

      
        [NotMapped]
        public bool HasDraft {  get; set; }
        [NotMapped]
        public string Body { get; set; }
        public List<EmailRecipient> EmailRecipients { get; set; }

    }
}
