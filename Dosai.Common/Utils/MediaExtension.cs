using LibVLCSharp.Shared;

namespace Dosai.Common.Utils
{
    public static class MediaExtension
    {
        public static string GetFileTitle(this Media media)
        {
            if (media.Type != MediaType.File)
                throw new InvalidDataException();

            var uri = new Uri(media.Mrl);
            return Path.GetFileName(uri.LocalPath);
        }
    }
}
