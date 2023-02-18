using Serilog;
using Dosai.CLT.Commands.Utils;
using Dosai.Common.Utils;
using Dosai.Common.Videos;

namespace Dosai.CLT.Commands.Protocols
{
    internal class ChangingProtocol : IProtocol
    {
        public string Description => "Change the information for the selected loaded video.";

        private readonly VideoController videoController;
        public ChangingProtocol(VideoController videoController)
        {
            this.videoController = videoController;
        }

        public void Run(Dictionary<string, string> kvs)
        {
            kvs.Must(out uint videoId, "video_id", "id");
            kvs.Any(out TimeSpan? offset, "offset", "o");
            kvs.Any(out int? volume, "volume", "v");

            var changed = false;
            if (!videoController.HasVideo(videoId))
            {
                Log.Information("That video([{VideoId}]) doesn't exist.",
                    videoId);
                return;
            }
            var detail = videoController.GetDetail(videoId);
            if (videoController.IsPlaying)
            {
                Log.Information("Videos are playing right now. Some command's sub order maybe ignored.");
            }
            else
            {
                if (offset != null)
                {
                    var value = offset ?? TimeSpan.Zero;
                    if (detail.Duration < value)
                    {
                        Log.Information("The offset({Value}) is larger than video's duration({Duration})! This manipulation will be ignored.",
                            value, detail.Duration);
                        Log.Information($"(*Format suggestion: How about \"HH:MM:SS\"?)");
                    }
                    else
                    {
                        videoController.SetOffset(videoId, value);
                        changed = true;
                        Log.Information("The video's([{VideoId}]) offset is succesfully changed({Offset}).",
                            videoId, value);
                    }
                }
            }

            if (volume != null)
            {
                var value = volume ?? 0;
                if (value < 0 || 100 < value)
                {
                    Log.Information("Volume value({Volume}) is out of range. This manipulation will be ignored.",
                        value);
                }
                else
                {
                    videoController.SetVolume(videoId, value);
                    changed = true;
                    Log.Information("The video's([{VideoId}]) volume is succesfully changed({Volume}).",
                        videoId, value);
                }
            }
            if (!changed)
            {
                Log.Information("The video([{VideoId}]) changed nothing.",
                    videoId);
            }
        }
    }
}