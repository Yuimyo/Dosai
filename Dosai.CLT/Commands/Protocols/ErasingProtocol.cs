using Serilog;
using Dosai.CLT.Commands.Utils;
using Dosai.Common.Utils;
using Dosai.Common.Videos;
using OpenQA.Selenium;

namespace Dosai.CLT.Commands.Protocols
{
    internal class ErasingProtocol : IProtocol
    {
        public string Description => "Forget the selected loaded video.";

        private readonly VideoController videoController;
        public ErasingProtocol(VideoController videoController)
        {
            this.videoController = videoController;
        }

        public void Run(Dictionary<string, string> kvs)
        {
            kvs.Must(out uint videoId, "video_id", "id");

            if (videoController.HasVideo(videoId))
            {
                var title = videoController.GetDetail(videoId).Title.Omit(20);
                videoController.Unregister(videoId);
                Log.Information("The video({Title}) is unregistered. That's id was [{VideoId}].",
                    title, videoId);
            }
            else
            {
                Log.Information($"No matches found for that id.");
            }
        }
    }
}