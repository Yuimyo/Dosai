using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Dosai.Common.Videos;
using Dosai.Common.Utils;
using Dosai.Common.Videos.Viewers;

namespace Dosai.Common.Videos
{
    public class Video : IVideo
    {
        private readonly IViewer viewer;
        public Video(IViewer viewer, TimeSpan offset, int volume)
        {
            this.viewer = viewer;
            this.Offset = offset;
            this.Volume = volume;
            Reload();
        }

        public string Title => viewer.Title;

        public int Volume
        {
            get { return viewer.Volume; }
            set { viewer.Volume = value; }
        }

        private TimeSpan offset = TimeSpan.Zero;
        public TimeSpan Offset
        {
            get { return offset; }
            set { offset = value; }
        }

        public TimeSpan CurrentTime
        {
            get { return viewer.CurrentTime; }
            set { viewer.CurrentTime = value; }
        }

        public TimeSpan Duration => viewer.Duration;

        public void Play()
        {
            if (disposedValue)
                return;

            viewer.Playing = true;
        }

        public void Stop()
        {
            if (disposedValue)
                return;

            viewer.Playing = false;
        }
        public void Reload()
        {
            this.CurrentTime = this.Offset;
        }

        #region IDisposable
        protected bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: マネージド状態を破棄します (マネージド オブジェクト)
                    viewer.Close();
                }

                // TODO: アンマネージド リソース (アンマネージド オブジェクト) を解放し、ファイナライザーをオーバーライドします
                // TODO: 大きなフィールドを null に設定します
                disposedValue = true;
            }
        }

        // // TODO: 'Dispose(bool disposing)' にアンマネージド リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします
        // ~Video()
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
