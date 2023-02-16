using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dosai.Common.Videos;
using Dosai.Common.Videos.Viewers;
using Xunit.Abstractions;

namespace Dosai.Test
{
    public class SeleniumTest
    {
        private readonly ITestOutputHelper output;
        public SeleniumTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void testNameof()
        {
            var name = nameof(Dosai.Common.Configurators.CommonSetting.Seleniums);
            Assert.Equal("Seleniums", name);
            output.WriteLine(name);
        }

        [Fact]
        public void playingYoutbe()
        {
            //var random = new Random();
            //using (var youtube = new YoutubeBrowserViewer("-HLRjWNC5iA"))
            //{
            //    youtube.Playing = true;
            //    var pl = true;
            //    while (true)
            //    {
            //        Console.ReadLine();
            //        var volume = random.Next(0, 100);
            //        var currentTime = new TimeSpan(0, 0, random.Next(100, 1000));
            //        youtube.CurrentTime = currentTime;
            //        Console.WriteLine($"Shifted current time to {currentTime}.");
            //        youtube.Volume = volume;
            //        Console.WriteLine($"Shifted volume to {volume} {youtube.Volume}.");
            //        youtube.Playing = pl = !pl;
            //        Console.WriteLine($"Shifted play/pause to {youtube.Playing}.");
            //        //youtube.GetCurrentTime();
            //        //youtube.GetDuration();
            //        //youtube.ClickPlay();
            //    }
            //}
        }
    }
}
