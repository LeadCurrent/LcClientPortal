
namespace Data
{
   public static class DocumentEnums
    {
        public enum DocumentFileType
        {
            Document = 0,
            Image = 1,
            Video = 2,
            Link = 3
        }
        public enum AccessType
        {
            AllUserAccess = 0,
            RestrictAccess = 1
        }
        public enum IconType
        {
            txt = 1,
            xlsx = 2,
            doc = 3,
            ppt = 4,
            pdf = 5,
            png = 6,
            jpg = 7,
            jpeg = 8,
            mp4 = 9,
            zip = 10
        }

        public enum VideoUploadType
        {
            UploadNewVideo = 0,
            YouTubeVideo = 1
        }
        public static string VideoUploadTypeDisplay(VideoUploadType VideoUploadType)
        {
            if (VideoUploadType == VideoUploadType.UploadNewVideo) return "Upload New Video";
            if (VideoUploadType == VideoUploadType.YouTubeVideo) return "YouTube Video";
            return "";
        }
    }
}

