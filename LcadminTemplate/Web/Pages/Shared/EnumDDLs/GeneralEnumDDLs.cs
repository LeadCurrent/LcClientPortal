using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using static Data.CompanyEnums;

namespace Web
{
    public static class GeneralEnumDDLs
    {
        public static List<SelectListItem> SampleDDL()
        {
            var ddl = new List<SelectListItem>()
                  {
                       new SelectListItem() { Text = "", Value = "0"},
                       new SelectListItem() { Text = "Item1", Value = "1"},
                       new SelectListItem() { Text = "Item2", Value = "2"},
                       new SelectListItem() { Text = "Item3", Value = "3"}
                 };
            return ddl;
        }

        public static string GetIanaTimeZone(CompanyTimeZone zone)
        {
            switch (zone)
            {
                case CompanyTimeZone.Central:
                    return "America/Chicago";
                case CompanyTimeZone.Eastern:
                    return "America/New_York";
                case CompanyTimeZone.Mountain:
                    return "America/Denver";
                case CompanyTimeZone.Pacific:
                    return "America/Los_Angeles";
                default:
                    return string.Empty; // Return an empty string if zone is not recognized
            }
        }

        public static List<SelectListItem> TimesDDL()
        {
            var ddl = new List<SelectListItem>()
                  {
                       new SelectListItem() { Text = "Any Time", Value = "0"},
                       new SelectListItem() { Text = "7:00 AM", Value = "1"},
                       new SelectListItem() { Text = "7:30 AM", Value = "2"},
                       new SelectListItem() { Text = "8:00 AM", Value = "3"},
                       new SelectListItem() { Text = "8:30 AM", Value = "4"},
                       new SelectListItem() { Text = "9:00 AM", Value = "5"},
                       new SelectListItem() { Text = "9:30 AM", Value = "6"},
                       new SelectListItem() { Text = "10:00 AM", Value = "7"},
                       new SelectListItem() { Text = "10:30 AM", Value = "8"},
                       new SelectListItem() { Text = "11:00 AM", Value = "9"},
                       new SelectListItem() { Text = "11:30 AM", Value = "10"},
                       new SelectListItem() { Text = "12:00 PM", Value = "11"},
                       new SelectListItem() { Text = "12:30 PM", Value = "12"},
                       new SelectListItem() { Text = "1:00 PM", Value = "13"},
                       new SelectListItem() { Text = "1:30 PM", Value = "14"},
                       new SelectListItem() { Text = "2:00 PM", Value = "15"},
                       new SelectListItem() { Text = "2:30 PM", Value = "16"},
                       new SelectListItem() { Text = "3:00 PM", Value = "17"},
                       new SelectListItem() { Text = "3:30 PM", Value = "18"},
                       new SelectListItem() { Text = "4:00 PM", Value = "19"},
                       new SelectListItem() { Text = "4:30 PM", Value = "20"},
                       new SelectListItem() { Text = "5:00 PM", Value = "21"},
                       new SelectListItem() { Text = "5:30 PM", Value = "22"},
                       new SelectListItem() { Text = "6:00 PM", Value = "23"},
                       new SelectListItem() { Text = "6:30 PM", Value = "24"}
                 };
            return ddl;
        }

        public static List<SelectListItem> SearchFilter()
        {
            var ddl = new List<SelectListItem>()
                {
                    new SelectListItem() { Text = "", Value = ""},
                    new SelectListItem() { Text = "Division", Value = "1"},
                    new SelectListItem() { Text = "Service", Value = "2"},
                    new SelectListItem() { Text = "Crew", Value = "3"},
                    new SelectListItem() { Text = "Status", Value = "4"},
                    new SelectListItem() { Text = "Assigned Staff", Value = "5"},
                    new SelectListItem() { Text = "Assigned Group", Value = "6"},
                    new SelectListItem() { Text = "Date Range", Value = "7"},
                    new SelectListItem() { Text = "Job Number", Value = "8"},
                    new SelectListItem() { Text = "Quote Number", Value = "9"},
                    new SelectListItem() { Text = "Invoice Number", Value = "10"}
                 };
            return ddl;
        }

