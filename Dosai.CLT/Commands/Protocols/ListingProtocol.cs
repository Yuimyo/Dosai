using Serilog;
using Dosai.CLT.Commands.Utils;
using Dosai.Common.Utils;
using Dosai.Common.Videos;

namespace Dosai.CLT.Commands.Protocols
{
    internal class ListingProtocol : IProtocol
    {
        public string Description => "Lists information about loaded videos.";

        private readonly VideoController videoController;
        public ListingProtocol(VideoController videoController)
        {
            this.videoController = videoController;
        }

        public void Run(Dictionary<string, string> kvs)
        {
            Log.Information($"Videos will be listed below:");
            foreach (var d in videoController.GetDetailAll())
                Log.Information("[{VideoId}]({CurrentTime}/{Duration} {Volume}) {Title}",
                    d.VideoId, d.CurrentTime.ToString(@"hh\:mm\:ss"), d.Duration.ToString(@"hh\:mm\:ss"), d.Volume, d.Title.Omit(50));
            Log.Information($"-----------------");
        }
    }
}