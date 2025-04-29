using System;
using System.Collections.Generic;
using System.Text;


namespace Data
{
    public static class CompanyEnums
    {
        public enum EmailType
        {
            Gmail = 1,
            Office365 = 2,
            Other = 3
        }

        public enum NoteType
        {
            General = 0

        }
        public enum SignatureTextType
        {
            Initial = 0,
            Signature = 1,
            Acknowledgement = 2
        }

        public enum RecipientType
        {
            TO = 1,
            CC = 2,
            BCC = 3
        }

        public enum EmailStatus
        {
            UnRead = 1,
            Read = 2
        }  
        public enum AgreementStatus
        {
            Draft = 0,
            Active = 1,
            Inactive = 2
        }
        public enum CompanyTimeZone
        {
            Central = 0,
            Eastern = 1,
            Mountain = -1,
            Pacific = -2
        }

        public static string TimeZoneDesc(CompanyTimeZone zone)
        {
            if (zone == CompanyTimeZone.Central)
                return "Central";
            if (zone == CompanyTimeZone.Eastern)
                return "Eastern";
            if (zone == CompanyTimeZone.Mountain)
                return "Mountain";
            if (zone == CompanyTimeZone.Pacific)
                return "Pacific";
            return "";
        }  
        public static string AgreementStatusDesc(AgreementStatus AgreementStatus)
        {
            if (AgreementStatus == AgreementStatus.Draft)
                return "Draft";
            if (AgreementStatus == AgreementStatus.Active)
                return "Active";
            if (AgreementStatus == AgreementStatus.Inactive)
                return "Inactive";         
            return "";
        }
    }
}

