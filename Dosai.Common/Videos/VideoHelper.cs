using Dosai.Common.Utils;
using Dosai.Common.Exceptions;
using Dosai.Common.Videos.Viewers;

namespace Dosai.Common.Videos
{
    public static class VideoHelper
    {
        public static Video PickupVideo(string url, TimeSpan offset, int volume)
        {
            var u = new Uri(url);
            switch (classifyVideoUri(u))
            {
                case VideoContentType.Youtube:
                    {
                        var q = UriHelper.ParseUrlQueries(u);
                        if (!q.ContainsKey("v")) throw new InvalidUrlException();
                        var ytVideoId = q["v"];
                        return new Video(new YoutubeBrowserViewer(ytVideoId), offset, volume);
                    }
                case VideoContentType.File:
                    {
                        return new Video(new VLCViewer(u), offset, volume);
                    }
                default:
                case VideoContentType.Null: throw new InvalidUrlException();
            }
        }

        private static VideoContentType classifyVideoUri(Uri u)
        {
            switch(u.GetLeftPart(UriPartial.Scheme)) 
            {
                case "file://":
                    return VideoContentType.File;
                default: break;
            }
            switch (u.GetLeftPart(UriPartial.Path))
            {
                case "https://www.youtube.com/watch":
                    {
                        return VideoContentType.Youtube;
                    }
                default:
                    {
                        return VideoContentType.Null;
                    }
            }
        }
    }

    public enum VideoContentType
    {
        Null,
        Youtube,
        File,
    }
}