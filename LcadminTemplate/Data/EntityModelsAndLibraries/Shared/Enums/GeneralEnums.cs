using System;
using System.Collections.Generic;
using System.Text;
using static Data.GeneralEnums;

namespace Data
{
    public static class GeneralEnums
    {
        public enum Status
        {
            Active = 0,
            Inactive = 1
        }

        public enum State
        {
            Alabama = 1,
            Alaska = 2,
            Arizona = 3,
            Arkansas = 4,
            California = 5,
            Colorado = 6,
            Connecticut = 7,
            Delaware = 8,
            Florida = 9,
            Georgia = 10,
            Hawaii = 11,
            Idaho = 12,
            Illinois = 13,
            Indiana = 14,
            Iowa = 15,
            Kansas = 16,
            Kentucky = 17,
            Louisiana = 18,
            Maine = 19,
            Maryland = 20,
            Massachusetts = 21,
            Michigan = 22,
            Minnesota = 23,
            Mississippi = 24,
            Missouri = 25,
            Montana = 26,
            Nebraska = 27,
            Nevada = 28,
            NewHampshire = 29,
            NewJersey = 30,
            NewMexico = 31,
            NewYork = 32,
            NorthCarolina = 33,
            NorthDakota = 34,
            Ohio = 35,
            Oklahoma = 36,
            Oregon = 37,
            Pennsylvania = 38,
            RhodeIsland = 39,
            SouthCarolina = 40,
            SouthDakota = 41,
            Tennessee = 42,
            Texas = 43,
            Utah = 44,
            Vermont = 45,
            Virginia = 46,
            Washington = 47,
            WestVirginia = 48,
            Wisconsin = 49,
            Wyoming = 50,
            None = 0
        }

        public static string StateDesc(State State)
        {
            if (State == State.Alabama) return "Alabama";
            if (State == State.Alaska) return "Alaska";
            if (State == State.Arizona) return "Arizona";
            if (State == State.Arkansas) return "Arkansas";
            if (State == State.California) return "California";
            if (State == State.Colorado) return "Colorado";
            if (State == State.Connecticut) return "Connecticut";
            if (State == State.Delaware) return "Delaware";
            if (State == State.Florida) return "Florida";
            if (State == State.Georgia) return "Georgia";
            if (State == State.Hawaii) return "Hawaii";
            if (State == State.Idaho) return "Idaho";
            if (State == State.Illinois) return "Illinois";
            if (State == State.Indiana) return "Indiana";
            if (State == State.Iowa) return "Iowa";
            if (State == State.Kansas) return "Kansas";
            if (State == State.Kentucky) return "Kentucky";
            if (State == State.Louisiana) return "Louisiana";
            if (State == State.Maine) return "Maine";
            if (State == State.Maryland) return "Maryland";
            if (State == State.Massachusetts) return "Massachusetts";
            if (State == State.Michigan) return "Michigan";
            if (State == State.Minnesota) return "Minnesota";
            if (State == State.Mississippi) return "Mississippi";
            if (State == State.Missouri) return "Missouri";
            if (State == State.Montana) return "Montana";
            if (State == State.Nebraska) return "Nebraska";
            if (State == State.Nevada) return "Nevada";
            if (State == State.NewHampshire) return "New Hampshire";
            if (State == State.NewJersey) return "New Jersey";
            if (State == State.NewMexico) return "New Mexico";
            if (State == State.NewYork) return "New York";
            if (State == State.NorthCarolina) return "North Carolina";
            if (State == State.NorthDakota) return "North Dakota";
            if (State == State.Ohio) return "Ohio";
            if (State == State.Oklahoma) return "Oklahoma";
            if (State == State.Oregon) return "Oregon";
            if (State == State.Pennsylvania) return "Pennsylvania";
            if (State == State.RhodeIsland) return "Rhode Island";
            if (State == State.SouthCarolina) return "South Carolina";
            if (State == State.SouthDakota) return "South Dakota";
            if (State == State.Tennessee) return "Tennessee";
            if (State == State.Texas) return "Texas";
            if (State == State.Utah) return "Utah";
            if (State == State.Vermont) return "Vermont";
            if (State == State.Virginia) return "Virginia";
            if (State == State.Washington) return "Washington";
            if (State == State.WestVirginia) return "West Virginia";
            if (State == State.Wisconsin) return "Wisconsin";
            if (State == State.Wyoming) return "Wyoming";
            else return "";
        }

