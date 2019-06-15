namespace YouTubeToPodcast
{
    public static class Constants
    {
        public static class Cache
        {
            public const int MinutesTillExpiration = 30;
        }

        public static class YouTube
        {
            public const int MaxResults = 50;
            public const int MaxRequests = 10;
            public const string AudioFileExtension = ".m4a";
            public const string AudioMimeType = "audio/m4a";
        }
    }
}