        public static List<SelectListItem> HourDropdown()
        {
            var ddl = new List<SelectListItem>()
                  {
                       new SelectListItem() { Text = "", Value = ""},
                       new SelectListItem() { Text = "1", Value = "1"},
                       new SelectListItem() { Text = "2", Value = "2"},
                       new SelectListItem() { Text = "3", Value = "3"},
                       new SelectListItem() { Text = "4", Value = "4"},
                       new SelectListItem() { Text = "5", Value = "5"},
                       new SelectListItem() { Text = "6", Value = "6"},
                       new SelectListItem() { Text = "7", Value = "7"},
                       new SelectListItem() { Text = "8", Value = "8"},
                       new SelectListItem() { Text = "9", Value = "9"},
                       new SelectListItem() { Text = "10", Value = "10"},
                       new SelectListItem() { Text = "11", Value = "11"},
                       new SelectListItem() { Text = "12", Value = "12"}
                 };
            return ddl;
        }

        public static List<SelectListItem> TimeZoneDDL()
        {
            var ddl = new List<SelectListItem>()
                {
                    new SelectListItem() { Text = "Pacific", Value = "-2"},
                    new SelectListItem() { Text = "Mountain", Value = "-1"},
                    new SelectListItem() { Text = "Central", Value = "0"},
                    new SelectListItem() { Text = "Eastern", Value = "1"}
                 };
            return ddl;
        }
        public static List<SelectListItem> PMAMDropdown()
        {
            var ddl = new List<SelectListItem>()
                  {
                       new SelectListItem() { Text = "pm", Value = "pm"},
                       new SelectListItem() { Text = "am", Value = "am"}

                 };
            return ddl;
        }
        public static List<SelectListItem> HourZeroToTwelveDropdown()
        {
            var ddl = new List<SelectListItem>()
                  {
                       new SelectListItem() { Text = "0", Value = "0"},
                       new SelectListItem() { Text = "1", Value = "1"},
                       new SelectListItem() { Text = "2", Value = "2"},
                       new SelectListItem() { Text = "3", Value = "3"},
                       new SelectListItem() { Text = "4", Value = "4"},
                       new SelectListItem() { Text = "5", Value = "5"},
                       new SelectListItem() { Text = "6", Value = "6"},
                       new SelectListItem() { Text = "7", Value = "7"},
                       new SelectListItem() { Text = "8", Value = "8"},
                       new SelectListItem() { Text = "9", Value = "9"},
                       new SelectListItem() { Text = "10", Value = "10"},
                       new SelectListItem() { Text = "11", Value = "11"},
                       new SelectListItem() { Text = "12", Value = "12"}
                 };
            return ddl;
        }
        public static List<SelectListItem> MinuteDropdown()
        {
            var ddl = new List<SelectListItem>()
                  {
                       new SelectListItem() { Text = "", Value = ""},
                       new SelectListItem() { Text = "00", Value = "00"},
                       new SelectListItem() { Text = "05", Value = "05"},
                       new SelectListItem() { Text = "10", Value = "10"},
                       new SelectListItem() { Text = "15", Value = "15"},
                       new SelectListItem() { Text = "20", Value = "20"},
                       new SelectListItem() { Text = "25", Value = "25"},
                       new SelectListItem() { Text = "30", Value = "30"},
                       new SelectListItem() { Text = "35", Value = "35"},
                       new SelectListItem() { Text = "40", Value = "40"},
                       new SelectListItem() { Text = "45", Value = "45"},
                       new SelectListItem() { Text = "50", Value = "50"},
                       new SelectListItem() { Text = "55", Value = "55"}
                 };
            return ddl;
        }

