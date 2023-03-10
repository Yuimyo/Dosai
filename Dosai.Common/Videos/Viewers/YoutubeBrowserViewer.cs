using Microsoft.Extensions.Configuration;
using Serilog;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using Dosai.Common.Utils;
using Dosai.Common.Configurations;
using static System.Net.Mime.MediaTypeNames;

namespace Dosai.Common.Videos.Viewers
{
    public class YoutubeBrowserViewer : IViewer
    {
        private static readonly CommonSeleniumSetting config = CommonSetting.Root.GetSection(nameof(CommonSetting.Seleniums)).Get<CommonSeleniumSetting>() ?? new CommonSeleniumSetting();

        private readonly TimeSpan timeout = TimeSpan.FromSeconds(config.TimeoutSec);
        private WebDriver driver;
        private string youtubeVideoId = string.Empty;
        public string YoutubeVideoId => youtubeVideoId;

        public string FormattedUrl => $"https://www.youtube.com/watch?v={youtubeVideoId}";

        public YoutubeBrowserViewer(string ytVideoId)
        {
            this.youtubeVideoId = ytVideoId;
            this.driver = prepareDriver();
            setupYoutube();
        }

        #region Preparing driver
        private WebDriver prepareDriver()
        {
            switch (config.SeleniumBrowserBrand)
            {
                default:
                case BrowserBrand.Chrome: return prepareChromeDriver();
                case BrowserBrand.Edge: return prepareEdgeDriver();
                case BrowserBrand.Firefox: return prepareFirefoxDriver();
            }
        }
        private ChromeDriver prepareChromeDriver()
        {
            var service = ChromeDriverService.CreateDefaultService(config.ChromeDriverDirectory);
            var option = new ChromeOptions();
            if (config.ChromeUsesCrx)
            {
                foreach (var filePath in Directory.GetFiles(config.ChromeCrxDirectory, "*.crx", SearchOption.TopDirectoryOnly))
                    option.AddExtension(filePath);
            }
            if (config.ChromeIsHeadless) 
                option.AddArgument("--headless=new");
            return new ChromeDriver(service, option);
        }
        private EdgeDriver prepareEdgeDriver()
        {
            var service = EdgeDriverService.CreateDefaultService(config.EdgeDriverDirectory);
            var option = new EdgeOptions();
            if (config.EdgeIsHeadless)
                option.AddArgument("headless");
            return new EdgeDriver(service, option);
        }
        private FirefoxDriver prepareFirefoxDriver()
        {
            var service = FirefoxDriverService.CreateDefaultService(config.FirefoxDriverDirectory);
            var option = new FirefoxOptions();
            if (config.FirefoxIsHeadless)
                option.AddArgument("headless");
            return new FirefoxDriver(service, option);
        }
        #endregion

        private void setupYoutube()
        {
            var wait = new WebDriverWait(driver, timeout);
            wait.Until(driver => driver.Url = $"{FormattedUrl}");
        }

        public string Title
        {
            get
            {
                var ytTitle = driver.Title;
                return ytTitle.Substring(0, ytTitle.Length - 10);
            }
        }
        public bool Playing
        {
            get
            {
                if (disposedValue)
                    return false;

                // What the damn code here...
                IWebElement? playingEl = null;
                try
                {
                    playingEl = driver.FindElement(By.ClassName("playing-mode"));
                }
                catch (NoSuchElementException) { }

                if (playingEl != null)
                    return true;
                else
                    return false;
            }
            set
            {
                if (disposedValue)
                    return;

                if (value) Play();
                else Stop();
            }
        }

        public void Play()
        {
            if (disposedValue)
                return;

            if (Playing) return;
            else clickPlay();
        }
        public void Stop()
        {
            if (disposedValue)
                return;

            if (!Playing) return;
            else clickPlay();
        }

