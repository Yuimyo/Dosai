namespace Dosai.Common.Videos
{
    public interface IVideo : IDisposable
    {
        string Title { get; }
        int Volume { get; set; }
        TimeSpan Offset { get; set; }
        TimeSpan CurrentTime { get; set; }
        TimeSpan Duration { get; }

        void Play();
        void Stop();
        void Reload();
    }
}
