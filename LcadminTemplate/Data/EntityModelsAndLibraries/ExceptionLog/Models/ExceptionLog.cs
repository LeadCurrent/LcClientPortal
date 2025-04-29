using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data
{
    public class ExceptionLog
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime DateTime { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string InnerException { get; set; }
        public string Page { get; set; }
        public string Notes { get; set; }
        public string UserEmailAddress { get; set; }
        public string Action { get; set; }
        public string Model { get; set; }
        public string Controller { get; set; }
        public string URL { get; set; }

    }
}
