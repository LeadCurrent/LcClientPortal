using System;

namespace CommonClasses
{
    public static class Environment
    {
        public static string environment = "Dev";

        public static string TenantId = "";
        public static string SiteEmail = "support@leadcurrent.com";
        public static string SiteName = "Lead Current";
        public static string SupportName = "Lead Current";
        public static string SupportEmail = "support@leadcurrent.com";
        public static string SupportPhone = "";
        public static string NoReplyEmail = "";
        public static string SendGridAPIKey = "";

        public static bool Microsoft = true;
        public static bool Gmail = true;
        public static bool SendTestEmail = false;
        public static string TestEmail = "";
        public static string SystemAPIKey = "jFuek*fsS#f934";
        public static string url()
        {
            if (environment.Contains("Dev"))
                return "https://localhost:44390/";
            else if (environment.Contains("Test"))
                return "https://localhost:44390/";
            else
                return "";
        }

        public static string DBConnection()
        {
            if (environment == "Dev")
                return "Server=tcp:lcdotnet.database.windows.net,1433;Database=LcClientPortal;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=true;User Id=lc-dotnet2024;Password=wQ7emaILDcxfw7q;max pool size=5000;";
            else if (environment == "Prod")
                return "Server=tcp:lcdotnet.database.windows.net,1433;Database=LcClientPortal;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=true;User Id=lc-dotnet2024;Password=wQ7emaILDcxfw7q;max pool size=5000;TransparentNetworkIPResolution=False;";

            else
                return "";
        }

        //public static string DBConnection()
        //{
        //    if (environment == "Dev")
        //        return "Server=(localdb)\\MSSQLLocalDB;Database=LcClientPortal;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=true;";
        //    else if (environment == "Test")
        //        return "Server=(localdb)\\MSSQLLocalDB;Database=LcClientPortal;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=true;";
        //    else
        //        return "";
        //}


        public static string MicrosoftClientId()
        {
            return "";
        }

        public static string MicrosoftClientSecret()
        {
            return "";
        }


        public static string GoogleClientId()
        {
            if (environment.Contains("Dev"))
                return "843118869333-anmt2cmc86hq7c8cuibvh2r1mjss20qm.apps.googleusercontent.com";
            else
                return "";
        }

        public static string GoogleClientSecret()
        {
            if (environment.Contains("Dev"))
                return "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";
            else
                return "";
        }

        public static string GoogleApplicationName()
        {
            return "";
        }

        public static bool SendNotifications()
        {
            //if (environment.Contains("Prod"))
            return true;
            //else
            //    return false;
        }

        public static string StorageContainer()
        {
            return "company";
        }

        public static string BaseStorageURL()
        {
                return "https://lcadminportal.blob.core.windows.net/";
        }

        public static string StorageURLWithoutSlash()
        {
            return "https://lcadminportal.blob.core.windows.net";
        }

        public static string StorageAccount()
        {
            return "DefaultEndpointsProtocol=https;AccountName=lcadminportal;AccountKey=hsUU26Gu6S2aYBPec+sj3AYuHLaS8t0udEXPzcJG4jmW8JUwkbUE2W0UBfgr76HRRv4leUh50ZLN+AStwMfNtg==;EndpointSuffix=core.windows.net";
        }

        public static string StorageAccountName()
        {
            return "lcadminportal";
        }

        public static string StorageAccountKey()
        {
            return "hsUU26Gu6S2aYBPec+sj3AYuHLaS8t0udEXPzcJG4jmW8JUwkbUE2W0UBfgr76HRRv4leUh50ZLN+AStwMfNtg==";
        }

        public static string StorageURL()
        {
            return BaseStorageURL() + StorageContainer() + "/";
        }


        public static bool Customers
        {
            get
            {

                return true;
            }
        }
        public static bool CompanyAdmin
        {
            get
            {

                return true;
            }
        }
        public static bool Templates
        {
            get
            {
                return true;
            }
        }
        public static bool Email
        {
            get
            {
                return true;
            }
        }
        public static bool Documents
        {
            get
            {
                return true;
            }
        }
        public static bool Sources
        {
            get
            {
                return true;
            }
        }
        public static bool Schools
        {
            get
            {
                return true;
            }
        }
        public static bool CompanyProfile
        {
            get
            {
                return true;
            }
        }
        public static bool Users
        {
            get
            {
                return true;
            }
        }

    }
}
