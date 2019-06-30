using System.Net;
using System.Runtime.CompilerServices;

public static string GetScriptFolder([CallerFilePath] string path = null) => Path.GetDirectoryName(path);

new WebClient()
    .DownloadFile("https://yt-dl.org/downloads/2019.06.27/youtube-dl",
        Path.Combine(GetScriptFolder(), "..", "src", "YouTubeToPodcast", "youtube-dl"));