        public static string StateShort(State State)
        {
            if (State == State.Alabama) return "AL";
            if (State == State.Alaska) return "AK";
            if (State == State.Arizona) return "AZ";
            if (State == State.Arkansas) return "AR";
            if (State == State.California) return "CA";
            if (State == State.Colorado) return "CO";
            if (State == State.Connecticut) return "CT";
            if (State == State.Delaware) return "DE";
            if (State == State.Florida) return "FL";
            if (State == State.Georgia) return "GA";
            if (State == State.Hawaii) return "HI";
            if (State == State.Idaho) return "ID";
            if (State == State.Illinois) return "IL";
            if (State == State.Indiana) return "IN";
            if (State == State.Iowa) return "IA";
            if (State == State.Kansas) return "KS";
            if (State == State.Kentucky) return "KY";
            if (State == State.Louisiana) return "LA";
            if (State == State.Maine) return "ME";
            if (State == State.Maryland) return "MD";
            if (State == State.Massachusetts) return "MA";
            if (State == State.Michigan) return "MI";
            if (State == State.Minnesota) return "MN";
            if (State == State.Mississippi) return "MS";
            if (State == State.Missouri) return "MO";
            if (State == State.Montana) return "MT";
            if (State == State.Nebraska) return "NE";
            if (State == State.Nevada) return "NV";
            if (State == State.NewHampshire) return "NH";
            if (State == State.NewJersey) return "NJ";
            if (State == State.NewMexico) return "NM";
            if (State == State.NewYork) return "NY";
            if (State == State.NorthCarolina) return "NC";
            if (State == State.NorthDakota) return "ND";
            if (State == State.Ohio) return "OH";
            if (State == State.Oklahoma) return "OK";
            if (State == State.Oregon) return "OR";
            if (State == State.Pennsylvania) return "PA";
            if (State == State.RhodeIsland) return "RI";
            if (State == State.SouthCarolina) return "SC";
            if (State == State.SouthDakota) return "SD";
            if (State == State.Tennessee) return "TN";
            if (State == State.Texas) return "TX";
            if (State == State.Utah) return "UT";
            if (State == State.Vermont) return "VT";
            if (State == State.Virginia) return "VA";
            if (State == State.Washington) return "WA";
            if (State == State.WestVirginia) return "WV";
            if (State == State.Wisconsin) return "WI";
            if (State == State.Wyoming) return "WY";
            else return "";
        }

        public static State StateAbreviationDecode(string State)
        {
            if (State == "AL") return GeneralEnums.State.Alabama;
            if (State == "AK") return GeneralEnums.State.Alaska;
            if (State == "AZ") return GeneralEnums.State.Arizona;
            if (State == "AR") return GeneralEnums.State.Arkansas;
            if (State == "CA") return GeneralEnums.State.California;
            if (State == "CO") return GeneralEnums.State.Colorado;
            if (State == "CT") return GeneralEnums.State.Connecticut;
            if (State == "DE") return GeneralEnums.State.Delaware;
            if (State == "FL") return GeneralEnums.State.Florida;
            if (State == "GA") return GeneralEnums.State.Georgia;
            if (State == "HI") return GeneralEnums.State.Hawaii;
            if (State == "ID") return GeneralEnums.State.Idaho;
            if (State == "IL") return GeneralEnums.State.Illinois;
            if (State == "IN") return GeneralEnums.State.Indiana;
            if (State == "IA") return GeneralEnums.State.Iowa;
            if (State == "KS") return GeneralEnums.State.Kansas;
            if (State == "KY") return GeneralEnums.State.Kentucky;
            if (State == "LA") return GeneralEnums.State.Louisiana;
            if (State == "ME") return GeneralEnums.State.Maine;
            if (State == "MD") return GeneralEnums.State.Maryland;
            if (State == "MA") return GeneralEnums.State.Massachusetts;
            if (State == "MI") return GeneralEnums.State.Michigan;
            if (State == "MN") return GeneralEnums.State.Minnesota;
            if (State == "MS") return GeneralEnums.State.Mississippi;
            if (State == "MO") return GeneralEnums.State.Missouri;
            if (State == "MT") return GeneralEnums.State.Montana;
            if (State == "NE") return GeneralEnums.State.Nebraska;
            if (State == "NV") return GeneralEnums.State.Nevada;
            if (State == "NH") return GeneralEnums.State.NewHampshire;
            if (State == "NJ") return GeneralEnums.State.NewJersey;
            if (State == "NM") return GeneralEnums.State.NewMexico;
            if (State == "NY") return GeneralEnums.State.NewYork;
            if (State == "NC") return GeneralEnums.State.NorthCarolina;
            if (State == "ND") return GeneralEnums.State.NorthDakota;
            if (State == "OH") return GeneralEnums.State.Ohio;
            if (State == "OK") return GeneralEnums.State.Oklahoma;
            if (State == "OR") return GeneralEnums.State.Oregon;
            if (State == "PA") return GeneralEnums.State.Pennsylvania;
            if (State == "RI") return GeneralEnums.State.RhodeIsland;
            if (State == "SC") return GeneralEnums.State.SouthCarolina;
            if (State == "SD") return GeneralEnums.State.SouthDakota;
            if (State == "TN") return GeneralEnums.State.Tennessee;
            if (State == "TX") return GeneralEnums.State.Texas;
            if (State == "UT") return GeneralEnums.State.Utah;
            if (State == "VT") return GeneralEnums.State.Vermont;
            if (State == "VA") return GeneralEnums.State.Virginia;
            if (State == "WA") return GeneralEnums.State.Washington;
            if (State == "WV") return GeneralEnums.State.WestVirginia;
            if (State == "WI") return GeneralEnums.State.Wisconsin;
            if (State == "WY") return GeneralEnums.State.Wyoming;

            return GeneralEnums.State.None;
        }

        public enum ContractType
        {
            TermsOfService = 0
        }
      
        public enum WeekofMonth
        {
            First = 0,
            Second = 1,
            Third = 2,
            Fourth = 3
        }

