using EllipticCurve.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using static CommonClasses.Enums;
namespace CommonClasses
{
    public static class StringFormating
    {
        public static string ShortDate(DateTime dt)
        {
            return dt.Month.ToString() + "/" + dt.Day.ToString() + "/" + dt.Year.ToString();
        }

        public static string ShortDateWithoutYear(DateTime dt)
        {
            return dt.Month.ToString() + "/" + dt.Day.ToString();
        }

        public static string DateWithDay(DateTime dt)
        {
            var day = "";
            if (dt.DayOfWeek == DayOfWeek.Monday) day = "Mon";
            else if (dt.DayOfWeek == DayOfWeek.Tuesday) day = "Tue";
            else if (dt.DayOfWeek == DayOfWeek.Wednesday) day = "Wed";
            else if (dt.DayOfWeek == DayOfWeek.Thursday) day = "Thu";
            else if (dt.DayOfWeek == DayOfWeek.Friday) day = "Fri";
            else if (dt.DayOfWeek == DayOfWeek.Saturday) day = "Sat";
            else if (dt.DayOfWeek == DayOfWeek.Sunday) day = "Sun";


            return day + " - " + dt.Month.ToString() + "/" + dt.Day.ToString() + "/" + dt.Year.ToString();
        }

        public static string MonthYear(DateTime dt)
        {
            return dt.Month.ToString() + "/" + dt.Day.ToString() + "/" + dt.Year.ToString() + " " + string.Format("{0:h:mm tt}", dt);
        }

        public static string FormatDateTime(DateTime dt)
        {
            return dt.Month.ToString() + "/" + dt.Day.ToString() + "/" + dt.Year.ToString() + " " + string.Format("{0:h:mm tt}", dt);
        }

        public static string FormatDateTime(DateTime? dt)
        {
            if (dt != null)
            {
                var DateTime = (DateTime)dt;
                return DateTime.Month.ToString() + "/" + DateTime.Day.ToString() + "/" + DateTime.Year.ToString() + " " + string.Format("{0:h:mm tt}", DateTime);
            }
            else
                return "";
        }

        public static string FormatDate(DateTime dt)
        {
            return dt.Month.ToString() + "/" + dt.Day.ToString() + "/" + dt.Year.ToString();
        }

        public static string FormatDate(DateTime? dt)
        {
            if (dt != null)
            {
                var DateTime = (DateTime)dt;
                return DateTime.Month.ToString() + "/" + DateTime.Day.ToString() + "/" + DateTime.Year.ToString();
            }
            else
                return "";

        }

        public static string FormatDate1(DateTime dt)
        {
            int day = dt.Day;
            return $"{dt.ToString("MMMM", CultureInfo.InvariantCulture)} {day} {dt.Year}";
        }
        public static string ShortTime(DateTime dt)
        {
            return " " + string.Format("{0:h:mm tt}", dt);
        }

        public static DateTime CurrentTime()
        {
            return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time");
        }

        public static string FileTimeStamp()
        {
            var Time = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time");
            return Time.Year.ToString() + Time.Month.ToString() + Time.Day.ToString() + Time.Hour.ToString() + Time.Minute.ToString() + Time.Second.ToString();
        }

        public static DateTime ConvertUTCToTimeZone(DateTime DT, CompanyTimeZone TZ)
        {
            DT = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DT, "UTC", "Central Standard Time");
            DT.AddHours((int)TZ);
            //if (TZ == CompanyTimeZone.Eastern)
            //    DT = DT.AddHours(-1);
            //if (TZ == CompanyTimeZone.Mountain)
            //    DT = DT.AddHours(1);
            //if (TZ == CompanyTimeZone.Pacific)
            //    DT = DT.AddHours(2);

            return DT;
        }
        public static string DatePicker(DateTime? DateTime)
        {
            if (DateTime == null)
                return "";
            else
            {
                var dt = (DateTime)DateTime;
                var date = dt;
                var year = date.Year.ToString();
                var month = "";
                if (date.Month < 10) { month = "0" + date.Month.ToString(); }
                else { month = date.Month.ToString(); }
                var day = "";
                if (date.Day < 10) { day = "0" + date.Day.ToString(); }
                else { day = date.Day.ToString(); }
                return year + "-" + month + "-" + day;
            }
        }

        public static string TimePicker(TimeOnly? timeOnly)
        {
            if (timeOnly == null)
                return "";
            else
            {
                var dt = (TimeOnly)timeOnly;
                var hour = dt.Hour % 12 == 0 ? 12 : dt.Hour % 12; 
                var minute = dt.Minute.ToString("D2");
                var ampm = dt.Hour >= 12 ? "PM" : "AM"; 
                var a = hour + ":" + minute + " " + ampm;
                return a;

                //return $"{hour}:{minute} {ampm}";
            }
        }


        public static string TimePicker1(TimeOnly? timeOnly)
        {
            if (timeOnly == null)
                return "";
            else
            {
                //var dt = (TimeOnly)timeOnly;
                //var time = dt;
                //var hour = time.Hour.ToString();
                //var minute = time.Minute.ToString();
                //var ampm = "";
                //return dt.ToString();

                return timeOnly.Value.ToString("hh:mm tt");
            }
        }


        public static string FormatCurrency(decimal? dec)
        {
            if (dec != null)
            {
                var Decimal = (Decimal)dec;
                return Decimal.ToString("c");
            }
            else
                return "";
        }

        public static string FormatCurrency(decimal dec)
        {
            return dec.ToString("c");

        }


        public static string FormatBool(bool b)
        {
            if (b) return "Yes";
            else return "No";

        }

        public static string FormatBool(bool? b)
        {
            if (b != null)
            {
                var bo = (Boolean)b;
                if (bo) return "Yes";
                else return "No";
            }
            else
                return "";

        }

        public static string TimeStr(int Hour, int Minute)
        {
            var minute = Minute.ToString();
            var hour = Hour.ToString();
            if (Minute < 10)
                minute = "0" + Minute.ToString();
            if (Hour < 10)
                hour = "0" + Hour.ToString();
            return hour + ":" + minute;
        }

        public static string TimeDisplay(int Hour, int Minute)
        {
            var minute = Minute.ToString();
            if (Minute < 10)
                minute = "0" + Minute.ToString();
            if (Hour == 0)
                return "12:" + minute + " AM";
            else if (Hour < 12)
                return Hour.ToString() + ":" + minute + " AM";
            else if (Hour == 12)
                return Hour.ToString() + ":" + minute + " PM";
            else
                return (Hour - 12).ToString() + ":" + minute + " PM";
        }
    }
}
