using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibVLCSharp.Shared;
using Microsoft.Extensions.Configuration;
using Dosai.Common.Utils;
using Dosai.Common.Configurations;

namespace Dosai.Common.Videos.Viewers
{
    public class VLCViewer : IViewer
    {
        private static readonly CommonVLCSetting config = CommonSetting.Root.GetSection(nameof(CommonSetting.VLCs)).Get<CommonVLCSetting>() ?? new CommonVLCSetting();

        // TODO: libVLCのDisposing？
        private static LibVLC? libVLC;
        private static bool loadedLibVLC = false;
        private static void tryToLoadLibVLC()
        {
            if (loadedLibVLC)
                return;
            loadedLibVLC = true;

            Core.Initialize();
            libVLC = new LibVLC(config.EnableDebugLogs);
        }

        private readonly MediaPlayer mediaPlayer;
        private readonly Media media;
        public VLCViewer(Uri uri)
        {
            tryToLoadLibVLC();
            if (libVLC == null)
                throw new NullReferenceException();

            this.media = new Media(libVLC, uri);
            this.mediaPlayer = new MediaPlayer(media);
            initialize();
        }
        
        private void initialize()
        {
            // TODO: 予約して何回かトライする。ダメならthrowしたほうがよさそう
            if (!media.IsParsed)
                this.media.Parse();
        }

        public string Title => media.GetFileTitle();

        public bool Playing 
        { 
            get
            {
                return mediaPlayer.IsPlaying;
            }
            set
            {
                if (value)
                {
                    if (!mediaPlayer.IsPlaying)
                        mediaPlayer.Play();
                }
                else
                {
                    if (mediaPlayer.IsPlaying)
                        mediaPlayer.Pause();
                }
            }
        }

        /// <summary>
        /// 0 = mute, 100 = 0db
        /// </summary>
        public int Volume
        {
            get
            {
                return mediaPlayer.Volume;
            }
            set
            {
                if (value < 0 || 100 < value)
                    throw new ArgumentOutOfRangeException();
                if (Playing)
                {
                    mediaPlayer.Pause();
                    mediaPlayer.Volume = value;
                    mediaPlayer.Play();
                }
                else
                {
                    mediaPlayer.Volume = value;
                }
            }
        }

        public TimeSpan CurrentTime
        {
            get
            {
                return TimeSpan.FromMilliseconds(mediaPlayer.Time);
            }
            set
            {
                mediaPlayer.Time = value.Milliseconds;
            }
        }

        public TimeSpan Duration => TimeSpan.FromMilliseconds(media.Duration);

        public void Close()
        {
            Dispose();
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

                if (Playing)
                    mediaPlayer.Stop();
                mediaPlayer.Dispose();
                media.Dispose();

                // TODO: 大きなフィールドを null に設定します
                disposedValue = true;
            }
        }

        // // TODO: 'Dispose(bool disposing)' にアンマネージド リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします
        // ~YoutubeWindow()
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
