using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClasses
{
    public class CommonTelnyxMessageMedia
    {
        public int Id { get; set; }
        public int TelnyxMessageId { get; set; }
        public CommonTelnyxMessage? TelnyxMessage { get; set; }
        public string? Url { get; set; }               
        public string? ContentType { get; set; }       
        public string? HashSha256 { get; set; }       
        public int Size { get; set; }              

  

    }
}
