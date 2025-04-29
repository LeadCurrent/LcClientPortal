using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data
{
    public static class CurrencyFunctions
    {
        public static string DisplayCurrency(decimal amount)
        {
            if(amount == 0)
            {
                return "$0.00";
            }
            decimal dollars = Convert.ToInt32(Math.Floor(amount));
            decimal cents = amount - dollars;
            if(cents == 0)
            {
                return "$" + dollars.ToString() + ".00";
            }
            string centsstr = cents.ToString();
            centsstr = centsstr.Substring(2, centsstr.Length - 2);
            if (centsstr.Length == 1)
            {
                centsstr = "0" + centsstr;
            }
            return "$" + dollars.ToString() + "." + centsstr;

        }
    }
}
