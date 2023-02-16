namespace Dosai.Common.Videos.Viewers
{
    public interface IViewer : IDisposable
    {
        string Title { get; }
        bool Playing { get; set; }
        int Volume { get; set; }
        TimeSpan CurrentTime { get; set; }
        TimeSpan Duration { get; }
        void Close();
    }
}
