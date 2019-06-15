namespace YouTubeToPodcast.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveSuffix(this string name, string suffix)
            => name.EndsWith(suffix)
            ? name.Substring(0, name.Length - suffix.Length)
            : name;
    }
}