using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;
using static Data.CompanyEnums;
using static Data.GeneralEnums;

namespace Data
{
    public class EmailDataLibrary
    {
        public DataContext context { get; }
        static readonly char[] padding = { '=' };

        public EmailDataLibrary(DataContext Context)
        {
            context = Context;
        }

      


    }
}
