
using System;

namespace Data
{
    public static class DateTimeFunctions
    {
        public static string ShortDate(DateTime dt)
        {
            return dt.Month.ToString() + "/" + dt.Day.ToString() + "/" + dt.Year.ToString();
        }

        public static string ShortDateTime(DateTime dt)
        {
            return dt.Month.ToString() + "/" + dt.Day.ToString() + "/" + (dt.Year - 2000).ToString() + " " + string.Format("{0:h:mm tt}", dt);
        }

        public static string ShortDateWithoutYear(DateTime dt)
        {
            return dt.Month.ToString() + "/" + dt.Day.ToString();
        }
        public static string EndTimeFormat(int EndHour, string EndMin, string EndAMPM)
        {
            if (EndHour == 0 && EndMin == null && EndAMPM == null)
                return "";
            else
                return EndHour.ToString() + ":" + EndMin + " " + EndAMPM;
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
        public static string FileTimeStamp()
        {
            var Time = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time");
            return Time.Year.ToString() + Time.Month.ToString() + Time.Day.ToString() + Time.Hour.ToString() + Time.Minute.ToString();
        }
        public static string DatePicker(DateTime? DateTime)
        {
            if (DateTime == null)
                return null;
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
        public static string FormatedDateTime(DateTime dt)
        {
            return dt.Month.ToString() + "/" + dt.Day.ToString() + "/" + dt.Year.ToString() + " " + string.Format("{0:h:mm tt}", dt);
        }

        public static string FormatedDate(DateTime dt)
        {
            return dt.Month.ToString() + "/" + dt.Day.ToString() + "/" + dt.Year.ToString();
        }

        public static string ShortTime(DateTime dt)
        {
            return " " + string.Format("{0:h:mm tt}", dt);
        }

        public static DateTime CurrentTime()
        {
            return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time");
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

        public static string GetDayWithSuffix(int day)
        {
            switch (day % 100)
            {
                case 11:
                case 12:
                case 13:
                    return $"{day}th";
            }

            switch (day % 10)
            {
                case 1:
                    return $"{day}st";
                case 2:
                    return $"{day}nd";
                case 3:
                    return $"{day}rd";
                default:
                    return $"{day}th";
            }
        }

        public static string GetCalendarDate(DateTime dt)
        {
            var month = "";
            if (dt.Month == 1) month = "January";
            else if (dt.Month == 2) month = "February";
            else if (dt.Month == 3) month = "March";
            else if (dt.Month == 4) month = "April";
            else if (dt.Month == 5) month = "May";
            else if (dt.Month == 6) month = "June";
            else if (dt.Month == 7) month = "July";
            else if (dt.Month == 8) month = "August";
            else if (dt.Month == 9) month = "September";
            else if (dt.Month == 10) month = "October";
            else if (dt.Month == 11) month = "November";
            else if (dt.Month == 12) month = "December";

            return month + " " + GetDayWithSuffix(dt.Day) + ", " + dt.Year.ToString();
        }

    }
}