        public static List<SelectListItem> AMPMDropdown()
        {
            var ddl = new List<SelectListItem>()
                  {
                       new SelectListItem() { Text = "", Value = ""},
                       new SelectListItem() { Text = "am", Value = "am"},
                       new SelectListItem() { Text = "pm", Value = "pm"}
                 };
            return ddl;
        }
        public static List<SelectListItem> HourDDL()
        {
            var ddl = new List<SelectListItem>()
                  {
                      new SelectListItem() { Text = "1", Value = "1"},
                      new SelectListItem() { Text = "2", Value = "2"},
                      new SelectListItem() { Text = "3", Value = "3"},
                      new SelectListItem() { Text = "4", Value = "4"},
                      new SelectListItem() { Text = "5", Value = "5"},
                      new SelectListItem() { Text = "6", Value = "6"},
                      new SelectListItem() { Text = "7", Value = "7"},
                      new SelectListItem() { Text = "8", Value = "8"},
                      new SelectListItem() { Text = "9", Value = "9"},
                      new SelectListItem() { Text = "10", Value = "10"},
                      new SelectListItem() { Text = "11", Value = "11"},
                      new SelectListItem() { Text = "12", Value = "12"}
                 };
            return ddl;
        }

        public static List<SelectListItem> MinuteDDL()
        {
            var ddl = new List<SelectListItem>()
                  {
                      new SelectListItem() { Text = "00", Value = "0"},
                      new SelectListItem() { Text = "01", Value = "1"},
                        new SelectListItem() { Text = "02", Value = "2"},
                        new SelectListItem() { Text = "03", Value = "3"},
                        new SelectListItem() { Text = "04", Value = "4"},
                        new SelectListItem() { Text = "05", Value = "5"},
                        new SelectListItem() { Text = "06", Value = "6"},
                        new SelectListItem() { Text = "07", Value = "7"},
                        new SelectListItem() { Text = "08", Value = "8"},
                        new SelectListItem() { Text = "09", Value = "9"},
                        new SelectListItem() { Text = "10", Value = "10"},
                        new SelectListItem() { Text = "11", Value = "11"},
                        new SelectListItem() { Text = "12", Value = "12"},
                        new SelectListItem() { Text = "13", Value = "13"},
                        new SelectListItem() { Text = "14", Value = "14"},
                        new SelectListItem() { Text = "15", Value = "15"},
                        new SelectListItem() { Text = "16", Value = "16"},
                        new SelectListItem() { Text = "17", Value = "17"},
                        new SelectListItem() { Text = "18", Value = "18"},
                        new SelectListItem() { Text = "19", Value = "19"},
                        new SelectListItem() { Text = "20", Value = "20"},
                        new SelectListItem() { Text = "21", Value = "21"},
                        new SelectListItem() { Text = "22", Value = "22"},
                        new SelectListItem() { Text = "23", Value = "23"},
                        new SelectListItem() { Text = "24", Value = "24"},
                        new SelectListItem() { Text = "25", Value = "25"},
                        new SelectListItem() { Text = "26", Value = "26"},
                        new SelectListItem() { Text = "27", Value = "27"},
                        new SelectListItem() { Text = "28", Value = "28"},
                        new SelectListItem() { Text = "29", Value = "29"},
                        new SelectListItem() { Text = "30", Value = "30"},
                        new SelectListItem() { Text = "31", Value = "31"},
                        new SelectListItem() { Text = "32", Value = "32"},
                        new SelectListItem() { Text = "33", Value = "33"},
                        new SelectListItem() { Text = "34", Value = "34"},
                        new SelectListItem() { Text = "35", Value = "35"},
                        new SelectListItem() { Text = "36", Value = "36"},
                        new SelectListItem() { Text = "37", Value = "37"},
                        new SelectListItem() { Text = "38", Value = "38"},
                        new SelectListItem() { Text = "39", Value = "39"},
                        new SelectListItem() { Text = "40", Value = "40"},
                        new SelectListItem() { Text = "41", Value = "41"},
                        new SelectListItem() { Text = "42", Value = "42"},
                        new SelectListItem() { Text = "43", Value = "43"},
                        new SelectListItem() { Text = "44", Value = "44"},
                        new SelectListItem() { Text = "45", Value = "45"},
                        new SelectListItem() { Text = "46", Value = "46"},
                        new SelectListItem() { Text = "47", Value = "47"},
                        new SelectListItem() { Text = "48", Value = "48"},
                        new SelectListItem() { Text = "49", Value = "49"},
                        new SelectListItem() { Text = "50", Value = "50"},
                        new SelectListItem() { Text = "51", Value = "51"},
                        new SelectListItem() { Text = "52", Value = "52"},
                        new SelectListItem() { Text = "53", Value = "53"},
                        new SelectListItem() { Text = "54", Value = "54"},
                        new SelectListItem() { Text = "55", Value = "55"},
                        new SelectListItem() { Text = "56", Value = "56"},
                        new SelectListItem() { Text = "57", Value = "57"},
                        new SelectListItem() { Text = "58", Value = "58"},
                        new SelectListItem() { Text = "59", Value = "59"},
                        new SelectListItem() { Text = "60", Value = "60"}
                 };
            return ddl;
        }

