﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataContexts.ModelForSourceDB
{
    public partial class ClientsNew
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool Active { get; set; }
        
        public bool Direct { get; set; }
        
    }
}
