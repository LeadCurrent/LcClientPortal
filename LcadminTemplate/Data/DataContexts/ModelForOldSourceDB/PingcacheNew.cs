using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataContexts.ModelForOldSourceDB
{
    public class PingcacheNew
    {
        public int Id { get; set; }

        public string Ping_Signature { get; set; }

        public bool? Allowed { get; set; }

        public DateTime Date { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Ping_Response { get; set; }

        public int Source_Id { get; set; }
   
    }
}
