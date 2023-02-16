using Serilog;
using Dosai.CLT.Commands.Utils;
using Dosai.Common.Utils;
using Dosai.Common.Videos;

namespace Dosai.CLT.Commands.Protocols
{
    internal class BackingProtocol : IProtocol
    {
        public string Description => "Returns the playback position of the video to the beginning.";

        private readonly VideoController videoController;
        public BackingProtocol(VideoController videoController) 
        {
            this.videoController = videoController;
        }

        public void Run(Dictionary<string, string> kvs)
        {
            videoController.StartOver();
        }
    }
}