        public enum MapMarkerColor
        {
            Grey = 0,
            Red = 1,
            Orange = 2,
            Yellow = 3,
            Green = 4,
            Blue = 5,
            Purple = 6,
            Pink = 7,
            White = 8,
            Teal = 9,
            RedWithDot = 10,
            OrangeWithDot = 11,
            YellowWithDot = 12,
            GreenWithDot = 13,
            BlueWithDot = 14,
            PurpleWithDot = 15,
            PinkWithDot = 16,
            WhiteWithDot = 17,
            TealWithDot = 18,
        }

        public enum RecurringType
        {
            Weekly = 0,
            Monthly = 1
        }

        public enum NoYes
        {
            No = 0,
            Yes = 1
        }

        public enum Time
        {
            AnyTime = 0,
            TwelveAM = 1,
            TwelveThirtyAM = 2,
            OneAM = 3,
            OneThirtyAM = 4,
            TwoAM = 5,
            TwoThirtyAM = 6,
            ThreeAM = 7,
            ThreeThirtyAM = 8,
            FourAM = 9,
            FourThirtyAM = 10,
            FiveAM = 11,
            FiveThirtyAM = 12,
            SixAM = 13,
            SixThirtyAM = 14,
            SevenAM = 15,
            SevenThirtyAM = 16,
            EightAM = 17,
            EightThirtyAM = 18,
            NineAM = 19,
            NineThirtyAM = 20,
            TenAM = 21,
            TenThirtyAM = 22,
            ElevenAM = 23,
            ElevenThirtyAM = 24,
            Twelve = 25,
            TwelveThirty = 26,
            One = 27,
            OneThirty = 28,
            Two = 29,
            TwoThirty = 30,
            Three = 31,
            ThreeThirty = 32,
            Four = 33,
            FourThirty = 34,
            Five = 35,
            FiveThirty = 36,
            Six = 37,
            SixThirty = 38,
            Seven = 39,
            SevenThirty = 40,
            Eight = 41,
            EightThirty = 42,
            Nine = 43,
            NineThirty = 44,
            Ten = 45,
            TenThirty = 46,
            Eleven = 47,
            ElevenThirty = 48
        }

        public static string TimeDisplay(Time time)
        {
            return time switch
            {
                Time.AnyTime => "Any Time",
                Time.TwelveAM => "12:00 AM",
                Time.TwelveThirtyAM => "12:30 AM",
                Time.OneAM => "1:00 AM",
                Time.OneThirtyAM => "1:30 AM",
                Time.TwoAM => "2:00 AM",
                Time.TwoThirtyAM => "2:30 AM",
                Time.ThreeAM => "3:00 AM",
                Time.ThreeThirtyAM => "3:30 AM",
                Time.FourAM => "4:00 AM",
                Time.FourThirtyAM => "4:30 AM",
                Time.FiveAM => "5:00 AM",
                Time.FiveThirtyAM => "5:30 AM",
                Time.SixAM => "6:00 AM",
                Time.SixThirtyAM => "6:30 AM",
                Time.SevenAM => "7:00 AM",
                Time.SevenThirtyAM => "7:30 AM",
                Time.EightAM => "8:00 AM",
                Time.EightThirtyAM => "8:30 AM",
                Time.NineAM => "9:00 AM",
                Time.NineThirtyAM => "9:30 AM",
                Time.TenAM => "10:00 AM",
                Time.TenThirtyAM => "10:30 AM",
                Time.ElevenAM => "11:00 AM",
                Time.ElevenThirtyAM => "11:30 AM",
                Time.Twelve => "12:00 PM",
                Time.TwelveThirty => "12:30 PM",
                Time.One => "1:00 PM",
                Time.OneThirty => "1:30 PM",
                Time.Two => "2:00 PM",
                Time.TwoThirty => "2:30 PM",
                Time.Three => "3:00 PM",
                Time.ThreeThirty => "3:30 PM",
                Time.Four => "4:00 PM",
                Time.FourThirty => "4:30 PM",
                Time.Five => "5:00 PM",
                Time.FiveThirty => "5:30 PM",
                Time.Six => "6:00 PM",
                Time.SixThirty => "6:30 PM",
                Time.Seven => "7:00 PM",
                Time.SevenThirty => "7:30 PM",
                Time.Eight => "8:00 PM",
                Time.EightThirty => "8:30 PM",
                Time.Nine => "9:00 PM",
                Time.NineThirty => "9:30 PM",
                Time.Ten => "10:00 PM",
                Time.TenThirty => "10:30 PM",
                Time.Eleven => "11:00 PM",
                Time.ElevenThirty => "11:30 PM",
                _ => ""
            };
        }
        public static DateTime GetDateTime(int hour, string minute, string ampm)
        {
            if (minute == null)
                minute = "00";
            if (ampm == null)
                ampm = "am";

            string timeString = $"{hour}:{minute} {ampm}";
            var dt = DateTime.ParseExact(timeString, "h:mm tt", null);

            return dt;
        }
        //public static DateTime GetDateTimeFromTime(Time time)
        //{
        //    switch (time)
        //    {
        //        case Time.SevenAM:
        //            return GetDateTime(7, "00", "AM");
        //        case Time.SevenThirty:
        //            return GetDateTime(7, "30", "AM");
        //        case Time.Eight:
        //            return GetDateTime(8, "00", "AM");
        //        case Time.EightThirty:
        //            return GetDateTime(8, "30", "AM");
        //        case Time.Nine:
        //            return GetDateTime(9, "00", "AM");
        //        case Time.NineThirty:
        //            return GetDateTime(9, "30", "AM");
        //        case Time.Ten:
        //            return GetDateTime(10, "00", "AM");
        //        case Time.TenThirty:
        //            return GetDateTime(10, "30", "AM");
        //        case Time.Eleven:
        //            return GetDateTime(11, "00", "AM");
        //        case Time.ElevenThirty:
        //            return GetDateTime(11, "30", "AM");
        //        case Time.Twelve:
        //            return GetDateTime(12, "00", "PM");
        //        case Time.TwelveThirty:
        //            return GetDateTime(12, "30", "PM");
        //        case Time.One:
        //            return GetDateTime(1, "00", "PM");
        //        case Time.OneThirty:
        //            return GetDateTime(1, "30", "PM");
        //        case Time.Two:
        //            return GetDateTime(2, "00", "PM");
        //        case Time.TwoThirty:
        //            return GetDateTime(2, "30", "PM");
        //        case Time.Three:
        //            return GetDateTime(3, "00", "PM");
        //        case Time.ThreeThirty:
        //            return GetDateTime(3, "30", "PM");
        //        case Time.Four:
        //            return GetDateTime(4, "00", "PM");
        //        case Time.FourThirty:
        //            return GetDateTime(4, "30", "PM");
        //        case Time.Five:
        //            return GetDateTime(5, "00", "PM");
        //        case Time.FiveThirty:
        //            return GetDateTime(5, "30", "PM");
        //        case Time.Six:
        //            return GetDateTime(6, "00", "PM");
        //        case Time.SixThirty:
        //            return GetDateTime(6, "30", "PM");
        //        case Time.Seven:
        //            return GetDateTime(7, "00", "PM");

