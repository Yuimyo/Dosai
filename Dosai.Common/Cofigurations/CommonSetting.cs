using Microsoft.Extensions.Configuration;

namespace Dosai.Common.Configurations
{
    public class CommonSetting
    {
        public CommonSeleniumSetting Seleniums { get; set; } = new CommonSeleniumSetting();
        public CommonVLCSetting VLCs { get; set; } = new CommonVLCSetting();

        public readonly static IConfigurationRoot Root = new ConfigurationBuilder()
            .AddJsonFile("common_settings.json")
            .Build();
    }
}
