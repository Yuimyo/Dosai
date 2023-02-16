using Serilog;
using Dosai.CLT.Commands.Utils;
using Dosai.Common.Utils;
using Dosai.Common.Videos;

namespace Dosai.CLT.Commands.Protocols
{
    internal class PlayingProtocol : IProtocol
    {
        public string Description => "Play the video.";

        private readonly VideoController videoController;
        public PlayingProtocol(VideoController videoController)
        {
            this.videoController = videoController;
        }

        public void Run(Dictionary<string, string> kvs)
        {
            videoController.Play();
        }
    }
}