        //        default:
        //            throw new ArgumentException("Invalid time");
        //    }
        //}
        public static DateTime GetDateTimeFromTime(Time time)
        {
            switch (time)
            {
                case Time.AnyTime:
                    return DateTime.MinValue; // Handle AnyTime as a placeholder

                case Time.TwelveAM:
                    return GetDateTime(12, "00", "AM");
                case Time.TwelveThirtyAM:
                    return GetDateTime(12, "30", "AM");
                case Time.OneAM:
                    return GetDateTime(1, "00", "AM");
                case Time.OneThirtyAM:
                    return GetDateTime(1, "30", "AM");
                case Time.TwoAM:
                    return GetDateTime(2, "00", "AM");
                case Time.TwoThirtyAM:
                    return GetDateTime(2, "30", "AM");
                case Time.ThreeAM:
                    return GetDateTime(3, "00", "AM");
                case Time.ThreeThirtyAM:
                    return GetDateTime(3, "30", "AM");
                case Time.FourAM:
                    return GetDateTime(4, "00", "AM");
                case Time.FourThirtyAM:
                    return GetDateTime(4, "30", "AM");
                case Time.FiveAM:
                    return GetDateTime(5, "00", "AM");
                case Time.FiveThirtyAM:
                    return GetDateTime(5, "30", "AM");
                case Time.SixAM:
                    return GetDateTime(6, "00", "AM");
                case Time.SixThirtyAM:
                    return GetDateTime(6, "30", "AM");
                case Time.SevenAM:
                    return GetDateTime(7, "00", "AM");
                case Time.SevenThirtyAM:
                    return GetDateTime(7, "30", "AM");
                case Time.EightAM:
                    return GetDateTime(8, "00", "AM");
                case Time.EightThirtyAM:
                    return GetDateTime(8, "30", "AM");
                case Time.NineAM:
                    return GetDateTime(9, "00", "AM");
                case Time.NineThirtyAM:
                    return GetDateTime(9, "30", "AM");
                case Time.TenAM:
                    return GetDateTime(10, "00", "AM");
                case Time.TenThirtyAM:
                    return GetDateTime(10, "30", "AM");
                case Time.ElevenAM:
                    return GetDateTime(11, "00", "AM");
                case Time.ElevenThirtyAM:
                    return GetDateTime(11, "30", "AM");

                case Time.Twelve:
                    return GetDateTime(12, "00", "PM");
                case Time.TwelveThirty:
                    return GetDateTime(12, "30", "PM");
                case Time.One:
                    return GetDateTime(1, "00", "PM");
                case Time.OneThirty:
                    return GetDateTime(1, "30", "PM");
                case Time.Two:
                    return GetDateTime(2, "00", "PM");
                case Time.TwoThirty:
                    return GetDateTime(2, "30", "PM");
                case Time.Three:
                    return GetDateTime(3, "00", "PM");
                case Time.ThreeThirty:
                    return GetDateTime(3, "30", "PM");
                case Time.Four:
                    return GetDateTime(4, "00", "PM");
                case Time.FourThirty:
                    return GetDateTime(4, "30", "PM");
                case Time.Five:
                    return GetDateTime(5, "00", "PM");
                case Time.FiveThirty:
                    return GetDateTime(5, "30", "PM");
                case Time.Six:
                    return GetDateTime(6, "00", "PM");
                case Time.SixThirty:
                    return GetDateTime(6, "30", "PM");
                case Time.Seven:
                    return GetDateTime(7, "00", "PM");
                case Time.SevenThirty:
                    return GetDateTime(7, "30", "PM");
                case Time.Eight:
                    return GetDateTime(8, "00", "PM");
                case Time.EightThirty:
                    return GetDateTime(8, "30", "PM");
                case Time.Nine:
                    return GetDateTime(9, "00", "PM");
                case Time.NineThirty:
                    return GetDateTime(9, "30", "PM");
                case Time.Ten:
                    return GetDateTime(10, "00", "PM");
                case Time.TenThirty:
                    return GetDateTime(10, "30", "PM");
                case Time.Eleven:
                    return GetDateTime(11, "00", "PM");
                case Time.ElevenThirty:
                    return GetDateTime(11, "30", "PM");

                default:
                    throw new ArgumentException("Invalid time");
            }
        }

