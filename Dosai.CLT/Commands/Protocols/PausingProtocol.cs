using Serilog;
using Dosai.CLT.Commands.Utils;
using Dosai.Common.Utils;
using Dosai.Common.Videos;

namespace Dosai.CLT.Commands.Protocols
{
    internal class PausingProtocol : IProtocol
    {
        public string Description => "Pause the video.";

        private readonly VideoController videoController;
        public PausingProtocol(VideoController videoController)
        {
            this.videoController = videoController;
        }

        public void Run(Dictionary<string, string> kvs)
        {
            videoController.Stop();
        }
    }
}