        public static List<SelectListItem> AMPMDDL()
        {
            var ddl = new List<SelectListItem>()
                  {
                      new SelectListItem() { Text = "AM", Value = "AM"},
                      new SelectListItem() { Text = "PM", Value = "PM"}
                 };
            return ddl;
        }
        public static List<SelectListItem> ActiveInactiveDDL()
        {
            var ddl = new List<SelectListItem>()
                  {
                       new () { Text = "Active", Value = "True"},
                       new SelectListItem() { Text = "Inactive", Value = "False"},
                 };
            return ddl;
        }

        public static List<SelectListItem> NoYesDDL()
        {
            var ddl = new List<SelectListItem>()
                {
                    new SelectListItem() { Text = "No", Value = "False"},
                    new SelectListItem() { Text = "Yes", Value = "True"}
                 };
            return ddl;
        }
        public static List<SelectListItem> AgreementNoYesDDL()
        {
            var ddl = new List<SelectListItem>()
                {
                    new SelectListItem() { Text = "No (Each instance of the agreement can be modified)", Value = "False"},
                    new SelectListItem() { Text = "Yes (Instances of the ageement are identical for each customer)", Value = "True"}
                 };
            return ddl;
        }

        public static List<SelectListItem> StateDDL()
        {
            var ddl = new List<SelectListItem>()
                {
                    new SelectListItem() { Text = "", Value = "0"},
                    new SelectListItem() { Text = "Alabama", Value = "1"},
                    new SelectListItem() { Text = "Alaska", Value = "2"},
                    new SelectListItem() { Text = "Arizona", Value = "3"},
                    new SelectListItem() { Text = "Arkansas", Value = "4"},
                    new SelectListItem() { Text = "California", Value = "5"},
                    new SelectListItem() { Text = "Colorado", Value = "6"},
                    new SelectListItem() { Text = "Connecticut", Value = "7"},
                    new SelectListItem() { Text = "Delaware", Value = "8"},
                    new SelectListItem() { Text = "Florida", Value = "9"},
                    new SelectListItem() { Text = "Georgia", Value = "10"},
                    new SelectListItem() { Text = "Hawaii", Value = "11"},
                    new SelectListItem() { Text = "Idaho", Value = "12"},
                    new SelectListItem() { Text = "Illinois", Value = "13"},
                    new SelectListItem() { Text = "Indiana", Value = "14"},
                    new SelectListItem() { Text = "Iowa", Value = "15"},
                    new SelectListItem() { Text = "Kansas", Value = "16"},
                    new SelectListItem() { Text = "Kentucky", Value = "17"},
                    new SelectListItem() { Text = "Louisiana", Value = "18"},
                    new SelectListItem() { Text = "Maine", Value = "19"},
                    new SelectListItem() { Text = "Maryland", Value = "20"},
                    new SelectListItem() { Text = "Massachusetts", Value = "21"},
                    new SelectListItem() { Text = "Michigan", Value = "22"},
                    new SelectListItem() { Text = "Minnesota", Value = "23"},
                    new SelectListItem() { Text = "Mississippi", Value = "24"},
                    new SelectListItem() { Text = "Missouri", Value = "25"},
                    new SelectListItem() { Text = "Montana", Value = "26"},
                    new SelectListItem() { Text = "Nebraska", Value = "27"},
                    new SelectListItem() { Text = "Nevada", Value = "28"},
                    new SelectListItem() { Text = "New Hampshire", Value = "29"},
                    new SelectListItem() { Text = "New Jersey", Value = "30"},
                    new SelectListItem() { Text = "New Mexico", Value = "31"},
                    new SelectListItem() { Text = "New York", Value = "32"},
                    new SelectListItem() { Text = "North Carolina", Value = "33"},
                    new SelectListItem() { Text = "North Dakota", Value = "34"},
                    new SelectListItem() { Text = "Ohio", Value = "35"},
                    new SelectListItem() { Text = "Oklahoma", Value = "36"},
                    new SelectListItem() { Text = "Oregon", Value = "37"},
                    new SelectListItem() { Text = "Pennsylvania", Value = "38"},
                    new SelectListItem() { Text = "Rhode Island", Value = "39"},
                    new SelectListItem() { Text = "South Carolina", Value = "40"},
                    new SelectListItem() { Text = "South Dakota", Value = "41"},
                    new SelectListItem() { Text = "Tennessee", Value = "42"},
                    new SelectListItem() { Text = "Texas", Value = "43"},
                    new SelectListItem() { Text = "Utah", Value = "44"},
                    new SelectListItem() { Text = "Vermont", Value = "45"},
                    new SelectListItem() { Text = "Virginia", Value = "46"},
                    new SelectListItem() { Text = "Washington", Value = "47"},
                    new SelectListItem() { Text = "West Virginia", Value = "48"},
                    new SelectListItem() { Text = "Wisconsin", Value = "49"},
                    new SelectListItem() { Text = "Wyoming", Value = "50"}
                 };
            return ddl;
        }
        public static List<SelectListItem> StatusDDL()
        {
            var ddl = new List<SelectListItem>()
                {
                    new SelectListItem() { Text = "Active", Value = "0"},
                    new SelectListItem() { Text = "Inactive", Value = "1"}
                 };
            return ddl;
        }
        public static List<SelectListItem> AgreementStatusDDL()
        {
            var ddl = new List<SelectListItem>()
                {
                    new SelectListItem() { Text = "Draft", Value = "0"},
                    new SelectListItem() { Text = "Active", Value = "1"},
                    new SelectListItem() { Text = "Inactive", Value = "2"}
                 };
            return ddl;
        }

        public static List<SelectListItem> StatusFilterDDL()
        {
            var ddl = new List<SelectListItem>()
                {
                    new SelectListItem() { Text = "", Value = "-1"},
                    new SelectListItem() { Text = "Active", Value = "0"},
                    new SelectListItem() { Text = "Inactive", Value = "1"}
                 };
            return ddl;
        }

        public static List<SelectListItem> CreatePageTypeDDL()
        {
            var ddl = new List<SelectListItem>()
                  {
                       new SelectListItem() { Text = "String", Value = "String"},
                       new SelectListItem() { Text = "Bool", Value = "Bool"},
                       new SelectListItem() { Text = "Number", Value = "Number"},
                       new SelectListItem() { Text = "Currency", Value = "Currency"}
                 };
            return ddl;
        }
    }
}

