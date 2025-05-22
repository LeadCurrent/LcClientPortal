using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataContexts.ModelForSourceDB
{
    public class DownSellOfferPostalCodeNew
    {
        public int Id { get; set; }

        public int DownSellOfferId { get; set; }

        public int Postalcodeid { get; set; }
    }
}
