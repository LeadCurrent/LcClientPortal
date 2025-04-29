using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public static class DateFormat
    {
        public static string ShortDate(DateTime dt)
        {
            string Month = "";
            if (dt.Month == 1)
                Month = "Jan";
            else if (dt.Month == 2)
                Month = "Feb";
            else if (dt.Month == 3)
                Month = "Mar";
            else if (dt.Month == 4)
                Month = "Apr";
            else if (dt.Month == 5)
                Month = "May";
            else if (dt.Month == 6)
                Month = "Jun";
            else if (dt.Month == 7)
                Month = "Jul";
            else if (dt.Month == 8)
                Month = "Aug";
            else if (dt.Month == 9)
                Month = "Sep";
            else if (dt.Month == 10)
                Month = "Oct";
            else if (dt.Month == 11)
                Month = "Nov";
            else if (dt.Month == 12)
                Month = "Dec";

            return Month + " " + dt.Day.ToString();
        }

        public static string ShortDateTime(DateTime dt)
        {
            return dt.Month.ToString() + "/" + dt.Day.ToString() + "/" + (dt.Year - 2000).ToString() + " " + string.Format("{0:h:mm tt}", dt);
        }

        public static string ShortTime(DateTime dt)
        {
            return " " + string.Format("{0:h:mm tt}", dt);
        }
    }
}
