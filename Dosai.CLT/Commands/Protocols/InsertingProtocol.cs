using Serilog;
using Dosai.CLT.Commands.Utils;
using Dosai.Common.Utils;
using Dosai.Common.Videos;

namespace Dosai.CLT.Commands.Protocols
{
    public class InsertingProtocol : IProtocol
    {
        public string Description => "Load your video.";

        private readonly VideoController videoController;
        public InsertingProtocol(VideoController videoController)
        {
            this.videoController = videoController;
        }

        public void Run(Dictionary<string, string> kvs)
        {
            kvs.Must(out string url, "url", "u");
            kvs.Any(out TimeSpan? offset, "offset", "o");
            kvs.Any(out int? volume, "volume", "v");

            if (videoController.IsPlaying)
            {
                Log.Information("Videos are playing right now. Registering will be ignored.");
                return;
            }

            uint videoId = videoController.Register(
                () => VideoHelper.PickupVideo(url, offset ?? TimeSpan.Zero, volume ?? 100));
            var detail = videoController.GetDetail(videoId);
            var title = detail.Title.Omit(20);
            Log.Information("The video({Title}) is now registered (Id: [{VideoId}], Offset: {Offset}, Volume: {Volume}).",
                title, videoId, detail.Offset, detail.Volume);
        }
    }
}