        public static Time GetTime(string time)
        {
            return time switch
            {
                "00:00" => Time.TwelveAM,
                "00:30" => Time.TwelveThirtyAM,
                "01:00" => Time.OneAM,
                "01:30" => Time.OneThirtyAM,
                "02:00" => Time.TwoAM,
                "02:30" => Time.TwoThirtyAM,
                "03:00" => Time.ThreeAM,
                "03:30" => Time.ThreeThirtyAM,
                "04:00" => Time.FourAM,
                "04:30" => Time.FourThirtyAM,
                "05:00" => Time.FiveAM,
                "05:30" => Time.FiveThirtyAM,
                "06:00" => Time.SixAM,
                "06:30" => Time.SixThirtyAM,
                "07:00" => Time.SevenAM,
                "07:30" => Time.SevenThirtyAM,
                "08:00" => Time.EightAM,
                "08:30" => Time.EightThirtyAM,
                "09:00" => Time.NineAM,
                "09:30" => Time.NineThirtyAM,
                "10:00" => Time.TenAM,
                "10:30" => Time.TenThirtyAM,
                "11:00" => Time.ElevenAM,
                "11:30" => Time.ElevenThirtyAM,
                "12:00" => Time.Twelve,
                "12:30" => Time.TwelveThirty,
                "13:00" => Time.One,
                "13:30" => Time.OneThirty,
                "14:00" => Time.Two,
                "14:30" => Time.TwoThirty,
                "15:00" => Time.Three,
                "15:30" => Time.ThreeThirty,
                "16:00" => Time.Four,
                "16:30" => Time.FourThirty,
                "17:00" => Time.Five,
                "17:30" => Time.FiveThirty,
                "18:00" => Time.Six,
                "18:30" => Time.SixThirty,
                "19:00" => Time.Seven,
                "19:30" => Time.SevenThirty,
                "20:00" => Time.Eight,
                "20:30" => Time.EightThirty,
                "21:00" => Time.Nine,
                "21:30" => Time.NineThirty,
                "22:00" => Time.Ten,
                "22:30" => Time.TenThirty,
                "23:00" => Time.Eleven,
                "23:30" => Time.ElevenThirty,
                _ => Time.AnyTime
            };
        }

        public enum DayOfTheWeek
        {
            Sunday = 0,
            Monday = 1,
            Tuesday = 2,
            Wednesday = 3,
            Thursday = 4,
            Friday = 5,
            Saturday = 6
        }

        public enum Month
        {
            January = 0,
            February = 1,
            March = 2,
            April = 3,
            May = 4,
            June = 5,
            July = 6,
            August = 7,
            September = 8,
            October = 9,
            November = 10,
            December = 11
        }
    }

    public static class Calendar
    {
        public static List<DayOfTheWeek> DaysOfWeek()
        {
            var Days = new List<DayOfTheWeek>();
            Days.Add(DayOfTheWeek.Sunday);
            Days.Add(DayOfTheWeek.Monday);
            Days.Add(DayOfTheWeek.Tuesday);
            Days.Add(DayOfTheWeek.Wednesday);
            Days.Add(DayOfTheWeek.Thursday);
            Days.Add(DayOfTheWeek.Friday);
            Days.Add(DayOfTheWeek.Saturday);
            return Days;
        }

        public static List<Time> Times()
        {
            var Times = new List<Time>();
            Times.Add(Time.SevenAM);
            Times.Add(Time.SevenThirty);
            Times.Add(Time.Eight);
            Times.Add(Time.EightThirty);
            Times.Add(Time.Nine);
            Times.Add(Time.NineThirty);
            Times.Add(Time.Ten);
            Times.Add(Time.TenThirty);
            Times.Add(Time.Eleven);
            Times.Add(Time.ElevenThirty);
            Times.Add(Time.Twelve);
            Times.Add(Time.TwelveThirty);
            Times.Add(Time.One);
            Times.Add(Time.OneThirty);
            Times.Add(Time.Two);
            Times.Add(Time.TwoThirty);
            Times.Add(Time.Three);
            Times.Add(Time.ThreeThirty);
            Times.Add(Time.Four);
            Times.Add(Time.FourThirty);
            Times.Add(Time.Five);
            Times.Add(Time.FiveThirty);
            Times.Add(Time.Six);
            Times.Add(Time.SixThirty);
            Times.Add(Time.Seven);

            return Times;
        }

