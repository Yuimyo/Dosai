using Dosai.Common.Videos.Viewers;

namespace Dosai.Common.Configurators
{
    public class CommonSeleniumSetting
    {
        public BrowserBrand SeleniumBrowserBrand { get; set; } = BrowserBrand.Chrome;
        public uint TimeoutSec { get; set; }
        public string ChromeDriverDirectory { get; set; } = "";
        public bool ChromeUsesCrx { get; set; }
        public string ChromeCrxDirectory { get; set; } = "";
        public bool ChromeIsHeadless { get; set; }
        public string EdgeDriverDirectory { get; set; } = "";
        public bool EdgeIsHeadless { get; set; }
        public string FirefoxDriverDirectory { get; set; } = "";
        public bool FirefoxIsHeadless { get; set; }
    }
}