        private void clickPlay()
        {
            var wait = new WebDriverWait(driver, timeout);
            wait.Until<IWebElement?>(driver =>
            {
                var video = driver.FindElement(By.CssSelector("div[id='movie_player']"));
                // var playButton = driver.FindElement(By.CssSelector("button[class='ytp-play-button ytp-button']"));
                // var video2 = driver.FindElement(By.CssSelector("video[class='video-stream html5-main-video']"));
                var current = Playing;
                video.Click();
                for (int i = 0; i < 15; i++)
                {
                    Thread.Sleep(200);
                    if (current != Playing) break;
                }
                return video;
            });
        }

        public int Volume
        {
            set
            {
                if (disposedValue)
                    return;

                var volume = MathHelper.Clamp(value, 0, 100);
                IWebElement? soundButton = null, volumePanel = null;
                for (int i = 0; i < 5; i++)
                {
                    try
                    {
                        soundButton = driver.FindElement(By.ClassName("ytp-mute-button"));
                        volumePanel = driver.FindElement(By.ClassName("ytp-volume-panel"));
                    }
                    catch (NoSuchElementException)
                    {
                        Thread.Sleep(200);
                        continue;
                    }
                    break;
                }

                var actions = new Actions(driver);
                actions.MoveToElement(soundButton);
                actions.MoveToElement(volumePanel);
                actions.ClickAndHold(volumePanel);
                actions.MoveByOffset((int)Math.Round(MathHelper.Lerp(-20, 20, volume / 100d)), 0);
                actions.Release();
                actions.Perform();
                driver.ResetInputState();
            }
            get
            {
                var text = driver.FindElement(By.ClassName("ytp-volume-panel")).GetAttribute("aria-valuenow").Trim();
                return int.Parse(text);
            }
        }

        public TimeSpan CurrentTime
        {
            get
            {
                if (disposedValue)
                    return TimeSpan.Zero;

                IWebElement currentTimeEl = driver.FindElement(By.CssSelector("span[class='ytp-time-current']"));

                // For updating current time value.
                var actions = new Actions(driver);
                actions.MoveToElement(currentTimeEl);
                actions.MoveByOffset(1, 0);
                actions.Perform();

                var text = currentTimeEl.GetAttribute("textContent").Trim();
                var values = text.Split(":");
                int hours = 0, minutes = 0, seconds = 0;
                if (values.Length == 3)
                {
                    hours = StringParser.ToInt(values[0]);
                    minutes = StringParser.ToInt(values[1]);
                    seconds = StringParser.ToInt(values[2]);
                }
                else if (values.Length == 2)
                {
                    minutes = StringParser.ToInt(values[0]);
                    seconds = StringParser.ToInt(values[1]);
                }
                else
                {
                    Log.Error("Invalid current time value has fetched (\"{CurrentTime}\").",
                        text);
                    throw new InvalidDataException();
                }

                return new TimeSpan(hours, minutes, seconds);
            }
            set
            {
                if (disposedValue)
                    return;

                uint sec = (uint)Math.Round(value.TotalSeconds);
                var wait = new WebDriverWait(driver, timeout);
                wait.Until(driver => driver.Url = $"{FormattedUrl}&t={sec}");
                for (int i = 0; i < 5; i++)
                {
                    Stop();
                    Thread.Sleep(200);
                }
            }
        }

        public TimeSpan Duration
        {
            get
            {
                if (disposedValue)
                    return TimeSpan.Zero;

                var text = driver.FindElement(By.CssSelector("span[class='ytp-time-duration']")).GetAttribute("textContent").Trim();
                var values = text.Split(":");
                int hours = 0, minutes = 0, seconds = 0;
                if (values.Length == 3)
                {
                    hours = StringParser.ToInt(values[0]);
                    minutes = StringParser.ToInt(values[1]);
                    seconds = StringParser.ToInt(values[2]);
                }
                else if (values.Length == 2)
                {
                    minutes = StringParser.ToInt(values[0]);
                    seconds = StringParser.ToInt(values[1]);
                }
                else
                {
                    Log.Error("Invalid duration value has fetched (\"{Duration}\").",
                        text);
                    throw new InvalidDataException();
                }

                return new TimeSpan(hours, minutes, seconds);
            }
        }

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
                if (driver != null)
                    driver.Quit();
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
