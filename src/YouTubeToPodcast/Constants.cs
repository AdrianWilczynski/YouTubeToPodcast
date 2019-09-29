namespace YouTubeToPodcast
{
    public static class Constants
    {
        public static class CacheExpirationTimeInMinutes
        {
            public const int Feed = 60;
            public const int Url = 10;
        }

        public static class YouTube
        {
            public const int MaxResults = 50;
            public const int MaxRequests = 1;
            public const string AudioFileExtension = ".m4a";
            public const string AudioMimeType = "audio/m4a";
        }
    }
}