        public static Time AddTwoHours(Time Time)
        {
            if (Time == Time.SevenAM) return Time.Nine;
            else if (Time == Time.Eight) return Time.Ten;
            else if (Time == Time.Nine) return Time.Eleven;
            else if (Time == Time.Ten) return Time.Twelve;
            else if (Time == Time.Eleven) return Time.One;
            else if (Time == Time.Twelve) return Time.Two;
            else if (Time == Time.One) return Time.Three;
            else if (Time == Time.Two) return Time.Four;
            else if (Time == Time.Three) return Time.Five;
            else if (Time == Time.Four) return Time.Six;
            else return Time.Seven;
        }

        public static string DayOfTheWeekDisplay(DayOfTheWeek DayOfTheWeek)
        {
            if (DayOfTheWeek == DayOfTheWeek.Sunday) return "Sunday";
            if (DayOfTheWeek == DayOfTheWeek.Monday) return "Monday";
            if (DayOfTheWeek == DayOfTheWeek.Tuesday) return "Tuesday";
            if (DayOfTheWeek == DayOfTheWeek.Wednesday) return "Wednesday";
            if (DayOfTheWeek == DayOfTheWeek.Thursday) return "Thursday";
            if (DayOfTheWeek == DayOfTheWeek.Friday) return "Friday";
            if (DayOfTheWeek == DayOfTheWeek.Saturday) return "Saturday";
            return "";
        }

        public static Time SubtractOneHour(Time Time)
        {
            if (Time == Time.SevenAM) return Time.SevenAM;
            else if (Time == Time.SevenThirty) return Time.SevenAM;
            else if (Time == Time.Eight) return Time.SevenAM;
            else if (Time == Time.EightThirty) return Time.SevenThirty;
            else if (Time == Time.Nine) return Time.Eight;
            else if (Time == Time.NineThirty) return Time.EightThirty;
            else if (Time == Time.Ten) return Time.Nine;
            else if (Time == Time.TenThirty) return Time.NineThirty;
            else if (Time == Time.Eleven) return Time.Ten;
            else if (Time == Time.ElevenThirty) return Time.Ten;
            else if (Time == Time.Twelve) return Time.Eleven;
            else if (Time == Time.TwelveThirty) return Time.Eleven;
            else if (Time == Time.One) return Time.Twelve;
            else if (Time == Time.OneThirty) return Time.Twelve;
            else if (Time == Time.Two) return Time.One;
            else if (Time == Time.TwoThirty) return Time.One;
            else if (Time == Time.Three) return Time.Two;
            else if (Time == Time.ThreeThirty) return Time.Two;
            else if (Time == Time.Four) return Time.Three;
            else if (Time == Time.FourThirty) return Time.Three;
            else if (Time == Time.Five) return Time.Four;
            else if (Time == Time.FiveThirty) return Time.Four;
            else if (Time == Time.Six) return Time.Five;
            else if (Time == Time.SixThirty) return Time.Five;
            else return Time.Six;
        }

        public static Time SubtractThirtyMinutes(Time time)
        {
            return time switch
            {
                Time.TwelveThirtyAM => Time.TwelveAM,
                Time.OneAM => Time.TwelveThirtyAM,
                Time.OneThirtyAM => Time.OneAM,
                Time.TwoAM => Time.OneThirtyAM,
                Time.TwoThirtyAM => Time.TwoAM,
                Time.ThreeAM => Time.TwoThirtyAM,
                Time.ThreeThirtyAM => Time.ThreeAM,
                Time.FourAM => Time.ThreeThirtyAM,
                Time.FourThirtyAM => Time.FourAM,
                Time.FiveAM => Time.FourThirtyAM,
                Time.FiveThirtyAM => Time.FiveAM,
                Time.SixAM => Time.FiveThirtyAM,
                Time.SixThirtyAM => Time.SixAM,
                Time.SevenAM => Time.SixThirtyAM,
                Time.SevenThirtyAM => Time.SevenAM,
                Time.EightAM => Time.SevenThirtyAM,
                Time.EightThirtyAM => Time.EightAM,
                Time.NineAM => Time.EightThirtyAM,
                Time.NineThirtyAM => Time.NineAM,
                Time.TenAM => Time.NineThirtyAM,
                Time.TenThirtyAM => Time.TenAM,
                Time.ElevenAM => Time.TenThirtyAM,
                Time.ElevenThirtyAM => Time.ElevenAM,
                Time.Twelve => Time.ElevenThirtyAM,
                Time.TwelveThirty => Time.Twelve,
                Time.One => Time.TwelveThirty,
                Time.OneThirty => Time.One,
                Time.Two => Time.OneThirty,
                Time.TwoThirty => Time.Two,
                Time.Three => Time.TwoThirty,
                Time.ThreeThirty => Time.Three,
                Time.Four => Time.ThreeThirty,
                Time.FourThirty => Time.Four,
                Time.Five => Time.FourThirty,
                Time.FiveThirty => Time.Five,
                Time.Six => Time.FiveThirty,
                Time.SixThirty => Time.Six,
                Time.Seven => Time.SixThirty,
                Time.SevenThirty => Time.Seven,
                Time.Eight => Time.SevenThirty,
                Time.EightThirty => Time.Eight,
                Time.Nine => Time.EightThirty,
                Time.NineThirty => Time.Nine,
                Time.Ten => Time.NineThirty,
                Time.TenThirty => Time.Ten,
                Time.Eleven => Time.TenThirty,
                Time.ElevenThirty => Time.Eleven,
                _ => Time.AnyTime // Default case if something goes wrong.
            };

        }
        public static Time AddOneHour(Time Time)
        {
            if (Time == Time.SevenAM) return Time.Eight;
            else if (Time == Time.SevenThirty) return Time.EightThirty;
            else if (Time == Time.Eight) return Time.Nine;
            else if (Time == Time.EightThirty) return Time.NineThirty;
            else if (Time == Time.Nine) return Time.Ten;
            else if (Time == Time.NineThirty) return Time.TenThirty;
            else if (Time == Time.Ten) return Time.Eleven;
            else if (Time == Time.TenThirty) return Time.ElevenThirty;
            else if (Time == Time.Eleven) return Time.Twelve;
            else if (Time == Time.ElevenThirty) return Time.TwelveThirty;
            else if (Time == Time.Twelve) return Time.One;
            else if (Time == Time.TwelveThirty) return Time.OneThirty;
            else if (Time == Time.One) return Time.Two;
            else if (Time == Time.OneThirty) return Time.TwoThirty;
            else if (Time == Time.Two) return Time.Three;
            else if (Time == Time.TwoThirty) return Time.ThreeThirty;
            else if (Time == Time.Three) return Time.Four;
            else if (Time == Time.ThreeThirty) return Time.FourThirty;
            else if (Time == Time.Four) return Time.Five;
            else if (Time == Time.FourThirty) return Time.FiveThirty;
            else if (Time == Time.Five) return Time.Six;
            else if (Time == Time.FiveThirty) return Time.SixThirty;
            else return Time.Seven;
        }

