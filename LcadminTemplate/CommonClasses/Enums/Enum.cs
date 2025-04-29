using Microsoft.AspNetCore.Mvc.Rendering;
using static CommonClasses.Enums;

namespace CommonClasses
{
    public class Enums
    {

        public enum RecipientType
        {
            TO = 1,
            CC = 2,
            BCC = 3
        }
        public enum CompanyTimeZone
        {
            Central = 0,
            Eastern = 1,
            Mountain = -1,
            Pacific = -2
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
    
    }


}

