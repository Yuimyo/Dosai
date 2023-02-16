using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dosai.Common.Exceptions;

namespace Dosai.Common.Videos
{
    public class VideoController : IDisposable
    {
        private readonly Dictionary<uint, IVideo> videos = new Dictionary<uint, IVideo>();

        public VideoController()
        {
        }

        private bool playing = false;
        public bool IsPlaying => playing;

        private uint latestVideoId = 0;
        public uint Register(Func<IVideo> videoConstructor)
        {
            var videoId = latestVideoId++;
            var video = videoConstructor(); 
            videos[videoId] = video;
            return videoId;
        }

        public bool HasVideo(uint videoId)
            => videos.ContainsKey(videoId);

        public VideoDetail GetDetail(uint videoId)
        {
            if (!HasVideo(videoId)) 
                throw new VideoNotFoundException();
            var detail = VideoDetail.FromVideo(videos[videoId]);
            detail.VideoId = videoId;
            return detail;
        }

        public IEnumerable<VideoDetail> GetDetailAll()
        {
            foreach (var videoId in videos.Keys)
                yield return GetDetail(videoId);   
        }

        public void Unregister(uint videoId)
        {
            if (!HasVideo(videoId)) 
                throw new VideoNotFoundException();
            videos[videoId].Dispose();
            videos.Remove(videoId);
        }

        public void Play()
        {
            foreach (var video in videos.Values)
                video.Play();
        }
        public void Stop()
        {
            foreach (var video in videos.Values)
                video.Stop();
            playing = false;
        }
        public void StartOver()
        {
            Stop();
            foreach (var video in videos.Values)
                video.Reload();
        }

        #region IDisposable
        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: マネージド状態を破棄します (マネージド オブジェクト)
                }

                // TODO: アンマネージド リソース (アンマネージド オブジェクト) を解放し、ファイナライザーをオーバーライドします
                foreach (var videoId in videos.Keys)
                    Unregister(videoId);
                // TODO: 大きなフィールドを null に設定します
                disposedValue = true;
            }
        }

        // // TODO: 'Dispose(bool disposing)' にアンマネージド リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします
        // ~VideoController()
        // {
        //     // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