        public static Time AddThirtyMinutes(Time time)
        {
            return time switch
            {
                Time.TwelveAM => Time.TwelveThirtyAM,
                Time.TwelveThirtyAM => Time.OneAM,
                Time.OneAM => Time.OneThirtyAM,
                Time.OneThirtyAM => Time.TwoAM,
                Time.TwoAM => Time.TwoThirtyAM,
                Time.TwoThirtyAM => Time.ThreeAM,
                Time.ThreeAM => Time.ThreeThirtyAM,
                Time.ThreeThirtyAM => Time.FourAM,
                Time.FourAM => Time.FourThirtyAM,
                Time.FourThirtyAM => Time.FiveAM,
                Time.FiveAM => Time.FiveThirtyAM,
                Time.FiveThirtyAM => Time.SixAM,
                Time.SixAM => Time.SixThirtyAM,
                Time.SixThirtyAM => Time.SevenAM,
                Time.SevenAM => Time.SevenThirtyAM,
                Time.SevenThirtyAM => Time.EightAM,
                Time.EightAM => Time.EightThirtyAM,
                Time.EightThirtyAM => Time.NineAM,
                Time.NineAM => Time.NineThirtyAM,
                Time.NineThirtyAM => Time.TenAM,
                Time.TenAM => Time.TenThirtyAM,
                Time.TenThirtyAM => Time.ElevenAM,
                Time.ElevenAM => Time.ElevenThirtyAM,
                Time.ElevenThirtyAM => Time.Twelve,
                Time.Twelve => Time.TwelveThirty,
                Time.TwelveThirty => Time.One,
                Time.One => Time.OneThirty,
                Time.OneThirty => Time.Two,
                Time.Two => Time.TwoThirty,
                Time.TwoThirty => Time.Three,
                Time.Three => Time.ThreeThirty,
                Time.ThreeThirty => Time.Four,
                Time.Four => Time.FourThirty,
                Time.FourThirty => Time.Five,
                Time.Five => Time.FiveThirty,
                Time.FiveThirty => Time.Six,
                Time.Six => Time.SixThirty,
                Time.SixThirty => Time.Seven,
                Time.Seven => Time.SevenThirty,
                Time.SevenThirty => Time.Eight,
                Time.Eight => Time.EightThirty,
                Time.EightThirty => Time.Nine,
                Time.Nine => Time.NineThirty,
                Time.NineThirty => Time.Ten,
                Time.Ten => Time.TenThirty,
                Time.TenThirty => Time.Eleven,
                Time.Eleven => Time.ElevenThirty,
                Time.ElevenThirty => Time.Twelve,
                _ => Time.AnyTime
            };
        }

