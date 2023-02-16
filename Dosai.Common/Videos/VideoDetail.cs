namespace Dosai.Common.Videos
{
    public struct VideoDetail
    {
        public uint? VideoId;
        public string Title;
        public int Volume;
        public TimeSpan Offset;
        public TimeSpan CurrentTime;
        public TimeSpan Duration;

        public static VideoDetail FromVideo(IVideo video)
        {
            return new VideoDetail()
            {
                VideoId = null,
                Title = video.Title,
                Volume = video.Volume,
                Offset = video.Offset,
                CurrentTime = video.CurrentTime,
                Duration = video.Duration,
            };
        }
    }
}