        public static Tuple<int, int, string> GetHourAndMinWithAMPM(Time Time)
        {
            var Hour = 0;
            var Min = "";
            var AMPM = "";

            if (Time == Time.SevenAM)
            {
                Hour = 7;
                Min = "00";
                AMPM = "am";
            }
            else if (Time == Time.SevenThirty)
            {
                Hour = 7;
                Min = "30";
                AMPM = "am";
            }
            else if (Time == Time.Eight)
            {
                Hour = 8;
                Min = "00";
                AMPM = "am";
            }
            else if (Time == Time.EightThirty)
            {
                Hour = 8;
                Min = "30";
                AMPM = "am";
            }
            else if (Time == Time.Nine)
            {
                Hour = 9;
                Min = "00";
                AMPM = "am";
            }
            else if (Time == Time.NineThirty)
            {
                Hour = 9;
                Min = "30";
                AMPM = "am";
            }
            else if (Time == Time.Ten)
            {
                Hour = 10;
                Min = "00";
                AMPM = "am";
            }
            else if (Time == Time.TenThirty)
            {
                Hour = 10;
                Min = "30";
                AMPM = "am";
            }
            else if (Time == Time.Eleven)
            {
                Hour = 11;
                Min = "00";
                AMPM = "am";
            }
            else if (Time == Time.ElevenThirty)
            {
                Hour = 11;
                Min = "30";
                AMPM = "am";
            }
            else if (Time == Time.Twelve)
            {
                Hour = 12;
                Min = "00";
                AMPM = "pm";
            }
            else if (Time == Time.TwelveThirty)
            {
                Hour = 12;
                Min = "30";
                AMPM = "pm";
            }
            else if (Time == Time.One)
            {
                Hour = 1;
                Min = "00";
                AMPM = "pm";
            }
            else if (Time == Time.OneThirty)
            {
                Hour = 1;
                Min = "30";
                AMPM = "pm";
            }
            else if (Time == Time.Two)
            {
                Hour = 2;
                Min = "00";
                AMPM = "pm";
            }
            else if (Time == Time.TwoThirty)
            {
                Hour = 2;
                Min = "30";
                AMPM = "pm";
            }
            else if (Time == Time.Three)
            {
                Hour = 3;
                Min = "00";
                AMPM = "pm";
            }
            else if (Time == Time.ThreeThirty)
            {
                Hour = 3;
                Min = "30";
                AMPM = "pm";
            }
            else if (Time == Time.Four)
            {
                Hour = 4;
                Min = "00";
                AMPM = "pm";
            }
            else if (Time == Time.FourThirty)
            {
                Hour = 4;
                Min = "30";
                AMPM = "pm";
            }
            else if (Time == Time.Five)
            {
                Hour = 5;
                Min = "00";
                AMPM = "pm";
            }
            else if (Time == Time.FiveThirty)
            {
                Hour = 5;
                Min = "30";
                AMPM = "pm";
            }
            else if (Time == Time.Six)
            {
                Hour = 6;
                Min = "00";
                AMPM = "pm";
            }
            else if (Time == Time.SixThirty)
            {
                Hour = 6;
                Min = "30";
                AMPM = "pm";
            }
            else if (Time == Time.Seven)
            {
                Hour = 7;
                Min = "00";
                AMPM = "pm";
            }

            return Tuple.Create(Hour, Int32.Parse(Min), AMPM);
        }

        public static string TimeToString(Time time)
        {
            return time switch
            {
                Time.TwelveAM => "12:00 AM",
                Time.TwelveThirtyAM => "12:30 AM",
                Time.OneAM => "01:00 AM",
                Time.OneThirtyAM => "01:30 AM",
                Time.TwoAM => "02:00 AM",
                Time.TwoThirtyAM => "02:30 AM",
                Time.ThreeAM => "03:00 AM",
                Time.ThreeThirtyAM => "03:30 AM",
                Time.FourAM => "04:00 AM",
                Time.FourThirtyAM => "04:30 AM",
                Time.FiveAM => "05:00 AM",
                Time.FiveThirtyAM => "05:30 AM",
                Time.SixAM => "06:00 AM",
                Time.SixThirtyAM => "06:30 AM",
                Time.SevenAM => "07:00 AM",
                Time.SevenThirtyAM => "07:30 AM",
                Time.EightAM => "08:00 AM",
                Time.EightThirtyAM => "08:30 AM",
                Time.NineAM => "09:00 AM",
                Time.NineThirtyAM => "09:30 AM",
                Time.TenAM => "10:00 AM",
                Time.TenThirtyAM => "10:30 AM",
                Time.ElevenAM => "11:00 AM",
                Time.ElevenThirtyAM => "11:30 AM",
                Time.Twelve => "12:00 PM",
                Time.TwelveThirty => "12:30 PM",
                Time.One => "01:00 PM",
                Time.OneThirty => "01:30 PM",
                Time.Two => "02:00 PM",
                Time.TwoThirty => "02:30 PM",
                Time.Three => "03:00 PM",
                Time.ThreeThirty => "03:30 PM",
                Time.Four => "04:00 PM",
                Time.FourThirty => "04:30 PM",
                Time.Five => "05:00 PM",
                Time.FiveThirty => "05:30 PM",
                Time.Six => "06:00 PM",
                Time.SixThirty => "06:30 PM",
                Time.Seven => "07:00 PM",
                Time.SevenThirty => "07:30 PM",
                Time.Eight => "08:00 PM",
                Time.EightThirty => "08:30 PM",
                Time.Nine => "09:00 PM",
                Time.NineThirty => "09:30 PM",
                Time.Ten => "10:00 PM",
                Time.TenThirty => "10:30 PM",
                Time.Eleven => "11:00 PM",
                Time.ElevenThirty => "11:30 PM",
                _ => ""
            };
        }

    }
}

